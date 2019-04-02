using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NPoco;
using NPoco.Linq;

namespace Batchworker.Test
{
    class NPocoDataBaseFactory : Data.NPocoDataBaseFactory
    {
        class DummyDatabase : IDatabase
        {
            public System.Data.Common.DbConnection Connection
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string ConnectionString
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IDictionary<string, object> Data
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public DatabaseType DatabaseType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public List<IInterceptor> Interceptors
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public MapperCollection Mappers
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public int OneTimeCommandTimeout
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public IPocoDataFactory PocoDataFactory
            {
                get
                {
                   return null; // throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public System.Data.Common.DbTransaction Transaction
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void AbortTransaction()
            {
                throw new NotImplementedException();
            }

            public void AddParameter(System.Data.Common.DbCommand cmd, object value)
            {
                throw new NotImplementedException();
            }

            public void BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public void BeginTransaction(System.Data.IsolationLevel isolationLevel)
            {
                throw new NotImplementedException();
            }

            public void BuildPageQueries<T>(long skip, long take, string sql, ref object[] args, out string sqlCount, out string sqlPage)
            {
                throw new NotImplementedException();
            }

            public void CloseSharedConnection()
            {
                throw new NotImplementedException();
            }

            public void CompleteTransaction()
            {
                throw new NotImplementedException();
            }

            public System.Data.Common.DbCommand CreateCommand(System.Data.Common.DbConnection connection, System.Data.CommandType commandType, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public System.Data.Common.DbParameter CreateParameter()
            {
                throw new NotImplementedException();
            }

            public int Delete(object poco)
            {
                throw new NotImplementedException();
            }

            public int Delete(string tableName, string primaryKeyName, object poco)
            {
                throw new NotImplementedException();
            }

            public int Delete(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
            {
                throw new NotImplementedException();
            }

            public int Delete<T>(object pocoOrPrimaryKey)
            {
                throw new NotImplementedException();
            }

            public int Delete<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public int Delete<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<int> DeleteAsync(object poco)
            {
                throw new NotImplementedException();
            }

            public IDeleteQueryProvider<T> DeleteMany<T>()
            {
                throw new NotImplementedException();
            }

            public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(Sql Sql)
            {
                throw new NotImplementedException();
            }

            public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public int Execute(Sql sql)
            {
                throw new NotImplementedException();
            }

            public int Execute(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public int Execute(string sql, System.Data.CommandType commandType, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<int> ExecuteAsync(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<int> ExecuteAsync(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T ExecuteScalar<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public T ExecuteScalar<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T ExecuteScalar<T>(string sql, System.Data.CommandType commandType, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<T> ExecuteScalarAsync<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<T> ExecuteScalarAsync<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public bool Exists<T>(object primaryKey)
            {
                throw new NotImplementedException();
            }

            public List<object> Fetch(Type type, Sql Sql)
            {
                throw new NotImplementedException();
            }

            public List<object> Fetch(Type type, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public List<T> Fetch<T>()
            {
                throw new NotImplementedException();
            }

            public List<T> Fetch<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public List<T> Fetch<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public List<T> Fetch<T>(long page, long itemsPerPage, Sql sql)
            {
                throw new NotImplementedException();
            }

            public List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> FetchAsync<T>()
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> FetchAsync<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> FetchAsync<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> FetchAsync<T>(long page, long itemsPerPage, Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> FetchAsync<T>(long page, long itemsPerPage, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, Sql sql)
            {
                throw new NotImplementedException();
            }

            public TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, Sql sql)
            {
                throw new NotImplementedException();
            }

            public TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, Sql sql)
            {
                throw new NotImplementedException();
            }

            public TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, Sql sql)
            {
                throw new NotImplementedException();
            }

            public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, Func<T, object> idFunc, Sql sql)
            {
                throw new NotImplementedException();
            }

            public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, Func<T, object> idFunc, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T First<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public T First<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T FirstInto<T>(T instance, Sql sql)
            {
                throw new NotImplementedException();
            }

            public T FirstInto<T>(T instance, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T FirstOrDefault<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public T FirstOrDefault<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T FirstOrDefaultInto<T>(T instance, Sql sql)
            {
                throw new NotImplementedException();
            }

            public T FirstOrDefaultInto<T>(T instance, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public ITransaction GetTransaction()
            {
                return new DummyTransaction();
            }

            public ITransaction GetTransaction(System.Data.IsolationLevel isolationLevel)
            {
                throw new NotImplementedException();
            }

            public object Insert<T>(T poco)
            {
                throw new NotImplementedException();
            }

            public object Insert<T>(string tableName, string primaryKeyName, T poco)
            {
                throw new NotImplementedException();
            }

            public object Insert<T>(string tableName, string primaryKeyName, bool autoIncrement, T poco)
            {
                throw new NotImplementedException();
            }

            public Task<object> InsertAsync<T>(T poco)
            {
                throw new NotImplementedException();
            }

            public void InsertBatch<T>(IEnumerable<T> pocos, BatchOptions options = null)
            {
                throw new NotImplementedException();
            }

            public void InsertBulk<T>(IEnumerable<T> pocos)
            {
                throw new NotImplementedException();
            }

            public bool IsNew<T>(T poco)
            {
                throw new NotImplementedException();
            }

            public IDatabase OpenSharedConnection()
            {
                throw new NotImplementedException();
            }

            public Page<T> Page<T>(long page, long itemsPerPage, Sql sql)
            {
                throw new NotImplementedException();
            }

            public Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<Page<T>> PageAsync<T>(long page, long itemsPerPage, Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<Page<T>> PageAsync<T>(long page, long itemsPerPage, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> Query(Type type, Sql Sql)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> Query(Type type, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public IQueryProviderWithIncludes<T> Query<T>()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> Query<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> Query<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<T>> QueryAsync<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public void Save<T>(T poco)
            {
                throw new NotImplementedException();
            }

            public void SetTransaction(System.Data.Common.DbTransaction tran)
            {
                throw new NotImplementedException();
            }

            public T Single<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public T Single<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T SingleById<T>(object primaryKey)
            {
                throw new NotImplementedException();
            }

            public Task<T> SingleByIdAsync<T>(object primaryKey)
            {
                throw new NotImplementedException();
            }

            public T SingleInto<T>(T instance, Sql sql)
            {
                throw new NotImplementedException();
            }

            public T SingleInto<T>(T instance, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T SingleOrDefault<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public T SingleOrDefault<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public T SingleOrDefaultById<T>(object primaryKey)
            {
                throw new NotImplementedException();
            }

            public Task<T> SingleOrDefaultByIdAsync<T>(object primaryKey)
            {
                throw new NotImplementedException();
            }

            public T SingleOrDefaultInto<T>(T instance, Sql sql)
            {
                throw new NotImplementedException();
            }

            public T SingleOrDefaultInto<T>(T instance, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public List<T> SkipTake<T>(long skip, long take, Sql sql)
            {
                throw new NotImplementedException();
            }

            public List<T> SkipTake<T>(long skip, long take, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> SkipTakeAsync<T>(long skip, long take, Sql sql)
            {
                throw new NotImplementedException();
            }

            public Task<List<T>> SkipTakeAsync<T>(long skip, long take, string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public int Update(object poco)
            {
                throw new NotImplementedException();
            }

            public int Update(object poco, object primaryKeyValue)
            {
                throw new NotImplementedException();
            }

            public int Update(object poco, IEnumerable<string> columns)
            {
                throw new NotImplementedException();
            }

            public int Update(object poco, object primaryKeyValue, IEnumerable<string> columns)
            {
                throw new NotImplementedException();
            }

            public int Update(string tableName, string primaryKeyName, object poco)
            {
                throw new NotImplementedException();
            }

            public int Update(string tableName, string primaryKeyName, object poco, IEnumerable<string> columns)
            {
                throw new NotImplementedException();
            }

            public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
            {
                throw new NotImplementedException();
            }

            public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue, IEnumerable<string> columns)
            {
                throw new NotImplementedException();
            }

            public int Update<T>(Sql sql)
            {
                throw new NotImplementedException();
            }

            public int Update<T>(string sql, params object[] args)
            {
                throw new NotImplementedException();
            }

            public int Update<T>(T poco, Expression<Func<T, object>> fields)
            {
                throw new NotImplementedException();
            }

            public Task<int> UpdateAsync(object poco)
            {
                throw new NotImplementedException();
            }

            public Task<int> UpdateAsync(object poco, IEnumerable<string> columns)
            {
                throw new NotImplementedException();
            }

            public Task<int> UpdateAsync<T>(T poco, Expression<Func<T, object>> fields)
            {
                throw new NotImplementedException();
            }

            public IUpdateQueryProvider<T> UpdateMany<T>()
            {
                throw new NotImplementedException();
            }
        }

        class DummyTransaction : ITransaction
        {
            public void Complete()
            {
            }

            public void Dispose()
            {
            }
        }

        public override IDatabase GetDatabase()
        {
            return new DummyDatabase();
        }
    }
}