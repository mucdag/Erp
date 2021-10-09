using FluentValidation;
using Erp.Resource.Enums;
using Erp.Resource.Models.Users;

namespace Erp.Resource.Validations.Users
{
    public class PasswordResetResourceValidation : AbstractValidator<PasswordResetResource>
    {
        public const int DIGIT_COUNT = 2;
        public const int LETTER_COUNT = 2;
        public const int PASSWORD_LENGTH = 6;

        public PasswordResetResourceValidation()
        {
            RuleSet(ValidationRuleType.Reset, () => Reset());
            RuleSet(ValidationRuleType.ResetRequest, () => ResetRequest());
        }

        public void Reset()
        {
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email boş olamaz!");
            RuleFor(x => x.ResetCode).NotEmpty().WithMessage("Sıfırlama kodu boş olamaz!");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Şifre boş olamaz!")
                .MinimumLength(PASSWORD_LENGTH)
                .WithMessage($"Şifre {PASSWORD_LENGTH} karakterden daha az olamaz!")
                .Must(PasswordValidationControl)
                .WithMessage($"Şifre en az {DIGIT_COUNT} rakam ve {LETTER_COUNT} harf içermelidir");
        }

        public void ResetRequest()
        {
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email boş olamaz!");
        }

        private bool PasswordValidationControl(string password)
        {
            var digitCount = password.Count(char.IsDigit);
            var letterCount = password.Count(char.IsLetter);
            return digitCount >= DIGIT_COUNT && letterCount >= LETTER_COUNT;
        }
    }
}