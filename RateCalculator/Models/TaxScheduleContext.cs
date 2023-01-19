using Microsoft.EntityFrameworkCore;

namespace RateCalculator.Models
{
    public class TaxScheduleContext : DbContext
    {
        public TaxScheduleContext(DbContextOptions<TaxScheduleContext> options) : base(options) { }

        public DbSet<TaxSchedule> TaxSchedules { get; set; } = null;

    }
}
