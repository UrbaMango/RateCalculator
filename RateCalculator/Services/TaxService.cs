using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateCalculator.Models;

namespace RateCalculator.Services
{
    public class TaxService : ITaxService
    {
        private readonly TaxScheduleContext _dbContext;

        public TaxService(TaxScheduleContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<double?> GetTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                //Brings a list of available TaxRates for the given date
               TaxSchedule? taxSchedule = _dbContext.TaxSchedules
                    .Where(x => x.Municipality == municipality &&
                                x.TaxDateStart <= taxDate &&
                                x.TaxDateEnd >= taxDate)
                    .ToList()
                    //Filters by the least days between the tax schedule interval. The First will have our needed date
                    .OrderBy(x => (x.TaxDateEnd - x.TaxDateStart).Days)
                    .FirstOrDefault();

                Console.WriteLine(taxSchedule);

                if(taxSchedule != null)
                {
                    return taxSchedule.TaxRate;                    
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<TaxSchedule?> AddTaxSchedule(TaxSchedule taxSchedule)
        {
            try
            {
                //Add logic to check if already exists if so update it
                _dbContext.TaxSchedules.Add(taxSchedule);
                _dbContext.SaveChanges();
                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        } 

    }
}
