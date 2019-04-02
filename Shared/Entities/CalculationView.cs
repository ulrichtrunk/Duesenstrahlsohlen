using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [TableName("Calculations")]
    public class CalculationView : Calculation
    {
        public string UserName { get; set; }
        public string StateName { get; set; }
        public int Action { get { return Id; } }

        public string EstimatedTimeLeft
        {
            get
            {
                if(!EstimatedEndDate.HasValue)
                {
                    return "n/a";
                }

                if(EndDate.HasValue)
                {
                    return TimeSpan.Zero.ToShortString();
                }

                var estimatedTime = EstimatedEndDate.Value - DateTime.Now;

                if (estimatedTime < TimeSpan.Zero)
                {
                    return TimeSpan.Zero.ToShortString();
                }

                return (EstimatedEndDate.Value - DateTime.Now).ToShortString();
            }
        }

        public string EstimatedTime
        {
            get
            {
                if (!EstimatedEndDate.HasValue || !StartDate.HasValue)
                {
                    return "n/a";
                }

                return (EstimatedEndDate.Value - StartDate.Value).ToShortString();
            }
        }
    }
}
