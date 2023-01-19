namespace RateCalculator
{
    public class TaxSchedule
    {
        //Table columns: Municipality, TaxTime(year,month,week,day), TaxDate, TaxRate
        private string Municipality { get; }

        public enum TaxTime { get; set; }

        public DateTime TaxDate { get; set; }

        public double TaxRate { get; set; }
    }
}