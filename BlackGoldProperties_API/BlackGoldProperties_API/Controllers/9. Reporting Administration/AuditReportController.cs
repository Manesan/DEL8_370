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
    public class AuditReportController : ApiController
    {

        //READ ALL DATA//
        [HttpGet]
        [Route("api/auditreport")]
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    dynamic reportDetails = new ExpandoObject();
                    
                    //Sales
                    var sales = db.SALEs.Select(x => new
                    {
                        x.SALEAMOUNT
                    }).OrderByDescending(y => y.SALEAMOUNT).ToList();
                    reportDetails.TotalPropertySales = sales.Count;
                    reportDetails.AverageSalePrice = sales.Average(x => x.SALEAMOUNT);
                    reportDetails.MaxSalePrice = sales.FirstOrDefault();
                    reportDetails.MinSalePrice = sales.LastOrDefault();

                    //Rentals
                    var rentals = db.RENTALs.Select(x => new
                    {
                        PRICEAMOUNT = x.PROPERTY.PRICEs.OrderByDescending(y => y.PRICEDATE).Select(z => z.PRICEAMOUNT).FirstOrDefault()
                    }).OrderByDescending(y => y.PRICEAMOUNT).ToList();
                    reportDetails.TotalPropertiesRented = rentals.Count;
                    reportDetails.AverageRentalPrice = rentals.Average(x => x.PRICEAMOUNT);
                    reportDetails.MaxRentalPrice = rentals.FirstOrDefault();
                    reportDetails.MinRentalPrice = rentals.LastOrDefault();

                    //Property total
                    reportDetails.PropertyCount = db.PROPERTies.Count();

                    reportDetails.ReportDate = DateTime.Now;
                    //reportDetails.rentals = db.RENTALs.Select(x => x.RENTALDATESTART).ToList();
                    //reportDetails.sales = db.SALEs.Select(x => x.SALEDATECONCLUDED).ToList();
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
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }

    }
}
