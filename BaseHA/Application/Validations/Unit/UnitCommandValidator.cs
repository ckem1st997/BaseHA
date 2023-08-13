using BaseHA.Application.ModelDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseHA.Application.Validations.Unit
{
    public partial class UnitCommandValidator : AbstractValidator<UnitCommands>
    {
        public UnitCommandValidator()
        {
            RuleFor(order => order.UnitName).NotEmpty().WithMessage("Bạn chưa tên đơn vị tính !");
            RuleFor(order => order.UnitName).NotNull().WithMessage("Bạn chưa tên đơn vị tính !");

        }
    }
}