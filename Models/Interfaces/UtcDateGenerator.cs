using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace TeczkaCore.Models.Interfaces
{
    public class UtcDateTimeGeneratorek : ValueGenerator<DateTime>
    {
        public override bool GeneratesTemporaryValues => false;
        private DateTime UtcDate()
        {
            return DateTime.UtcNow;
        }
        public override DateTime Next(EntityEntry entry)
        {
            entry.State = EntityState.Modified;
            return UtcDate();
        }
        protected override object NextValue(EntityEntry entry)
        {
            return UtcDate();
        }
    }
}
