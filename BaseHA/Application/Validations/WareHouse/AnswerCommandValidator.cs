using BaseHA.Application.ModelDto;
using FluentValidation;

namespace BaseHA.Application.Validations.WareHouse
{
    public class AnswerCommandValidator : AbstractValidator<AnswerCommands>
    {
        public AnswerCommandValidator()
        {
            RuleFor(order => order.IntentId).NotNull().WithMessage("Bạn chưa nhập mã English");
            RuleFor(order => order.AnswerVn).NotNull().WithMessage("Bạn chưa nhập câu trả lời");
        }
    }
}
