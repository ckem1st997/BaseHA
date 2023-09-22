using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class IntentCommandValidator : AbstractValidator<IntentCommands>
    {
        public IntentCommandValidator()
        {
            RuleFor(order => order).Must(order => order.IntentEn != null || order.IntentVn !=null)
                .WithMessage("Bạn phải nhập ít nhất Ý định tiếng Anh hoặc Ý định tiếng Việt");
            RuleFor(order => order.IntentEn).Length(1, 255).WithMessage("Ý định có độ dài ký tự từ 1-255");
            RuleFor(order => order.IntentVn).Length(1, 255).WithMessage("Ý định có độ dài ký tự từ 1-255"); 

        }
    }
}
