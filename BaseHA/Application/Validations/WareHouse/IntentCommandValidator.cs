using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class IntentCommandValidator : AbstractValidator<IntentCommands>
    {
        public IntentCommandValidator()
        {
            
           
            RuleFor(order => order.IntentVn).NotEmpty().WithMessage("Bạn chưa nhập Ý định")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage(" Và không nhận giá trị rỗng."); 
 

        }

    }
   
}
