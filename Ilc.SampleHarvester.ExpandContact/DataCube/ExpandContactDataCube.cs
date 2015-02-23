using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using Ilc.BusinessObjects;
using Ilc.DataCube.Contract;
using Ilc.BusinessObjects.Common;

namespace Ilc.SampleHarvester.ExpandContact.DataCube
{
    [Export(typeof(IDataCube))]
    public class ExpandContactDataCube : IDataCube 
    {
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
            for (int i = 0; i < informationIds.Count; i++)
            {
                ExpandObject(context.Session, informationIds[i], dataInterface);
                if (i % 25 == 0)
                    dataInterface.Flush();
            }
        }

        private void ExpandObject(SessionInfo session, string informationId, IInformationDataInterface dataInterface)
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
            var pictures = new PicturesManager();
            var url = pictures.Get(person);
            person.PictureUrl = url;
            return !string.IsNullOrEmpty(url);
        }
    }
}
