using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class AnswerCommandValidator : AbstractValidator<AnswerCommands>
    {

        public AnswerCommandValidator() 
        {
            RuleFor(order => order.AnswerVn).NotEmpty().WithMessage("Bạn chưa nhập câu trả lời");
                
        }
    }

    
}
