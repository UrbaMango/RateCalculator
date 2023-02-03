using FluentValidation;
using RateCalculator.Models;

namespace RateCalculator.Services
{
    public class TaxScheduleValidation : AbstractValidator<TaxSchedule>
    {
        public TaxScheduleValidation()
        {
            RuleFor(x => x.Municipality)
                .NotEmpty().WithMessage("Municipality cannot be empty")
                .Length(3, 100).WithMessage("Municipality must be between 3 and 300 characters");

            RuleFor(x => x.TaxRate)
                .NotEmpty().WithMessage("TaxRate cannot be empty")
                .GreaterThan(0).WithMessage("TaxRate must be grater than 0");

            RuleFor(x => x.TaxDateStart)
                .NotEmpty().WithMessage("TaxDateStart cannot be empty")
                .LessThanOrEqualTo(x => x.TaxDateEnd).WithMessage("TaxDateStart must be less than TaxDateEnd");
        }
    }
}
