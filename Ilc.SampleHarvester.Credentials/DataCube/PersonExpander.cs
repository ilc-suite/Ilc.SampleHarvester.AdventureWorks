using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ilc.BusinessObjects;
using Ilc.BusinessObjects.Common;
using Ilc.DataCube.Contract;
using Ilc.DataCube.Data;
using Ilc.SampleHarvester.ExpandContact.Configuration;

namespace Ilc.SampleHarvester.ExpandContact.DataCube
{
    public class PersonExpander
    {
        private PictureClient client;

        public PersonExpander(PictureServer server, CredentialsApiIdentity identity)
        {
            client = new PictureClient(server.Url, identity.Username, identity.GetPassword());
        }

        public void ExpandObject(SessionInfo session, string informationId, IInformationDataInterface dataInterface)
        {
            var item = dataInterface.GetExisting(informationId);

            if (ExpandObject(session, item))
                dataInterface.Update(informationId, item);
            else
                dataInterface.NoUpdate(informationId);
        }

        private bool ExpandObject(SessionInfo session, InformationObject item)
        {
            if (item is Person)
            {
                return ExpandPerson(session, (Person)item);
            }
            return false;
        }

        private bool ExpandPerson(SessionInfo session, Person person)
        {
            var url = client.GetPictureUrl();
            person.PictureUrl = url;
            return !string.IsNullOrEmpty(url);
        }
    }
}