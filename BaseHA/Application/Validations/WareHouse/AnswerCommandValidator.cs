using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class AnswerCommandValidator : AbstractValidator<AnswerCommands>
    {

        public AnswerCommandValidator() 
        {
            RuleFor(order => order.AnswerVn).NotEmpty().WithMessage("Bạn chưa nhập câu trả lời")
                .Length(1, 1000).WithMessage("Câu trả lời có độ dài ký tự từ 1-1000");
        }
    }

    
}
