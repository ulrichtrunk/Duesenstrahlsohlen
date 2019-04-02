using Data;
using Data.Depots;

namespace Business.Services
{
    public class SealingSlabService
    {
        private SealingSlabDepot sealingSlabDepot;

        public SealingSlabService()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            sealingSlabDepot = new SealingSlabDepot(databaseFactory.GetDatabase());
        }


        /// <summary>
        /// Gets the highest iteration.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <returns></returns>
        public int GetMaxIteration(int calculationId)
        {
            return sealingSlabDepot.GetMaxIteration(calculationId);
        }
    }
}