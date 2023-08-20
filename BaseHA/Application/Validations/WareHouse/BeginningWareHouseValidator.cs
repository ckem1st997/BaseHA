using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class BeginningWareHouseValidator : AbstractValidator<BeginningCommands>
    {
        public BeginningWareHouseValidator()
        {
            RuleFor(order => order.WareHouseId).NotNull().NotEmpty().WithMessage("Bạn chưa nhập mã kho");
            RuleFor(order => order.ItemId).NotNull().NotEmpty().WithMessage("Bạn chưa nhập mã kho sản phẩm");
            RuleFor(order => order.UnitId).NotNull().NotEmpty().WithMessage("Bạn chưa nhập mã đơn vị");

            //RuleFor(order => order.UnitName).NotNull().WithMessage("Bạn chưa nhập tên đơn vị ");
            RuleFor(order => order.Quantity).NotNull().WithMessage("Bạn chưa nhập số lượng");
            RuleFor(order => order.CreatedDate).NotNull().WithMessage("Bạn chưa nhập ngày tạo");
            //RuleFor(order => order.CreatedBy).NotNull().WithMessage("Bạn chưa nhập tên sản phẩm");
            RuleFor(order => order.ModifiedDate).NotNull().WithMessage("Bạn chưa nhập ngày tạo thông tin");
            // RuleFor(order => order.ModifiedBy).NotNull().WithMessage("Bạn chưa nhập tên sản phẩm");
        }
    }
}
