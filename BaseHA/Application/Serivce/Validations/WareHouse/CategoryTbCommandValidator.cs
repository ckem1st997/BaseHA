using BaseHA.Application.ModelDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseHA.Application.Validations.WareHouse
{
    public class CategoryTbCommandValidator : AbstractValidator<CategoryTbCommands>
    {
        public CategoryTbCommandValidator()
        {
            /*RuleFor(order => order.Category).NotNull().WithMessage("Bạn chưa nhập tên danh mục");
            RuleFor(order => order.IntentCodeEn).NotNull().WithMessage("Bạn chưa nhập code EN");*/
        }
    }
}