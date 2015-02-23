using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using Ilc.BusinessObjects;
using Ilc.DataCube.Contract;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube
{
    [Export(typeof(IDataCube))]
    public class AdventureDataCube : IDataCube         
    {

        private string connectionString;

        public AdventureDataCube() : this("DefaultConnection")
        {}

        public AdventureDataCube(string connectionString)
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings[connectionString] != null)
            {
                var configString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
                this.connectionString = configString;
            }
            else
            {
                this.connectionString = connectionString;
            }
        }
        
        /// This function is called when a new Context is generated and InfoPoints are beeing collected by IlcCore Server
        public void ResolveInfoPoints(InfoPointProcess process, IInfoPointDataInterface dataInterface)
        {
            if (process.Context == null)
                return;

            var parser = new ContextParser(process.Context);
            var companyLoader = new CompanyLoader(connectionString);

            if (parser.IsCompanyName)
            {
                var companies = companyLoader.LoadCompanyByName(parser.CompanyName);
                dataInterface.Insert(companies);
            }
            else if (parser.IsDebitorNumber || parser.IsKreditorNumber)
            {
                var company = companyLoader.LoadCompany(parser.DebitorNumber);
                dataInterface.Insert(company);
            }
            else if (parser.IsEmailAddress)
            {
                var companies = companyLoader.LoadCompanyByContactEmail(parser.EmailAddress.ToString());
                dataInterface.Insert(companies);
            }
        }

        public List<ObjectType> GetCollectTypes(string tenant)
        {
            return new List<ObjectType>();
        }

        public void CollectInformations(InformationProcess context, InfoPoint infoPoint, IInformationDataInterface dataInterface)
        {
            
        }

        public void ExpandInformations(InformationProcess context, List<string> informationIds, IInformationDataInterface dataInterface)
        {
            
        }
    }
}
