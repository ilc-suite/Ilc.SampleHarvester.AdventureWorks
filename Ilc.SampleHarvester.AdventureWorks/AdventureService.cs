using Ilc.InformationHarvester;
using Ilc.SampleHarvester.AdventureWorks.DataCube;
using Ilc.WcfService;

namespace Ilc.SampleHarvester.AdventureWorks
{
    [FaultServiceBehavior]
    public class AdventureService : HarvesterService
    {
        public AdventureService()
            : base(new AdventureDataCube())
        {
        }
    }
}