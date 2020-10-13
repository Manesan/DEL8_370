using System;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using BlackGoldProperties_API.Models;

namespace BlackGoldProperties_API.Controllers._5._Valuation_Administration
{
    public class ValuationReportController : ApiController
    {
        [HttpGet]
        [Route("api/valuationreport")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] DateTime startdate, [FromUri] DateTime enddate)
        {

            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Retreive inspection report info
                    var valuations = db.VALUATIONs.Where(x => x.VALUATIONDATE >= startdate && x.VALUATIONDATE <= enddate).Select(y => new
                    {
                        y.VALUATIONDATE,
                        y.PROPERTY.PROPERTYADDRESS,
                        y.VALUATIONDESCRIPTION,
                        y.EMPLOYEE.USER.USERNAME,
                        y.EMPLOYEE.USER.USERSURNAME,
                        y.VALUATIONAMOUNT
                    }).OrderByDescending(z => z.VALUATIONDATE).ToList();

                    dynamic reportDetails = new ExpandoObject();
                    reportDetails.valuations = valuations;
                    reportDetails.ReportDate = DateTime.Now;
                    var useremail = TokenManager.ValidateToken(token);

                    reportDetails.CurrentUser = db.USERs.Where(x => x.USEREMAIL == useremail).Select(y => new
                    {
                        y.USERNAME,
                        y.USERSURNAME
                    });

                    if (reportDetails == null)
                        return BadRequest();
                    else
                        return Ok(reportDetails);
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }
    }
}


