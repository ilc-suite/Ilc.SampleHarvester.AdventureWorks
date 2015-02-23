using Ilc.InformationHarvester;
using Ilc.SampleHarvester.ExpandContact.DataCube;
using Ilc.WcfService;

namespace Ilc.SampleHarvester.ExpandContact
{
    [FaultServiceBehavior]
    public class ExpandContactService : HarvesterService
    {
        public ExpandContactService()
            : base(new ExpandContactDataCube())
        {
        }
    }
}