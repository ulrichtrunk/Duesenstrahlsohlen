using Shared.Entities;
using NPoco;
using Shared;
using System;
using System.Linq;

namespace Data.Depots
{
    public class CalculationViewDepot : EntityDepot<CalculationView>
    {
        public CalculationViewDepot(IDatabase database)
            : base(database)
        {

        }

        /// <summary>
        /// Gets the view SQL statement.
        /// </summary>
        /// <returns></returns>
        private Sql GetViewSql()
        {
            return new Sql().Select(TableName + ".*",
                "Users.Name AS UserName",
                "States.Name AS StateName")
                .From(TableName)
                .InnerJoin("Users").On("Users.Id = Calculations.UserId")
                .InnerJoin("States").On("States.Id = Calculations.StateId");
        }


        /// <summary>
        /// Gets view for grid.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public PagedList<CalculationView> GetForGrid(SortAndPagingParameters parameters)
        {
            Sql sql = GetViewSql();

            return GetSortedAndPaged(parameters, sql, nameof(CalculationView.Id));
        }


        /// <summary>
        /// Gets the view by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override CalculationView GetById(int id)
        {
            Sql sql = GetViewSql().Where("Calculations.Id = @Id", new { Id = id });

            return Database.Fetch<CalculationView>(sql).SingleOrDefault();
        }


        /// <summary>
        /// Saves the calculation view.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Save(CalculationView entity)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes the calculation entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Delete(CalculationView entity)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes the calculation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}