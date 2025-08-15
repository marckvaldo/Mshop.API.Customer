using FluentValidation;
using MShop.Domain.ValueObjects;

namespace MShop.Domain.Validation
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.Street)
                .NotEmpty().WithMessage("A rua é obrigatória.")
                .MaximumLength(100).WithMessage("A rua não pode ter mais que 100 caracteres.");

            RuleFor(a => a.Number)
                .NotEmpty().WithMessage("O número é obrigatório.")
                .MaximumLength(10).WithMessage("O número não pode ter mais que 10 caracteres.");

            RuleFor(a => a.Complement)
                .MaximumLength(50).WithMessage("O complemento não pode ter mais que 50 caracteres.");

            RuleFor(a => a.District)
                .NotEmpty().WithMessage("O bairro é obrigatório.")
                .MaximumLength(50).WithMessage("O bairro não pode ter mais que 50 caracteres.");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("A cidade é obrigatória.")
                .MaximumLength(50).WithMessage("A cidade não pode ter mais que 50 caracteres.");

            RuleFor(a => a.State)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .MaximumLength(50).WithMessage("O estado não pode ter mais que 50 caracteres.");

            RuleFor(a => a.PostalCode)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .MaximumLength(20).WithMessage("O CEP não pode ter mais que 20 caracteres.");

            RuleFor(a => a.Country)
                .NotEmpty().WithMessage("O país é obrigatório.")
                .MaximumLength(50).WithMessage("O país não pode ter mais que 50 caracteres.");
        }
    }
}