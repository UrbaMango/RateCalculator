using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RateCalculator.Models
{
    public class TaxSchedule
    {
        //Table columns: id, Municipality, TaxTime(year,month,week,day), TaxDate, TaxRate
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Municipality { get; set; }

        public string TaxTime { get; set; }

        public DateTime TaxDate { get; set; }

        public double TaxRate { get; set; }
    }
}