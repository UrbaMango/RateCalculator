using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RateCalculator.Models
{
    public class TaxSchedule
    {
        //Table columns: id, Municipality, TaxTime(year,month,week,day), TaxDate, TaxRate
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;private set; }
        public string Municipality { get; set; }
        public DateTime TaxDateStart { get; set; }
        public DateTime TaxDateEnd { get; set; }
        public double TaxRate { get; set; }
    }
}