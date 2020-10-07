using BlackGoldProperties_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace BlackGoldProperties_API.Controllers
{
    public class HomeController : ApiController
    {
        [HttpPost]
        public static dynamic Login(string UserEmail, string Password)
        {
            try
            {
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                string hashedPasword = ComputeSha256Hash(Password);
                USER user = db.USERs.Include(x => x.EMPLOYEE).Include(x => x.EMPLOYEE.EMPLOYEEROLEs).Where(y => y.USEREMAIL == UserEmail && y.USERPASSWORD == hashedPasword).FirstOrDefault();

                if (user != null)
                {
                    UserModel userModel = new UserModel
                    {
                        user = user
                    };
                    userModel.RefreshGUID(db);
                    dynamic userToken = new System.Dynamic.ExpandoObject();
                    userToken.token = TokenManager.GenerateToken(UserEmail);
                    if (userModel.user.EMPLOYEE != null)
                    {
                        userToken.roles = new List<int>();
                        foreach (var role in user.EMPLOYEE.EMPLOYEEROLEs)
                        {
                            userToken.roles.Add(role.EMPLOYEETYPEID);
                        }
                    }
                    if (TokenManager.Validate(userToken.token) == true)
                    {
                        return userToken;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception)
            {
                return 404;
            }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GetToken(List<JObject> objectList)
        {
            TokenManager.Token t = JsonConvert.DeserializeObject<TokenManager.Token>(objectList[1].ToString());
            string token = t.token;
            return token;
        }

        public static string HashPassword(string password)
        {
            return ComputeSha256Hash(password);
        }

        public static dynamic GUIDActions()
        {
            USER GUIDUser = new USER();
            GUIDUser.USERGUID = Guid.NewGuid().ToString();
            var time = LinkToDBController.db.USERLOGINTIMEOUTs.OrderByDescending(y => y.USERLOGINTIMEOUTID).Select(x => x.USERLOGINTIMEOUTDESCRIPTION).FirstOrDefault();
            GUIDUser.USERGUIDEXPIRY = DateTime.Now.AddMinutes(Convert.ToDouble(time));
            return GUIDUser;
        }
    }
}
