using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class IntentCommandValidator : AbstractValidator<IntentCommands>
    {
        public IntentCommandValidator()
        {
            RuleFor(order => order.CategoryID).NotNull().WithMessage("Bạn chưa chọn danh mục !");
            //RuleFor(order => order.IntentEn).NotNull().WithMessage("Bạn chưa nhập Intent English");
            //RuleFor(order => order.IntentVn).NotNull().WithMessage("Bạn chưa nhập Intnet VietNam");
        }
    }
}
