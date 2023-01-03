using Application.Models;
using DomainModels.Enums;
using FluentValidation;

namespace AppServices.Validations;

public class CreateVendaValidator : AbstractValidator<CreateVendaRequest>
{
    public CreateVendaValidator()
    {
        RuleFor(x => x.DataVenda)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Vendedor.Cpf)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsValidDocument());

        RuleFor(x => x.Vendedor.Nome)
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

        RuleFor(x => x.Vendedor.Telefone)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsCellphone())
            .WithMessage("O Cellphone precisa estar no formato '(XX) XXXXX-XXXX'");

        RuleFor(x => x.Status)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Itens)
            .NotEmpty()
            .NotNull();
    }
}