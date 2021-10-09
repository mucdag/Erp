using FluentValidation;
using Erp.Resource.Enums;
using Erp.Resource.Models.Users;

namespace Erp.Resource.Validations.Users
{
    public class PasswordChangeResourceValidation : AbstractValidator<PasswordChangeResource>
    {
        public const int DIGIT_COUNT = 2;
        public const int LETTER_COUNT = 2;
        public const int PASSWORD_LENGTH = 6;

        public PasswordChangeResourceValidation()
        {
            RuleSet(ValidationRuleType.Update, () => Update());
        }

        public void Update()
        {
            RequireOldPassword();
            RequireNewPassword();
            RequireConfirmNewPassword();
            ConfirmNewPassword();
        }

        private void RequireNewPassword()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Şifre boş olamaz!")
                .MinimumLength(PASSWORD_LENGTH)
                .WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!")
                .Must(PasswordValidationControl)
                .WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir");
        }

        private void RequireConfirmNewPassword()
        {
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .WithMessage("Şifre boş olamaz!")
                .MinimumLength(PASSWORD_LENGTH)
                .WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!");

            RuleFor(x => x.ConfirmNewPassword).Must(PasswordValidationControl)
                .WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir")
                .When(x => x.ConfirmNewPassword != null);
        }

        private void ConfirmNewPassword()
        {
            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Geçersiz şifre!");
        }

        private void RequireOldPassword()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("Şifre boş olamaz!")
                .MinimumLength(PASSWORD_LENGTH)
                .WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!")
                .Must(PasswordValidationControl)
                .WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir");
        }

        private bool PasswordValidationControl(string password)
        {
            var digitCount = password.Count(char.IsDigit);
            var letterCount = password.Count(char.IsLetter);
            return digitCount >= DIGIT_COUNT && letterCount >= LETTER_COUNT;
        }
    }
}