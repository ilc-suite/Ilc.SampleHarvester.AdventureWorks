using Ilc.InformationHarvester;
using Ilc.SampleHarvester.ExpandContact;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Ilc.SampleHarvester.ExpandContact
{
    public class Startup : HarvesterStartupBase
    {
        public void Configuration(IAppBuilder app)
        {
            Initialize();
        }
    }
}