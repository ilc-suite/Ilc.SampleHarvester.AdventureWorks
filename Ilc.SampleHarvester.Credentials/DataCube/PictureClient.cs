using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using RestSharp;

namespace Ilc.SampleHarvester.ExpandContact.DataCube
{
    public class PictureClient
    {
        private RestClient client;

        public PictureClient(string serverUrl, string username, string password)
        {
            client = new RestClient(serverUrl);
            client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public string GetPictureUrl()
        {
            var request = new RestRequest("api/Picture");

            var response = client.Execute(request);

            if(response.StatusCode == HttpStatusCode.Unauthorized)
                throw new IlcInvalidCredentialsException("Invalid credentials!");

            if(response.StatusCode == HttpStatusCode.Forbidden)
                throw new IlcAccessDeniedException("You are not allowed to access this resource!");

            if(response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Something went wrong!");

            return Json.JsonUtil.Deserialize<string>(response.Content);
;
        }

        public bool IsLoggedIn()
        {
            var request = new RestRequest("api/Test");
            var response = client.Execute(request);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}