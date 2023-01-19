using Microsoft.AspNetCore.Mvc;
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
        public bool ValidTaxTime(string taxTime)
        {
            List<string> validTaxTimes = new List<String> { "Day", "Week", "Month", "Year" };

            if (validTaxTimes.Contains(taxTime))
            {
                return true;
            }
            return false;
        }
        public TaxSchedule FindTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                //Prioritization rules Day>Week>Month>Year
                //First searching for matching exact dates. TaxTime Day
                TaxSchedule? taxSchedule = DayTaxRate(municipality, taxDate);

                if (taxSchedule == null)
                    taxSchedule = WeekTaxRate(municipality, taxDate);

                if (taxSchedule == null)
                    taxSchedule = MonthTaxRate(municipality, taxDate);

                if (taxSchedule == null)
                    taxSchedule = YearTaxRate(municipality, taxDate);

                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<ActionResult<TaxSchedule>> PostTaxSchedule(TaxSchedule taxSchedule)
        {
            try
            {
                //Need to init this garbage
                TaxSchedule? tax;
                //check if already exists if so update existing rate for that date period
                switch (taxSchedule.TaxTime)
                {
                    case "Day":
                        tax = DayTaxRate(taxSchedule.Municipality, taxSchedule.TaxDate);
                        break;
                    case "Week":
                        tax = WeekTaxRate(taxSchedule.Municipality, taxSchedule.TaxDate);
                        break;
                    case "Month":
                        tax = MonthTaxRate(taxSchedule.Municipality, taxSchedule.TaxDate);
                        break;
                    case "Year":
                        tax = YearTaxRate(taxSchedule.Municipality, taxSchedule.TaxDate);
                        break;

                    default:
                        tax = null;
                        break;
                }

                if (tax == null)
                {
                    //Update cause does NOT exists
                    return await AddTaxSchedule(taxSchedule);
                }

                //Update cause already exists
                //Change only the TaxRate
                tax.TaxRate = taxSchedule.TaxRate;
                return await UpdateTaxSchedule(tax);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
                throw;
            }
        }

        //Add Tax Schedule if update is not needed
        private async Task<ActionResult<TaxSchedule>> AddTaxSchedule(TaxSchedule taxSchedule)
        {
            try
            {
                _dbContext.TaxSchedules.Add(taxSchedule);
                await _dbContext.SaveChangesAsync();
                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        private async Task<ActionResult<TaxSchedule>> UpdateTaxSchedule(TaxSchedule taxSchedule)
        {
            try
            {
                _dbContext.Update(taxSchedule);
                await _dbContext.SaveChangesAsync();
                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        //Finds Tax Where TaxTime = Day
        private TaxSchedule DayTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                TaxSchedule? taxSchedule = _dbContext.TaxSchedules.Where(x =>
                x.Municipality == municipality
                && x.TaxTime == "Day"
                && x.TaxDate.Date == taxDate.Date)
                        .SingleOrDefault();
                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        //Finds Tax Where TaxTime = Week
        //Logic is that Schedule date is always first day of that weeek, so +7days will work
        private TaxSchedule WeekTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                var query = from x in _dbContext.TaxSchedules
                            where x.Municipality == municipality
                            && x.TaxTime == "Week"
                            && x.TaxDate.Year == taxDate.Year
                            && x.TaxDate.Date <= taxDate.Date
                            && x.TaxDate.Date.AddDays(7) >= taxDate.Date
                            select x;
                TaxSchedule? taxSchedule = query.SingleOrDefault();

                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        //Finds Tax Where TaxTime = Month
        private TaxSchedule MonthTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                Console.WriteLine("Working Month");
                TaxSchedule? taxSchedule = _dbContext.TaxSchedules.Where(x => x.Municipality == municipality
                    && x.TaxTime == "Month"
                    && x.TaxDate.Year == taxDate.Year
                    && x.TaxDate.Month == taxDate.Month)
                        .SingleOrDefault();
                return taxSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        //Finds Tax Where TaxTime = Year
        private TaxSchedule YearTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                TaxSchedule? taxSchedule = _dbContext.TaxSchedules.Where(x =>
                    x.Municipality == municipality
                    && x.TaxTime == "Year"
                    && x.TaxDate.Year == taxDate.Year).
                        SingleOrDefault();
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
