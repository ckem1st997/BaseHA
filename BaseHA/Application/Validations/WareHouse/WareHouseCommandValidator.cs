using BaseHA.Application.ModelDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseHA.Application.Validations.WareHouse
{
    public class WareHouseCommandValidator : AbstractValidator<WareHouseCommands>
    {
        public WareHouseCommandValidator()
        {
            RuleFor(order => order.Code).NotEmpty().WithMessage("Bạn chưa nhập mã sản phẩm");
            RuleFor(order => order.Code).NotNull().WithMessage("Bạn chưa nhập mã sản phẩm");
            RuleFor(order => order.Name).NotEmpty().WithMessage("Bạn chưa nhập tên sản phẩm");
            RuleFor(order => order.Name).NotNull().WithMessage("Bạn chưa nhập tên sản phẩm");
        }
    }
}