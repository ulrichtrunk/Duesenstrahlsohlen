using NPoco;

namespace Shared.Entities
{
    [TableName("Estimations")]
    public class Estimation : BaseEntity<int>
    {
        public double CPUFactor { get; set; }
        public double DBInsertFactor { get; set; }
    }
}
