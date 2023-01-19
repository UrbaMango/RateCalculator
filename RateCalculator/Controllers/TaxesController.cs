using Microsoft.AspNetCore.Mvc;
using RateCalculator.Models;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RateCalculator.Services;

namespace RateCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxesController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxesController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpGet]
        [Route("GetTaxRate")]
        public async Task<ActionResult<TaxSchedule>> GetTaxRate(string municipality, DateTime taxDate)
        {
            try
            {
                //finds needed taxSchedule according to logic
                TaxSchedule taxSchedule = _taxService.FindTaxRate(municipality, taxDate);

                if (taxSchedule == null)
                {
                    return NotFound(new { message = "Tax schedule not found for the given Municipality and Tax Date" });
                }

                return Ok(taxSchedule.TaxRate);
   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult<TaxSchedule>> PostTaxSchedule(TaxSchedule taxSchedule)
        {
            try
            {
                if (!_taxService.ValidTaxTime(taxSchedule.TaxTime))
                    return BadRequest(new { meesage = "Invalid TaxTime. Valid TaxTimes: Year, Month, Week, Day" });

                //check if already exists if so update existing rate for that date period
                if (await _taxService.PostTaxSchedule(taxSchedule) == null)
                    return StatusCode(500);
                return Ok(taxSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }

        /*
         * Initial Testing to remember .NET and id my logic works and understand how everything should work.
         * 
         *  //Table columns: Municipality, TaxTime(year,month,week,day), TaxDate, TaxRate
        private static TaxSchedule[] Schedules = new[]
        {
            new TaxSchedule{ Municipality = "Copenhagen", TaxTime = "Year", TaxDate = new DateTime(2023, 01, 01), TaxRate = 0.2},
            new TaxSchedule{ Municipality = "Copenhagen", TaxTime = "Month", TaxDate= new DateTime(2023, 05, 01), TaxRate = 0.4},
            new TaxSchedule{ Municipality = "Copenhagen", TaxTime = "Day", TaxDate = new DateTime(2023, 01, 01), TaxRate = 0.1},
            new TaxSchedule{ Municipality = "Copenhagen", TaxTime = "Day", TaxDate = new DateTime(2023, 12, 25), TaxRate = 0.1},
            new TaxSchedule{ Municipality = "Kaunas", TaxTime = "Year", TaxDate = new DateTime(2023, 01, 01), TaxRate = 420}
        };

        [HttpPost]
        [Route("/AddTaxSchedule")]
        public IActionResult PostTaxSchedule(TaxSchedule taxSchedule)
        {
            try
            {
                Schedules = Schedules.Concat(new TaxSchedule[] { taxSchedule }).ToArray();
                return Ok(JsonSerializer.Serialize(taxSchedule));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        [Route("/GetTaxRate")]
        public IActionResult GetTaxRate(string municipality, DateTime taxDate)
        {
            Console.WriteLine("Request input: " + municipality + " " + taxDate);

            try
            {
                //Priority is to get Day Tax and then others.
                //Day > Week > Month > Year


                //First Try Find By exact date
                TaxSchedule? taxSchedule = Schedules.Where(x => x.Municipality == municipality && x.TaxTime == "Day" && x.TaxDate == taxDate)
                    .SingleOrDefault();
                
                if (taxSchedule == null)
                    //Need to match year and the number of this years week number
                    taxSchedule = Schedules.Where(x => x.Municipality == municipality && x.TaxTime == "Week" && x.TaxDate == taxDate)
                        .SingleOrDefault();

                if (taxSchedule == null)
                    //Matches year and month
                    taxSchedule = Schedules.Where(x => x.Municipality == municipality && x.TaxTime == "Month" && x.TaxDate.Year == taxDate.Year && x.TaxDate.Month == taxDate.Month)
                        .SingleOrDefault();

                if (taxSchedule == null)
                    //Matches only year
                    taxSchedule = Schedules.Where(x => x.Municipality == municipality && x.TaxTime == "Year" && x.TaxDate.Year == taxDate.Year).
                        SingleOrDefault();

                //Console.WriteLine(JsonSerializer.Serialize(Schedules));

                if (taxSchedule != null)
                {
                    Console.WriteLine("Found Schedule" + JsonSerializer.Serialize(taxSchedule));
                    Console.WriteLine("Request output: " + taxSchedule.TaxRate);
                    return Ok(taxSchedule.TaxRate);
                }
                else
                {
                    return NotFound(new { message = "Tax schedule not found for the given Municipality and Tax Date" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }                


        }
        */
    }
}