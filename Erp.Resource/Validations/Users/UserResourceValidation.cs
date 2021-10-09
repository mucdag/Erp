using Erp.Resource.Enums;
using Erp.Resource.Interfaces;
using Erp.Resource.Models.Users;
using FluentValidation;

namespace Erp.Resource.Validations.Users
{
    public class UserResourceValidation : AbstractValidator<UserResource>, IValidationRules
    {
        public const int DIGIT_COUNT = 2;
        public const int LETTER_COUNT = 2;
        public const int PASSWORD_LENGTH = 6;

        public UserResourceValidation()
        {
            RuleSet(ValidationRuleType.Login, () => Login());
            RuleSet(ValidationRuleType.LoginRequest, () => LoginRequest());
            RuleSet(ValidationRuleType.Insert, () => Insert());
            RuleSet(ValidationRuleType.Delete, () => Delete());
            RuleSet(ValidationRuleType.Update, () => Update());
        }

        public void Insert()
        {
            RuleFor(x => x.Id).Empty().WithMessage("Id Alanı Boş olmalı");
            RulesOfEdit();
        }

        public void Update()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Id Alanı Boş olmamalı");
            RulesOfEdit();
        }

        public void Delete()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Id Alanı Boş olmamalı");
        }

        private void RulesOfEdit()
        {
            RuleFor(x => x.PersonId).NotEmpty().GreaterThan(0).WithMessage("Kişi Alanı Boş olmamalı");
            RuleFor(x => x.PersonEmailAddressId).NotEmpty().GreaterThan(0).WithMessage("Mail Alanı Boş olmamalı");

        }

        private void LoginRequest()
        {
            PasswordValidation();
        }

        private void Login()
        {
            PasswordValidation();
        }

        private void PasswordValidation()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz!").MinimumLength(PASSWORD_LENGTH).WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!")
                .Must(PasswordValidationControl).WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir");
        }

        private bool PasswordValidationControl(string password)
        {
            var digitCount = password.Count(char.IsDigit);
            var letterCount = password.Count(char.IsLetter);
            return digitCount >= DIGIT_COUNT && letterCount >= LETTER_COUNT;
        }

    }
}