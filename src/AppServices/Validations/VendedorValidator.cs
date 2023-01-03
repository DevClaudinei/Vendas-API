using DomainModels.Entities;
using FluentValidation;

namespace AppServices.Validations;

public class VendedorValidator : AbstractValidator<Vendedor>
{
	public VendedorValidator()
	{
		RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsValidDocument());

        RuleFor(x => x.Nome)
            .NotEmpty()
            .NotNull()
            .Must(x => x.Split(" ").Length > 1)
            .WithMessage("FullName deve conter ao menos um sobrenome")
            .Must(x => !x.ContainsEmptySpace())
            .WithMessage("FullName não deve conter espaços em branco")
            .Must(x => !x.AnySymbolOrSpecialCharacter())
            .WithMessage("FullName não deve conter caracteres especiais")
            .Must(x => x.HasAtLeastTwoCharactersForEachWord())
            .WithMessage("FullName inválido. Nome e/ou sobrenome devem conter ao menos duas letras ou mais");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsCellphone())
            .WithMessage("O Cellphone precisa estar no formato '(XX) XXXXX-XXXX'");
    }
}