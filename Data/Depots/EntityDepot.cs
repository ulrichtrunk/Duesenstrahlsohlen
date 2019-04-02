using Shared.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace Data.Depots
{
    /// <summary>
    /// BaseDepot which provides often used base functionality.
    /// </summary>
    /// <typeparam name="TEntity">the type of entity the depot handles by default. It is BaseEntity<int></typeparam>
    /// <seealso cref="Comitas.CAF.Core.Data.Depots.IDepot{TEntity, System.Int32}" />
    public abstract class EntityDepot<TEntity> : BaseDepot where TEntity : BaseEntity<int>
    {
        protected static Dictionary<string, string> EntityToColumnMap { get; set; }

        protected EntityDepot(IDatabase database) 
            : base(database)
        {
            CreateEntityToColumnMap();
        }


        /// <summary>
        /// Creates the entity to column map.
        /// </summary>
        private void CreateEntityToColumnMap()
        {
            if (EntityToColumnMap == null)
            {
                var pocoData = GetPocoDataForCurrentType();

                if(pocoData == null)
                {
                    return;
                }

                EntityToColumnMap = pocoData.
                    AllColumns.
                    ToDictionary(x => x.MemberInfoKey, x => x.ColumnName);
            }
        }


        /// <summary>
        /// Gets the poco data for current type.
        /// </summary>
        /// <returns></returns>
        protected PocoData GetPocoDataForCurrentType()
        {
            var pocoDataFactory = Database.PocoDataFactory;

            if(pocoDataFactory == null)
            {
                return null;
            }

            return Database.PocoDataFactory.ForType(typeof(TEntity));
        }


        protected string TableName => GetPocoDataForCurrentType().TableInfo.TableName;

        protected string PrimaryKey => GetPocoDataForCurrentType().TableInfo.PrimaryKey;
        
        protected virtual Sql SelectAllSql
        {
            get
            {
                return Sql.Builder.Select("*").From(TableName);
            }
        }


        /// <summary>
        /// Gets all columns as sql-ready, comma separated string
        /// </summary>
        /// <returns>string of all columns</returns>
        protected string GetAllColumns()
        {
            return string.Join(",", GetPocoDataForCurrentType().AllColumns.Select(x => x.ColumnName));
        }


        /// <summary>
        /// Saves the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Save(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                Save(entity);
            }
        }


        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public virtual void Save(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // save the entity in the database
            Database.Save(entity);
        }


        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(int id)
        {
            Database.Delete<TEntity>(id);
        }


        /// <summary>
        /// Deletes the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <exception cref="System.ArgumentNullException">ids</exception>
        public void Delete(IEnumerable<int> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            foreach (int id in ids)
            {
                Delete(id);
            }
        }


        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Database.Delete(entity);
        }


        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (TEntity entity in entities)
            {
                Delete(entity);
            }
        }


        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual TEntity GetById(int id)
        {
            return Database.Query<TEntity>().Where(x => x.Id == id).SingleOrDefault();
        }


        /// <summary>
        /// Gets the entities sorted and paged.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="defaultOrderBy">The default order by.</param>
        /// <returns></returns>
        protected PagedList<TEntity> GetSortedAndPaged(SortAndPagingParameters parameters, Sql sql, string defaultOrderBy)
        {
            string orderBy;
            string sortColumnName;

            if (!string.IsNullOrEmpty(parameters.Sort) && EntityToColumnMap.TryGetValue(parameters.Sort, out sortColumnName))
            {
                orderBy = sortColumnName + " " + parameters.SortDir;
            }
            else
            {
                orderBy = defaultOrderBy;
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sql.OrderBy(orderBy);
            }

            return GetPaged<TEntity>(parameters.Page, parameters.PageSize, sql);
        }


        /// <summary>
        /// Gets the entites paged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">pageSize must be >= 1</exception>
        protected PagedList<T> GetPaged<T>(int currentPage, int pageSize, Sql sql)
        {
            if (pageSize < 1)
                throw new ArgumentException("pageSize must be >= 1");

            if (currentPage < 1)
                currentPage = 1;

            Page<T> nPocoPage = Database.Page<T>(currentPage, pageSize, sql);
            PagedList<T> pagedList = new PagedList<T>(nPocoPage.Items, (int)nPocoPage.CurrentPage, (int)nPocoPage.ItemsPerPage, (int)nPocoPage.TotalItems);

            return pagedList;
        }


        /// <summary>
        /// Fast insert for many entities without checking for transient state.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        public virtual void InsertBulk<T>(IEnumerable<T> entities, int chunkSize)
        {
            var chunks = entities.Chunkify(chunkSize);

            var members = GetPocoDataForCurrentType()
                .AllColumns
                .Where(x => !x.ColumnName.Equals(PrimaryKey, StringComparison.OrdinalIgnoreCase));

            var properties = members
                .Select(x => $"@{x.MemberInfoKey}");

            string columns = string.Join(",", members.Select(x => x.ColumnName));
            string parameters = string.Join(",", properties);

            string sql = $"INSERT INTO {TableName}({columns}) VALUES({parameters});";

            foreach (var chunk in chunks)
            {
                Sql insert = new Sql();

                foreach (var entity in chunk)
                {
                    insert.Append(sql, entity);
                }

                Database.Execute(insert);
            }
        }
    }
}