using BaseHA.Models.SearchModel;
using FluentValidation;

namespace BaseHA.Application.Validations.BaseValidator
{
    public class BaseSearchModelValidator : AbstractValidator<BaseSearchModel>
    {
        public BaseSearchModelValidator()
        {
            RuleFor(order => order.PageIndex).GreaterThanOrEqualTo(0).WithMessage("Giá trị truyền vào phải lớn hơn 0 !");
            RuleFor(order => order.PageIndex * order.PageSize).GreaterThan(0).LessThan(1000).WithMessage("Giá trị truyền vào phải nhỏ hơn 1000 !");
        }
    }
}