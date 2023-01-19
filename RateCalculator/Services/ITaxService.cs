using Microsoft.AspNetCore.Mvc;
using RateCalculator.Models;

namespace RateCalculator.Services
{
    public interface ITaxService
    {
        TaxSchedule FindTaxRate(string municipality, DateTime taxDate);
        Task<ActionResult<TaxSchedule>> PostTaxSchedule(TaxSchedule taxSchedule);
        bool ValidTaxTime(string taxTime);
    }
}
