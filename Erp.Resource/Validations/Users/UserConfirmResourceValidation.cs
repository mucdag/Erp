using FluentValidation;
using Erp.Resource.Enums;
using Erp.Resource.Models.Users;

namespace Erp.Resource.Validations.Users
{
    public class UserConfirmResourceValidation : AbstractValidator<UserConfirmResource>
    {
        public const int DIGIT_COUNT = 2;
        public const int LETTER_COUNT = 2;
        public const int PASSWORD_LENGTH = 6;

        public UserConfirmResourceValidation()
        {
            RuleSet(ValidationRuleType.Update, Update);
        }

        public void Update()
        {
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email boş olamaz!");

            RuleFor(x => x.Password)
                        .NotEmpty()
                        .WithMessage("Şifre boş olamaz!")
                        .MinimumLength(PASSWORD_LENGTH)
                        .WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!")
                        .Must(PasswordValidationControl)
                        .WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Şifre boş olamaz!")
                   .MinimumLength(PASSWORD_LENGTH).WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!");

            RuleFor(x => x.ConfirmPassword).Must(PasswordValidationControl)
                .WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir")
                .When(x => x.ConfirmPassword != null && x.ValidatePassword);

            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Geçersiz şifre!");
        }

        private bool PasswordValidationControl(string password)
        {
            var digitCount = password.Count(char.IsDigit);
            var letterCount = password.Count(char.IsLetter);
            return digitCount >= DIGIT_COUNT && letterCount >= LETTER_COUNT;
        }

    }
}
