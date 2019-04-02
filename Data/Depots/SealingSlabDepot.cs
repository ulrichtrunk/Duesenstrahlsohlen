using Shared.Entities;
using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace Data.Depots
{
    public class SealingSlabDepot : EntityDepot<SealingSlab>
    {
        public SealingSlabDepot(IDatabase database)
            : base(database)
        {

        }

        /// <summary>
        /// Gets the sealing slabs by calculation identifier and iteration.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        public List<SealingSlab> GetByCalculationIdAndIteration(int calculationId, int iteration)
        {
            return Database.Query<SealingSlab>()
                .Where(x => x.CalculationId == calculationId && x.Iteration == iteration)
                .ToList();
        }

        /// <summary>
        /// Gets maximum identifier of sealing slab by calculation identifier.
        /// </summary>
        /// <param name="calculationId"></param>
        public int? GetMaxId(int calculationId)
        {
            Sql sql = new Sql().Select("MAX(Id)").From(TableName).Where("CalculationId = @Id", new { Id = calculationId });

            return Database.Fetch<int?>(sql).SingleOrDefault();
        }

        /// <summary>
        /// Gets minimum identifier of sealing slab by calculation identifier.
        /// </summary>
        /// <param name="calculationId"></param>
        public int? GetMinId(int calculationId)
        {
            Sql sql = new Sql().Select("MIN(Id)").From(TableName).Where("CalculationId = @Id", new { Id = calculationId });

            return Database.Fetch<int?>(sql).SingleOrDefault();
        }

        /// <summary>
        /// Deletes the sealing slabs by calculation identifier.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        public void DeleteByCalculationId(int calculationId)
        {
            Sql sql = new Sql().Append("DELETE").From(TableName).Where("CalculationId = @Id", new { Id = calculationId });

            Database.Execute(sql);
        }

        /// <summary>
        /// Deletes the sealing slabs by calculation identifier.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        public void DeleteByCalculationId(int calculationId, int minId, int maxId)
        {
            Sql sql = new Sql().Append("DELETE").From(TableName)
                .Where("CalculationId = @Id", new { Id = calculationId })
                .Where("Id >= @MinId", new { MinId = minId })
                .Where("Id <= @MaxId", new { MaxId = maxId });

            Database.Execute(sql);
        }

        /// <summary>
        /// Gets the highest iteration.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <returns></returns>
        public virtual int GetMaxIteration(int calculationId)
        {
            Sql sql = new Sql().Select("MAX(Iteration)").From(TableName).Where("CalculationId = @Id", new { Id = calculationId });

            int? result = Database.Fetch<int?>(sql).SingleOrDefault();

            return result ?? 0;
        }
    }
}