using Microsoft.AspNetCore.Mvc;
using RateCalculator.Models;

namespace RateCalculator.Services
{
    public interface ITaxService
    {
        Task<double?> GetTaxRate(string municipality, DateTime taxDate);
        Task<TaxSchedule?> AddTaxSchedule(TaxSchedule taxSchedule);
    }
}
