using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class VendorCommandValidator : AbstractValidator<VendorCommands>
    {
        public VendorCommandValidator()
        {
            RuleFor(order => order.Code).NotEmpty().WithMessage("Bạn chưa nhập mã người bán");
            RuleFor(order => order.Name).NotEmpty().WithMessage("Bạn chưa nhập tên người bán");
           /* RuleFor(order => order.Address).NotNull().WithMessage("Bạn chưa nhập địa chỉ người bán");
            RuleFor(order => order.Phone).NotNull().WithMessage("Bạn chưa nhập sdt người bán");
            RuleFor(order => order.Email).NotNull().WithMessage("Bạn chưa nhập mail người bán");*/

        }
    }
}
