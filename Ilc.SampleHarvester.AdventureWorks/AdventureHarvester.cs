using Ilc.SampleHarvester.AdventureWorks.DataCube;
using Ilc.InformationHarvester;
using Ilc.WcfService;

namespace Ilc.SampleHarvester.AdventureWorks
{
    [OneWayFaultServiceBehavior]
    public class AdventureHarvester : Ilc.InformationHarvester.InformationHarvester
    {
        public AdventureHarvester()
            : base(new AdventureDataCube())
        {
        }
    }
}