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
        private readonly TaxScheduleValidation _taxScheduleValidation;

        public TaxesController(ITaxService taxService, TaxScheduleValidation taxScheduleValidation)
        {
            _taxService = taxService;
            _taxScheduleValidation = taxScheduleValidation;

        }

        [HttpGet]
        public async Task<ActionResult<double>> GetTaxRate(string municipality, DateTime taxdate)
        {
            try
            {
                double? taxRate = await _taxService.GetTaxRate(municipality, taxdate);

                if (taxRate == null)
                    return NotFound(new { message = "Tax schedule not found for the given Municipality and Tax Date" });
                else
                    return Ok(taxRate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaxSchedule>> CreateTaxSchedule([FromBody] TaxSchedule taxSchedule)
        {
            try
            {

                var validationResult = _taxScheduleValidation.Validate(taxSchedule);

                if (!validationResult.IsValid)
                {
                    Console.WriteLine(validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                taxSchedule = await _taxService.AddTaxSchedule(taxSchedule);

                if (taxSchedule == null)
                {
                    Console.WriteLine("nirabotajat");
                    return StatusCode(500, "An error occured while processing the request.");
                }

                return Created("TaxSchedule",taxSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occured while processing the request.");
            }
        }

    }
}