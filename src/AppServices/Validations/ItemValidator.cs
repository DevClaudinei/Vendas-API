using DomainModels.Entities;
using FluentValidation;

namespace AppServices.Validations;

public class ItemValidator : AbstractValidator<Item>
{
	public ItemValidator()
	{
		RuleFor(x => x.Name)
			.NotNull()
			.NotEmpty()
			.MaximumLength(50);
	}
}