using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;
using FluentValidation;
using Nest;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;

namespace BaseHA.Application.Validations.WareHouse
{
    public class CategoryCommandValidator : AbstractValidator<CategoryCommands>
    {
        public CategoryCommandValidator()
        {
            RuleFor(order => order.NameCategory).NotNull().WithMessage("Bạn chưa nhập Name Category");
            RuleFor(order => order.IntentCodeEn).NotNull().WithMessage("Bạn chưa nhập Code English");
           // RuleFor(order => order.IntentCodeEn).NotNull().WithMessage("Bạn chưa nhập Code English");
            RuleFor(order => order.IntentCodeVn).NotNull().WithMessage("Bạn chưa nhập Code VietNam");

            
        }
    }
}
