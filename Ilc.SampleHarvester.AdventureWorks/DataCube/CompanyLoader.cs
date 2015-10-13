using Ilc.BusinessObjects.Common;
using Ilc.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube 
{
    /// <summary>
    /// A class for loading company object from Database.
    /// </summary>
    public class CompanyLoader : LoaderBase
    {
        /// <summary>
        /// Creates a CompanyLoader class.
        /// </summary>
        /// <param name="connectionString">A connection string to a AdventureWorkds database.</param>
        public CompanyLoader(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Loads one or more companies by its name.
        /// </summary>
        /// <param name="name">The name of the company to be loaded.</param>
        /// <returns>A list of compnaies</returns>
        public List<Company> LoadCompanyByName(string name)
        {
            var result = new List<Company>();
            using (SqlConnection connection = CreateConnection())
            {
                var sql = "select * from [Purchasing].[vVendorWithAddresses] where [Name] like @name " +
                    "union select * from [Sales].[vStoreWithAddresses] where [Name] like @name ";
                var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@name", name + "%");
                using (SqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        result.Add(ReadCompany(datareader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Loads a company by its id
        /// </summary>
        /// <param name="businessEntityId">The unique identifier for the company.</param>
        /// <returns></returns>
        public Company LoadCompany(long businessEntityId)
        {
            Company result = null;
            using (SqlConnection connection = CreateConnection())
            {
                var sql = "select * from [Purchasing].[vVendorWithAddresses] where [BusinessEntityId] = @id " +
                    "union select * from [Sales].[vStoreWithAddresses] where [BusinessEntityId] = @id ";
                var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", businessEntityId);
                using (SqlDataReader datareader = cmd.ExecuteReader())
                {
                    if (!datareader.Read())
                    {
                        throw new System.Data.RowNotInTableException(string.Format("no entries found for BusinessEntityId {0}", businessEntityId));
                    }
                    result = ReadCompany(datareader);
                    if (datareader.Read())
                    {
                        throw new System.Data.DBConcurrencyException(string.Format("found more than one company for BusinessEntityId {0}", businessEntityId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Loads a company by the email address of one of its contacts.
        /// </summary>
        /// <param name="email">the email address of the contact person.</param>
        /// <returns>A List of companies</returns>
        public List<Company> LoadCompanyByContactEmail(string email)
        {
            var result = new List<Company>();
            using (SqlConnection connection = CreateConnection())
            {
                var sqlvendor = "select adr.* from [Purchasing].[vVendorWithAddresses] adr" +
                    " inner join [Purchasing].[vVendorWithContacts] contacts on adr.[BusinessEntityId] = contacts.[BusinessEntityId] " +
                    " where [EmailAddress] = @email ";

                var cmd = new SqlCommand(sqlvendor, connection);
                cmd.Parameters.AddWithValue("@email", email);
                using (SqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        result.Add(ReadCompany(datareader));
                    }
                }

                var sqlstore = "select * from [Sales].[vStoreWithAddresses] adr " +
                    " inner join [Sales].[vStoreWithContacts] contacts on adr.[BusinessEntityId] = contacts.[BusinessEntityId] " +
                    " where [EmailAddress] = @email ";

                var cmdstore = new SqlCommand(sqlstore, connection);
                cmdstore.Parameters.AddWithValue("@email", email);
                using (SqlDataReader datareader = cmdstore.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        result.Add(ReadCompany(datareader));
                    }
                }
            }

            return result;
        }

        private Company ReadCompany(SqlDataReader reader)
        {
            return new Company
            {
                Number = reader.GetInt32OrDefault("BusinessEntityId").ToString(),
                Name = reader.GetStringOrDefault("Name"),

                Addresses = new List<PostAddress>()
                {
                   new PostAddress
                   {
                       Address = reader.GetStringOrDefault("AddressLine1") + reader.GetStringOrDefault("AddressLine2"),
                       City = reader.GetStringOrDefault("City"),
                       Zip = reader.GetStringOrDefault("PostalCode"),
                       Country = reader.GetStringOrDefault("CountryRegionName")
                   }
                }                
            };
        }

        
    }
}