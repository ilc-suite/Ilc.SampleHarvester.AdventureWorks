using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PictureServer.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";
        
        protected CustomPrincipal CurrentUser
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
            set { Thread.CurrentPrincipal = value as CustomPrincipal; }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;

                if(authValue == null)
                    throw new AuthenticationException("Invalid credentials");

                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)
                {
                    Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);

                    if(parsedCredentials == null)
                        throw new AuthenticationException("Invalid credentials");

                    if (parsedCredentials != null)
                    {
                        var user = UserStore.Identities.FirstOrDefault(u => u.Username == parsedCredentials.Username && u.Password == parsedCredentials.Password);
                        if (user == null)
                            throw new AuthenticationException("Invalid credentials");

                        var roles = user.Roles.Select(m => m.RoleName).ToArray();

                        CurrentUser = new CustomPrincipal(parsedCredentials.Username, roles);

                        if (!String.IsNullOrEmpty(Roles))
                        {
                            if (!CurrentUser.IsInRole(Roles))
                            {
                                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                return;
                            }
                        }

                        if (!String.IsNullOrEmpty(Users))
                        {
                            if (!Users.Contains(CurrentUser.UserId.ToString()))
                            {
                                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
        }

        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new Credentials() { Username = credentials[0], Password = credentials[1], };
        }
    }
}