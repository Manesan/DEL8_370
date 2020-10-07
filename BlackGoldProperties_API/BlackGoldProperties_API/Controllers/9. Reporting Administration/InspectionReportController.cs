using System;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using BlackGoldProperties_API.Models;

namespace BlackGoldProperties_API.Controllers._9._Reporting_Administration
{
    public class InspectionReportController : ApiController
    {
        [HttpGet]
        [Route("api/inspectionreport")]
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
                    var takeonInspections = db.INSPECTIONs.Where(x => x.IVSTATUSID == 2 && x.INSPECTIONDATE >= startdate && x.INSPECTIONDATE <= enddate && x.INSPECTIONTYPEID == 1).Select(y => new
                    {
                        y.INSPECTIONID,
                        y.INSPECTIONDATE,
                        y.PROPERTY.PROPERTYADDRESS,
                        y.INSPECTIONTYPE.INSPECTIONTYPEDESCRIPTION,
                        y.EMPLOYEE.USER.USERNAME,
                        y.EMPLOYEE.USER.USERSURNAME,
                        Defects = y.PROPERTYDEFECTs.Select(z => new { z.DEFECT.DEFECTDESCRIPTION }).ToList(),
                        y.INSPECTIONDOCUMENT
                    }).ToList();

                    var outgoingInspections = db.INSPECTIONs.Where(x => x.IVSTATUSID == 2 && x.INSPECTIONDATE >= startdate && x.INSPECTIONDATE <= enddate && x.INSPECTIONTYPEID == 2).Select(y => new
                    {
                        y.INSPECTIONID,
                        y.INSPECTIONDATE,
                        y.PROPERTY.PROPERTYADDRESS,
                        y.INSPECTIONTYPE.INSPECTIONTYPEDESCRIPTION,
                        y.EMPLOYEE.USER.USERNAME,
                        y.EMPLOYEE.USER.USERSURNAME,
                        Defects = y.PROPERTYDEFECTs.Select(z => new { z.DEFECT.DEFECTDESCRIPTION }).ToList(),
                        y.INSPECTIONDOCUMENT
                    }).ToList();

                    var allInspections = db.INSPECTIONs.Where(x => x.IVSTATUSID == 2 && x.INSPECTIONDATE >= startdate && x.INSPECTIONDATE <= enddate).Select(y => new
                    {
                        y.INSPECTIONID,
                        y.INSPECTIONDATE,
                        y.PROPERTY.PROPERTYADDRESS,
                        y.INSPECTIONTYPE.INSPECTIONTYPEDESCRIPTION,
                        y.EMPLOYEE.USER.USERNAME,
                        y.EMPLOYEE.USER.USERSURNAME,
                        Defects = y.PROPERTYDEFECTs.Select(z => new { z.DEFECT.DEFECTDESCRIPTION }).ToList(),
                        y.INSPECTIONDOCUMENT
                    }).ToList();

                    dynamic reportDetails = new ExpandoObject();
                    reportDetails.takeonInspections = takeonInspections;
                    reportDetails.outgoingInspections = outgoingInspections;
                    reportDetails.allInspections = allInspections;
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
