using Ilc.DataCube.Data.System;

namespace Ilc.SampleHarvester.ExpandContact.Configuration
{
    public class PictureServerConfiguration
    {
        private static volatile ISingleServerStore<PictureServer> serverInstance;
        private static object syncLock = new object();

        public static ISingleServerStore<PictureServer> Server
        {
            get
            {
                if (serverInstance == null)
                {
                    lock (syncLock)
                    {
                        if (serverInstance == null)
                            serverInstance = new MongoDbSingleServerStore<PictureServer>("Picture.Server");
                    }
                }
                return serverInstance;
            }
        }
    }
}