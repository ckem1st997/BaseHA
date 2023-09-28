using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class IntentCommandValidator : AbstractValidator<IntentCommands>
    {
        public IntentCommandValidator()
        {
            
            RuleFor(order => order.IntentEn).Length(0, 255).WithMessage("Kịch bản có độ dài tối đa 255");
            RuleFor(order => order.IntentVn).NotEmpty().WithMessage("Bạn chưa nhập kịch bản tiếng Việt");
 

        }

    }
   
}
