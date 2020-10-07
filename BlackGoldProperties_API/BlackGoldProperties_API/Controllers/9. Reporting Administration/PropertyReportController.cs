using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Dynamic;

namespace BlackGoldProperties_API.Controllers._9._Reporting_Administration
{
    public class PropertyReportController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/propertyreport")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] DateTime startdate, [FromUri] DateTime endDate)
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Find user from token
                    //var email = TokenManager.ValidateToken(token);
                    //var user = db.USERs.Where(x => x.USEREMAIL == email).FirstOrDefault();
                    //var generatorName = user.USERNAME;
                    //var generatorSurname = user.USERSURNAME;

                    //Get all properties 
                    var properties = db.PROPERTies.Select(x => new {
                        x.PROPERTYID,
                        x.PROPERTYADDRESS,
                        x.PROPERTYTYPE.PROPERTYTYPEID,
                        x.PROPERTYTYPE.PROPERTYTYPEDESCRIPTION,
                        x.MARKETTYPE.MARKETTYPEID,
                        x.MARKETTYPE.MARKETTYPEDESCRIPTION,
                        x.PROPERTYOWNER.PROPERTYOWNERID,
                        x.PROPERTYOWNER.PROPERTYOWNERNAME,
                        x.PROPERTYOWNER.PROPERTYOWNERSURNAME,
                        x.PROPERTYADDEDDATE,
                        Defects = x.PROPERTYSPACEs.Select(z => z.SPACE.PROPERTYDEFECTs.Select(t => t.DEFECT).ToList()),
                        PointofInterests = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.POINTOFINTEREST.POINTOFINTERESTID, y.POINTOFINTEREST.POINTOFINTERESTNAME, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEID, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION }).ToList(),
                        Municipal = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 5).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).ToList(),
                        //generatorName,
                        //generatorSurname
                    }).OrderBy(z => z.PROPERTYID).ToList();

                    dynamic reportDetails = new ExpandoObject();
                    reportDetails.Properties = properties;
                    reportDetails.ReportDate = DateTime.Now;
                    reportDetails.rentals = db.RENTALs.Select(x => x.RENTALDATESTART).ToList();
                    reportDetails.sales = db.SALEs.Select(x => x.SALEDATECONCLUDED).ToList();
                    var useremail = TokenManager.ValidateToken(token);
                    reportDetails.CurrentUser = db.USERs.Where(x => x.USEREMAIL == useremail).Select(y => new
                    {
                        y.USERNAME,
                        y.USERSURNAME
                    });

                    if (reportDetails == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(reportDetails);
                    }
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
