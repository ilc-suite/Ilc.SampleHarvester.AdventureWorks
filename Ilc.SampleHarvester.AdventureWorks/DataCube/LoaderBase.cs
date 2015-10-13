using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube
{
    /// <summary>
    /// A class for loading data from Database
    /// </summary>
    public class LoaderBase
    {
        private string ConnectionString { set; get;}

        /// <summary>
        /// Creates a LoaderBase class.
        /// </summary>
        /// <param name="connectionString">A connection string to a AdventureWorkds database.</param>
        protected LoaderBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Creates a SQL Connection to a AdventureWorks database.
        /// </summary>
        protected SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

    }
}