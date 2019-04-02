namespace Data
{
    using NPoco;
    using Shared;
    using System.IO;

    public class NPocoDataBaseFactory
    {
        /// <summary>
        /// Creates the date base by configuration.
        /// </summary>
        /// <returns></returns>
        public static Database CreateDateBaseByConfig()
        {
            return new Database(nameof(App.Config.ConnectionString));
        }


        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns></returns>
        public virtual IDatabase GetDatabase()
        {
            return CreateDateBaseByConfig();
        }
    }
}