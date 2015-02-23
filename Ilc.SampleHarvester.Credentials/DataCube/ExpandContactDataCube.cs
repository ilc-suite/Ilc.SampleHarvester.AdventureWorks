using System.ComponentModel.Composition;
using System.Collections.Generic;
using Ilc.BusinessObjects;
using Ilc.DataCube.Contract;
using Ilc.DataCube.Data;
using Ilc.Diagnostics;
using Ilc.SampleHarvester.ExpandContact.Configuration;
using Ilc.DataCube;

namespace Ilc.SampleHarvester.ExpandContact.DataCube
{
    [Export(typeof(IDataCube))]
    public class ExpandContactDataCube : SingleCredentialsDataCube, IDataCube, IDataCubeWithCredentialsAuth
    {
        #region Private Fields

        private ILoggingService logger;
        
        #endregion

        public ExpandContactDataCube()
        {
            logger = IlcLoggingService.Instance;
        }

        public void ResolveInfoPoints(InfoPointProcess context, IInfoPointDataInterface dataInterface)
        {
            
        }

        public List<ObjectType> GetCollectTypes(string tenant)
        {
            return new List<ObjectType>();
        }

        public void CollectInformations(InformationProcess context, InfoPoint infoPoint, IInformationDataInterface dataInterface)
        {
            
        }

        // this function is called when this harvester is configured to succeed from another harvester.
        public void ExpandInformations(InformationProcess context, List<string> informationIds, IInformationDataInterface dataInterface)
        {
            var server = PictureServerConfiguration.Server.TryLoadByTenant(context.Session.Tenant);
            var identity = base.GetCredentials(context.Session);
            var expander = new PersonExpander(server, identity);

            for (int i = 0; i < informationIds.Count; i++)
            {
                expander.ExpandObject(context.Session, informationIds[i], dataInterface);
                if (i % 25 == 0)
                    dataInterface.Flush();
            }
        }
      

        public override string Category
        {
            get { return "PictureServer"; }
        }

        public override string AppName
        {
            get { return "CredentialsTest"; }
        }

        protected override bool IsConfigurated(SessionInfo session)
        {
            var server = PictureServerConfiguration.Server.TryLoadByTenant(session.Tenant);
            return (server != null);
        }

        protected override bool TestCredentials(SessionInfo session, CredentialsApiIdentity identity)
        {
            var server = PictureServerConfiguration.Server.TryLoadByTenant(session.Tenant);
            var client = new PictureClient(server.Url, identity.Username, identity.GetPassword());
            return client.IsLoggedIn();
        }
    }
}
