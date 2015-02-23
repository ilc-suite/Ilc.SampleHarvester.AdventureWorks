using Ilc.InformationHarvester;
using Ilc.SampleHarvester.AdventureWorks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Ilc.SampleHarvester.AdventureWorks
{
    public class Startup : HarvesterStartupBase
    {
        public void Configuration(IAppBuilder app)
        {
            Initialize();
        }
    }
}