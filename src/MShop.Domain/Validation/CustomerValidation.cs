using FluentValidation;
using MShop.Domain.Entities;

namespace MShop.Domain.Validation
{
    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O nome � obrigat�rio.")
                .MaximumLength(100).WithMessage("O nome n�o pode ter mais que 100 caracteres.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O e-mail � obrigat�rio.")
                .EmailAddress().WithMessage("O e-mail est� em formato inv�lido.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("O telefone � obrigat�rio.")
                .MaximumLength(20).WithMessage("O telefone n�o pode ter mais que 20 caracteres.")
                .Matches(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$")
                .WithMessage("O telefone deve estar em um formato v�lido. Ex: (81) 91234-5678");

            /*RuleFor(c => c.Address)
                .NotNull().WithMessage("O endere�o � obrigat�rio.")
                .SetValidator(new AddressValidation());*/

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Senha � obrigat�ria")
                .Must(IsPasswordStrong)
                .WithMessage("Senha n�o atende aos requisitos de seguran�a");
        }

        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (password.Length < 8) return false;

            int upperCount = password.Count(char.IsUpper);
            int lowerCount = password.Count(char.IsLower);
            int digitCount = password.Count(char.IsDigit);
            int specialCount = password.Count(ch => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(ch));

            int criteriaMet = 0;
            if (upperCount > 0) criteriaMet++;
            if (lowerCount > 0) criteriaMet++;
            if (digitCount > 0) criteriaMet++;
            if (specialCount > 0) criteriaMet++;

            return criteriaMet >= 2;
        }
    }
}