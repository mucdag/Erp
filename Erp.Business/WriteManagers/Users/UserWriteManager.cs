using AutoMapper;
using Core.Aspects.Autofac.ValidationAspects;
using Core.CrossCuttingConcerns.AppSecurity;
using Core.Utilities.IoC;
using Erp.Data.Models.Users;
using Erp.Data.Repositories.WriteRepository.People;
using Erp.Data.Repositories.WriteRepository.Users;
using Erp.Resource.Enums;
using Erp.Resource.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Erp.Core.TokenService;
using Erp.View.Models.Users;
using Microsoft.Extensions.Options;
using Core.Settings;
using Erp.Core.Utilities;
using Core.Utilities;
using Erp.Resource.Validations.Users;

namespace Erp.Business.WriteManagers.Users
{
    public class UserWriteManager : IUserWriteManager
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPersonEmailAddressWriteRepository _personEmailAddressWriteRepository;
        private readonly IAccountWriteManager _accountWriteManager;
        private readonly ITokenService _tokenService;

        private readonly int _passwordResetExpiryDate = 0;

        public UserWriteManager(IUserWriteRepository userWriteRepository, IPersonEmailAddressWriteRepository personEmailAddressWriteRepository, IAccountWriteManager accountWriteManager,
            ITokenService tokenService,
            IOptions<AppSettings> options)
        {
            _userWriteRepository = userWriteRepository;
            _personEmailAddressWriteRepository = personEmailAddressWriteRepository;
            _accountWriteManager = accountWriteManager;
            _tokenService = tokenService;
            _passwordResetExpiryDate = options.Value.PasswordResetExpiryDate;
        }

        [FluentValidationAspect(typeof(UserResourceValidation), ValidationRuleType.Insert)]
        public async Task AddAsync(UserResource resource)
        {
            var personEmailAdress = await _personEmailAddressWriteRepository.GetByIdAsync(resource.PersonEmailAddressId);
            if (personEmailAdress == null)
            {
                throw new Exception("EMail adresi bulunamadı");
            }

            if (personEmailAdress.PersonId != resource.PersonId)
            {
                throw new Exception("Bu email adresi farklı bir kişiye aittir.");
            }

            var isUserExists = await _userWriteRepository.IsExist(x => x.PersonId == resource.PersonId);
            if (isUserExists)
            {
                throw new Exception("Bu kullanıcıya ait farklı bir hesap bulunmaktadır");
            }

            var user = Mapper.Map<User>(resource);

            user.IsActive = true;

            user.Password = BCrypt.Net.BCrypt.HashPassword(resource.Password);

            await _userWriteRepository.AddAsync(user);
            resource.Id = user.Id;
        }

        [FluentValidationAspect(typeof(UserResourceValidation), ValidationRuleType.Update)]
        public async Task UpdateAsync(UserResource resource)
        {
            var personEmailAdress = await _personEmailAddressWriteRepository.GetByIdAsync(resource.PersonEmailAddressId);
            if (personEmailAdress == null)
            {
                throw new Exception("EMail adresi bulunamadı");
            }

            if (personEmailAdress.PersonId != resource.PersonId)
            {
                throw new Exception("Bu email adresi farklı bir kişiye aittir.");
            }

            var isUserExists = await _userWriteRepository.IsExist(x => x.PersonId == resource.PersonId && x.Id != resource.Id);
            if (isUserExists)
            {
                throw new Exception("Bu kullanıcıya ait farklı bir hesap bulunmaktadır");
            }

            var user = Mapper.Map<User>(resource);

            user.Password = BCrypt.Net.BCrypt.HashPassword(resource.Password);

            user.IsActive = true;
            await _userWriteRepository.UpdateAsync(user);
        }

        [FluentValidationAspect(typeof(UserConfirmResourceValidation), ValidationRuleType.Update)]
        public async Task<LoggedUserView> LoginAsync(UserConfirmResource userResource)
        {
            var user = await _userWriteRepository.FirstOrDefaultAsync(x => x.PersonEmailAddress.EmailAddress.Equals(userResource.EmailAddress));
            if (user == null || !user.IsActive)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            // verify password
            if (!BCrypt.Net.BCrypt.Verify(userResource.Password, user.Password))
                throw new Exception("Geçersiz email adresi ya da şifre", new AuthenticationException());

            var token = _tokenService.BuildToken(new ErpIdentity
            {
                Name = userResource.EmailAddress,
                User = user.Id.ToString(),
                PersonId = user.PersonId,
                IsAuthenticated = true
            });

            var view = await _userWriteRepository.GetLoggedUserViewByIdAsync(user.Id);
            view.AccessToken = token;

            return view;
        }

        [FluentValidationAspect(typeof(PasswordChangeResourceValidation), ValidationRuleType.Update)]
        public async Task PasswordChangeAsync(PasswordChangeResource passwordChangeResource)
        {
            var user = await _userWriteRepository.GetByIdAsync(CurrentUser.UserIdentity.Name.ToInt());
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            if (!BCrypt.Net.BCrypt.Verify(passwordChangeResource.OldPassword, user.Password))
            {
                throw new Exception("Geçersiz şifre");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(passwordChangeResource.NewPassword);
            await _userWriteRepository.UpdateAsync(user);
        }

        [FluentValidationAspect(typeof(PasswordResetResourceValidation), ValidationRuleType.Reset)]
        public async Task PasswordResetAsync(PasswordResetResource passwordResetResource)
        {
            var user = await _userWriteRepository.FirstOrDefaultAsync(x => x.PersonEmailAddress.EmailAddress.Equals(passwordResetResource.EmailAddress) && x.IsActive);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            if (user.PasswordResetExpiryDate == null || user.PasswordResetExpiryDate < DateTime.Now || !user.PasswordResetCode.Equals(passwordResetResource.ResetCode))
            {
                throw new Exception("Geçerisiz işlem!");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(passwordResetResource.Password);

            user.PasswordResetCode = null;
            user.PasswordResetExpiryDate = null;
            user.IsPasswordShouldBeReset = false;

            await _userWriteRepository.UpdateAsync(user);
        }

        [FluentValidationAspect(typeof(PasswordResetResourceValidation), ValidationRuleType.ResetRequest)]
        public async Task PasswordResetRequestAsync(PasswordResetResource passwordResetResource)
        {
            var user = await _userWriteRepository.FirstOrDefaultAsync(x => x.PersonEmailAddress.EmailAddress.Equals(passwordResetResource.EmailAddress) && x.IsActive);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            user.PasswordResetCode = Guid.NewGuid().ToString();
            user.PasswordResetExpiryDate = DateTime.Now.AddHours(_passwordResetExpiryDate);

            if (EmailHelper.IsValid(passwordResetResource.EmailAddress))
            {
                await _accountWriteManager.SendPasswordResetEmailCodeAsync(passwordResetResource.EmailAddress, user.PasswordResetCode, user.PasswordResetExpiryDate.Value);
            }

            await _userWriteRepository.UpdateAsync(user);
        }

    }
}
