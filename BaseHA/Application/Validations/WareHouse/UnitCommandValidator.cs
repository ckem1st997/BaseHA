using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public partial class UnitCommandValidator : AbstractValidator<UnitCommands>
    {
        public UnitCommandValidator()
        {
           
            RuleFor(order => order.UnitName).NotEmpty().WithMessage("Bạn chưa nhập tên người bán");
           

        }
    }
}
