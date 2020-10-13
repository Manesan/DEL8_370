using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlackGoldProperties_API.Controllers;
using System.Web.Http;

namespace BlackGoldProperties_API.Models
{
    public class TokenManager
    {
        dynamic db = new LinkToDBController();
        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";

        public class Token
        {
            public string token { get; set; }
        }
        public static string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            var time = LinkToDBController.db.USERLOGINTIMEOUTs.OrderByDescending(y => y.USERLOGINTIMEOUTID).Select(x => x.USERLOGINTIMEOUTDESCRIPTION).FirstOrDefault();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(time)),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static bool Validate(string token)
        {
            try
            {
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                string tokenUseremail = ValidateToken(token);
                if (tokenUseremail == null)
                    return false;
                if (db.USERs.Where(x => x.USEREMAIL == tokenUseremail).FirstOrDefault() == null) //----why did this crash?? -- Should be sorted with previous if statement
                    return false;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public static string ValidateToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            string useremail = usernameClaim.Value;
            return useremail;
        }

        public static dynamic GetRoles(string token) //--Add try catch here
        {
            try
            {
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                var useremail = ValidateToken(token);
                var user = db.USERs.Where(x => x.USEREMAIL == useremail).Select(y => new { y.EMPLOYEE.EMPLOYEEROLEs }).FirstOrDefault();
                dynamic roleList = user.EMPLOYEEROLEs.Select(x => x.EMPLOYEETYPEID).ToList();
                return roleList;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsLoggedIn(string token)
        {
            try
            {
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                var useremail = ValidateToken(token);
                var guids = db.USERs.Where(x => x.USEREMAIL == useremail && x.USERGUIDEXPIRY > DateTime.Now).Count();
                if (guids > 0)
                {
                    var user = db.USERs.Where(x => x.USEREMAIL == useremail && x.USERGUIDEXPIRY > DateTime.Now).FirstOrDefault();
                    /*if (user.USERGUIDEXPIRY.Value <= DateTime.Now.AddMinutes(5))
                    {
                        var time = LinkToDBController.db.USERLOGINTIMEOUTs.OrderByDescending(y => y.USERLOGINTIMEOUTID).Select(x => x.USERLOGINTIMEOUTDESCRIPTION).FirstOrDefault();
                        user.USERGUIDEXPIRY = DateTime.Now.AddMinutes(Convert.ToDouble(time));
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }*/
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}