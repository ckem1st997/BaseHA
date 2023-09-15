using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class IntentCommandValidator : AbstractValidator<IntentCommands>
    {
        public IntentCommandValidator()
        {
            RuleFor(order => order.IntentCodeEn).NotNull().WithMessage("Bạn chưa nhập Code English");
            RuleFor(order => order.IntentEn).NotNull().WithMessage("Bạn chưa nhập Intent English");
            RuleFor(order => order.IntentVn).NotNull().WithMessage("Bạn chưa nhập Intnet VietNam");
        }
    }
}
