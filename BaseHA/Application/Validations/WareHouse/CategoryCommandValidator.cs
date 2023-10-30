using BaseHA.Application.ModelDto;
using BaseHA.Application.Validations.BaseValidator;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class CategoryCommandValidator : AbstractValidator<CategoryCommands>
    {
        public CategoryCommandValidator() 
        {

            RuleFor(order => order.IntentCodeVn).NotEmpty().WithMessage("Bạn chưa nhập mã kịch bản ")
            //.Must(value => value.Trim()=="")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage(" Và không nhận giá trị rỗng.");


        }
    }
}
