using FluentValidation;
using MShop.Domain.ValueObjects;

namespace MShop.Domain.Validation
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.Street)
                .NotEmpty().WithMessage("A rua � obrigat�ria.")
                .MaximumLength(100).WithMessage("A rua n�o pode ter mais que 100 caracteres.");

            RuleFor(a => a.Number)
                .NotEmpty().WithMessage("O n�mero � obrigat�rio.")
                .MaximumLength(10).WithMessage("O n�mero n�o pode ter mais que 10 caracteres.");

            RuleFor(a => a.Complement)
                .MaximumLength(50).WithMessage("O complemento n�o pode ter mais que 50 caracteres.");

            RuleFor(a => a.District)
                .NotEmpty().WithMessage("O bairro � obrigat�rio.")
                .MaximumLength(50).WithMessage("O bairro n�o pode ter mais que 50 caracteres.");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("A cidade � obrigat�ria.")
                .MaximumLength(50).WithMessage("A cidade n�o pode ter mais que 50 caracteres.");

            RuleFor(a => a.State)
                .NotEmpty().WithMessage("O estado � obrigat�rio.")
                .MaximumLength(50).WithMessage("O estado n�o pode ter mais que 50 caracteres.");

            RuleFor(a => a.PostalCode)
                .NotEmpty().WithMessage("O CEP � obrigat�rio.")
                .MaximumLength(20).WithMessage("O CEP n�o pode ter mais que 20 caracteres.");

            RuleFor(a => a.Country)
                .NotEmpty().WithMessage("O pa�s � obrigat�rio.")
                .MaximumLength(50).WithMessage("O pa�s n�o pode ter mais que 50 caracteres.");
        }
    }
}