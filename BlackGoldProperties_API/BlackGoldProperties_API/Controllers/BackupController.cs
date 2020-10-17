using Azure.Core;
using BlackGoldProperties_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;

namespace BlackGoldProperties_API.Controllers
{
    public class BackupController : ApiController
    {
        private readonly string clientId = "3ea491ad-2d59-46bc-9afb-0e595df6f802";
        private readonly string clientSecret = "ObZ6Hp.~wdjLgvuVm6sKNPkUwueZnOCdUN";
        private readonly string tenantId = "ddfa59c8-38d0-49a3-822e-1d3bcb5bf85b";
        private readonly string resource = "https://management.azure.com/";
        private readonly string subscriptionId = "f446f314-c88c-4d86-8580-dc68668928a1"; 
        private static readonly HttpClient client = new HttpClient();
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        //private readonly IHttpClientFactory _clientFactory;
        //private readonly ITokenAcquisition _tokenAcquisition;
        //private readonly IConfiguration _configuration;

        //public ApiService(IHttpClientFactory clientFactory,
        //    ITokenAcquisition tokenAcquisition,
        //    IConfiguration configuration)
        //{
        //    _clientFactory = clientFactory;
        //    _tokenAcquisition = tokenAcquisition;
        //    _configuration = configuration;
        //}

        //LOGIN//
        [HttpPost]
        [Route("api/backup")]          

        public async Task<dynamic> GetApiDataAsync([FromUri] string token)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "resource", resource },
                };

                var content = new FormUrlEncodedContent(values);
                var getAddTokenUrl = "https://login.microsoftonline.com/" + tenantId + "/oauth2/token";
                var response = await client.PostAsync(getAddTokenUrl, content);
                string newResponse = await response.Content.ReadAsStringAsync();
                dynamic dynamic = _Serializer.Deserialize<dynamic>(newResponse);
                var accessToken =  dynamic["access_token"];
                var backupUrl = "https://management.azure.com/subscriptions/" + subscriptionId + "/resourceGroups/BlackGoldProperties/providers/Microsoft.Sql/servers/bgp-db/databases/BlackGoldPropertiesDB/export?api-version=2017-05-10";
                var response2 = await client.GetStringAsync(backupUrl);
                //return response2;
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, backupUrl))
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", accessToken);
                        await client.SendAsync(requestMessage);
                    return requestMessage;
                }



                string authority = "https://login.microsoftonline.com/azure.com";
                    string[] scopes = new string[] { "user.read" };
                    IPublicClientApplication app = PublicClientApplicationBuilder
                         .Create(clientId)
                         .WithAuthority(authority)
                         .Build();
                //var client = _clientFactory.CreateClient();

                var accounts = app.GetAccountsAsync();

                //AuthenticationResult result = null;
                //if (accounts.Any())
                //{
                //    result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                //        .ExecuteAsync();
                //    return result;
                //    //var 
                //}
                //return result;

                //var scope = _configuration["CallApi:ScopeForAccessToken"];
                //var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { scope });

                //client.BaseAddress = new Uri(_configuration["CallApi:ApiBaseAddress"]);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var response = await client.GetAsync("weatherforecast");
                //if (response.IsSuccessStatusCode)
                //{
                //    var responseContent = await response.Content.ReadAsStringAsync();
                //    var data = JArray.Parse(responseContent);

                //    return data;
                //}

                //throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
            }
            catch (Exception e)
            {
                return e;
                //throw new ApplicationException($"Exception {e}");
            }
        }

    }
}
