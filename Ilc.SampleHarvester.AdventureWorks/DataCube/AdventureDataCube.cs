using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using Ilc.BusinessObjects;
using Ilc.DataCube.Contract;
using Ilc.BusinessObjects.Common;
using AdventureWorks.InformaitonObjects;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube
{
    [Export(typeof(IDataCube))]
    public class AdventureDataCube : IDataCube, IDetailsDataCube
    {
        public static string ProductPhotoDetailslink = "ProductPhoto";
        private string connectionString;

        public AdventureDataCube() : this ("DefaultConnection")
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

        // This function is called when the IlcCore Server requests the information types that can be loaded from this harvester
        public List<ObjectType> GetCollectTypes(string tenant)
        {
            return new List<ObjectType> 
            {
                new ObjectType(typeof(Company), "Company"),                
                new ObjectType(typeof(ContactPerson), "ContactPerson"),
                new ObjectType(typeof(BikeProduct), "BikeProduct"),
            };
        }


        // This function is called when the IlcCore Server request informations for an InfoPoint
        public void CollectInformations(InformationProcess context, InfoPoint infoPoint, IInformationDataInterface dataInterface)
        {
            var company = infoPoint.Value as Company;

            if (company == null)
                return;

            dataInterface.Insert(company);

            // load contact informations
            var contactsLoader = new ContactsLoader(connectionString);
            var contacts = contactsLoader.LoadContactsByCompany(company);
            dataInterface.Insert(contacts);

            var productsLoader = new ProductsLoader(connectionString);            
            var products = productsLoader.LoadProductByCompany(company);

            foreach (var product in products)
            {
                // Creating and adding a DetailsLink for the product information
                var detailsLink = new List<DetailsLink>();
                detailsLink.Add(dataInterface.CreateDetailsLink(AdventureDataCube.ProductPhotoDetailslink, product.Id));

                dataInterface.Insert(product, null, detailsLink);                        
            }
        }

        public void ExpandInformations(InformationProcess context, List<string> informationIds, IInformationDataInterface dataInterface)
        {
            
        }        

        public void CollectDetails(DetailsProcess process, IDetailsDataInterface dataInterface)
        {
            if (dataInterface.IsIdDetailsLink(AdventureDataCube.ProductPhotoDetailslink))
            {
                var productsLoader = new ProductsLoader(connectionString);
                var productid = dataInterface.GetLinkId();
                var items = productsLoader.GetProductPhoto(productid);
                var item = items.FirstOrDefault();
                dataInterface.Set(item);
            }
        }
    }
}
