using BaseHA.Application.ModelDto;
using BaseHA.Application.Validations.BaseValidator;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class CategoryCommandValidator : AbstractValidator<CategoryCommands>
    {
        public CategoryCommandValidator() 
        {
            RuleFor(order => order.NameCategory).NotNull().WithMessage("Bạn chưa nhập tên danh mục");
            RuleFor(order => order.IntentCodeEn).NotNull().WithMessage("Bạn chưa nhập mã tiếng Anh")
                .Length(1,255).WithMessage("Mã có độ dài ký tự 1-255");
            RuleFor(order => order.IntentCodeVn).NotEmpty().WithMessage("Bạn chưa nhập mã tiếng Việt")
                .Length(1, 255).WithMessage("Mã có độ dài ký tự 1-255"); ;
            RuleFor(order => order.Description).NotNull().WithMessage("Bạn chưa nhập tên sản phẩm")
                .Length(1, 255).WithMessage("Mã có độ dài ký tự 1-1000"); ;

        }
    }
}
