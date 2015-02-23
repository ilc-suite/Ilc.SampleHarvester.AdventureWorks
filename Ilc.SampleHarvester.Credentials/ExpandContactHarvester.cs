using Ilc.SampleHarvester.ExpandContact.DataCube;
using Ilc.InformationHarvester;
using Ilc.WcfService;

namespace Ilc.SampleHarvester.ExpandContact
{
    [OneWayFaultServiceBehavior]
    public class ExpandContactHarvester : Ilc.InformationHarvester.InformationHarvester
    {
        public ExpandContactHarvester()
            : base(new ExpandContactDataCube())
        {
        }
    }
}