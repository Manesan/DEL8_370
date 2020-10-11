using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Dynamic;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class SearchPropertiesController : ApiController
    {
        //READ ALL PROPERTIES//
        [HttpGet]
        [Route("api/searchproperties")]
        public IHttpActionResult Get([FromUri] int markettype, [FromUri] int propertytype, [FromUri] string area, [FromUri] int pricefrom, [FromUri] int priceto, [FromUri] int bedroom, [FromUri] int bathroom)
        {
            try
            {
                //Null checks
                if (markettype < 1 || string.IsNullOrEmpty(markettype.ToString()))
                    return BadRequest();
                if (propertytype < 1 || string.IsNullOrEmpty(propertytype.ToString()))
                    return BadRequest();
                if (string.IsNullOrEmpty(area))
                    return BadRequest();

                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all properties that match initial required search criteria 
                var properties = db.PROPERTies.Select(x => new {
                    x.PROPERTYID,
                    x.PROPERTYADDRESS,
                    SUBURBID = (int?)x.SUBURB.SUBURBID,
                    x.SUBURB.SUBURBNAME,
                    CITYID = (int?)x.SUBURB.CITY.CITYID,
                    x.SUBURB.CITY.CITYNAME,
                    MARKETTYPEID = (int?)x.MARKETTYPE.MARKETTYPEID,
                    x.MARKETTYPE.MARKETTYPEDESCRIPTION,
                    PROVINCEID = (int?)x.SUBURB.CITY.PROVINCE.PROVINCEID,
                    x.SUBURB.CITY.PROVINCE.PROVINCENAME,
                    PROPERTYTYPEID = (int?)x.PROPERTYTYPE.PROPERTYTYPEID,
                    x.PROPERTYTYPE.PROPERTYTYPEDESCRIPTION,
                    PropertyFeatures = x.PROPERTYFEATUREs.Select(y => new { y.FEATURE, y.PROPERTYFEATUREQUANTITY }).ToList(),
                    x.PROPERTYSTATU,
                    x.PROPERTYAVAILABLEDATE,
                    InspectionStatus = x.INSPECTIONs.Select(y => new { y.INSPECTIONID, y.IVSTATUSID }).OrderByDescending(y => y.INSPECTIONID).FirstOrDefault(),                    
                    Price = x.PRICEs.OrderByDescending(y => y.PRICEDATE).Select(z => z.PRICEAMOUNT).FirstOrDefault(), 
                    Bedrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 1).FirstOrDefault(),
                    Bathrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 3).FirstOrDefault(),
                    Parking = x.PROPERTYFEATUREs.Select(y => new { y.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).Where(z => z.FEATUREID == 3).FirstOrDefault(),
                    Picture = x.LISTINGPICTUREs.Select(y => new { y.LISTINGPICTUREID, y.LISTINGPICTUREIMAGE }).OrderByDescending(y => y.LISTINGPICTUREID).FirstOrDefault(),
                    Employee = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USER.USEREMAIL }),
                    PropertyPOI = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.POINTOFINTEREST, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION, SUBURBID = (int?)y.SUBURB.SUBURBID, y.SUBURB.SUBURBNAME }).ToList(),
                    Otherbuildingdetail = x.PROPERTYOTHERBUILDINGDETAILs.Select(y => new { OTHERBUILDINGDETAILID = (int?)y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILID, y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILDESCRIPTION }).ToList()
                }).Where(y => y.MARKETTYPEID == markettype && y.PROPERTYTYPEID == propertytype && (y.SUBURBNAME.Contains(area) || y.CITYNAME.Contains(area)) && y.PROPERTYSTATU.PROPERTYSTATUSID == 1 && y.PROPERTYAVAILABLEDATE <= DateTime.Now).ToList();


                if (properties == null)
                {
                    return BadRequest();
                }

                
                if (pricefrom != 999)
                {
                    var filtered = properties.Where(x => x.Price >= pricefrom).ToList();
                    properties = filtered;
                }

                if (priceto != 999)
                {
                    var filtered = properties.Where(x => x.Price <= priceto).ToList();
                    properties = filtered;
                }

                if (bedroom != 999)
                {
                    var filtered = properties.Where(x => x.Bedrooms.PROPERTYSPACEQUANTITY >= bedroom).ToList();
                    properties = filtered;
                }

                if (bathroom != 999)
                {
                    var filtered = properties.Where(x => x.Bathrooms.PROPERTYSPACEQUANTITY >= bathroom).ToList();
                    properties = filtered;
                }
                           
                                
                return Ok(properties);
                
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        //READ ALL Areas (Suburbs and Cities)//
        [HttpGet]
        [Route("api/areas")]
        public IHttpActionResult Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get list of suburbs
                var suburbs = db.SUBURBs.Select(x => x.SUBURBNAME).ToList();

                //Get list of cities
                var cities = db.CITies.Select(x => x.CITYNAME).ToList();

                //Join two lists
                var areas = suburbs.Concat(cities);

                return Ok(areas);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
