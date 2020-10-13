﻿using System;
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

                    //Get all properties 
                    /*var properties = db.PROPERTies.Select(x => new {
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
                        
                        //municipality valuation and Rates & Taxes
                        Municipal = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 5).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYDOCUMENTID).FirstOrDefault(),

                        //Title Deed
                        TitleDeed = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 1).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYDOCUMENTID).FirstOrDefault(),

                        //Municipality Report
                        MunicipalReport = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 2).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYDOCUMENTID).FirstOrDefault(),

                        //Valuation Document
                        Valuation = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 3).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYDOCUMENTID).FirstOrDefault(),

                        //Lights and Water
                        LightsWater = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 4).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYDOCUMENTID).FirstOrDefault(),

                        //Levies
                        Levies = x.PROPERTYDOCUMENTs.Where(y => y.PROPERTYDOCUMENTTYPEID == 6).Select(y => new { y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYDOCUMENTID).FirstOrDefault(),

                        //generatorName,
                        //generatorSurname
                    }).OrderBy(z => z.PROPERTYID).ToList();*/

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
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }

    }
}
