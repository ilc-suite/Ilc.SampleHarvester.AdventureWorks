using Ilc.BusinessObjects.Common;
using Ilc.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube
{
    public class ContactsLoader : LoaderBase
    {
        /// <summary>
        /// Creates a ContactsLoader class.
        /// </summary>
        /// <param name="connectionString">A connection string to a AdventureWorkds database.</param>
        public ContactsLoader(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Loads one or more companies by its name.
        /// </summary>
        /// <param name="name">The name of the company to be loaded.</param>
        /// <returns>A list of compnaies</returns>
        public List<ContactPerson> LoadContactsByCompany(Company company)
        {
            var result = new List<ContactPerson>();
            using (SqlConnection connection = CreateConnection())
            {
                var sql = "select * from [Purchasing].[vVendorWithContacts] where [BusinessEntityID] = @id " +
                    "union select * from [Sales].[vStoreWithContacts] where [BusinessEntityID] = @id ";
                var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", company.Number);
                using (SqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        result.Add(ReadContact(datareader));
                    }
                }
            }
            return result;
        }

        private ContactPerson ReadContact(SqlDataReader reader)
        {
            return new ContactPerson
            {
                Number = reader.GetString("EmailAddress"),
                Firstname = reader.GetStringOrDefault("FirstName"),
                Lastname = reader.GetStringOrDefault("LastName"),
                
                Address = new PostAddress
                {
                    Phone = reader.GetStringOrDefault("PhoneNumber"),
                    
                },
                EMails = new List<string>() { reader.GetStringOrDefault("EmailAddress") },
                JobTitle = reader.GetStringOrDefault("ContactType")
            };
        }
    }
}