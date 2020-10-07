using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class BrowsePropertiesController : ApiController
    {
        //browse properties on the client side

        //READ ALL Properties//
        [HttpGet]
        [Route("api/browseproperties")]
        public IHttpActionResult Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all properties
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
                    InspectionStatus = x.INSPECTIONs.Select(y => new { y.INSPECTIONID, IVSTATUSID = (int?)y.IVSTATUSID }).OrderByDescending(y => y.INSPECTIONID).FirstOrDefault(),
                    x.PROPERTYAVAILABLEDATE,
                    PropertyFeatures = x.PROPERTYFEATUREs.Select(y => new { y.FEATURE, y.PROPERTYFEATUREQUANTITY }).ToList(),
                    x.PROPERTYSTATU,
                    Price = x.PRICEs.OrderByDescending(y => y.PRICEDATE).Select(z => z.PRICEAMOUNT).FirstOrDefault(),
                    //PropertySpaces = x.PROPERTYSPACEs.Select(y => new { y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).ToList(),
                    Bedrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 1).FirstOrDefault(),
                    Bathrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 3).FirstOrDefault(),
                    Parking = x.PROPERTYFEATUREs.Select(y => new { y.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).Where(z => z.FEATUREID == 3).FirstOrDefault(),
                    Picture = x.LISTINGPICTUREs.Select(y => new { y.LISTINGPICTUREID, y.LISTINGPICTUREIMAGE }).OrderByDescending(y => y.LISTINGPICTUREID).FirstOrDefault(),
                    Employee = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USER.USEREMAIL }),
                    PropertyPOI = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.POINTOFINTEREST, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION, SUBURBID = (int?)y.SUBURB.SUBURBID, y.SUBURB.SUBURBNAME }).ToList(),
                    Otherbuildingdetail = x.PROPERTYOTHERBUILDINGDETAILs.Select(y => new { OTHERBUILDINGDETAILID = (int?)y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILID, y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILDESCRIPTION }).ToList()
                }).Where(z => z.PROPERTYSTATU.PROPERTYSTATUSID == 1 && z.PROPERTYAVAILABLEDATE <= DateTime.Now).ToList();

                if (properties == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(properties);
                }
            }
            catch (Exception)
            {

                return NotFound();
            }
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/browseproperties")]
        public IHttpActionResult Get([FromUri] int id)
        {
            if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get specified property
                var property = db.PROPERTies.Where(z => z.PROPERTYID == id).Select(x => new {
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
                    Price = x.PRICEs.OrderByDescending(y => y.PRICEDATE).Select(z => z.PRICEAMOUNT).FirstOrDefault(),
                    //PropertySpaces = x.PROPERTYSPACEs.Select(y => new { y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).ToList(),
                    Bedrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 1).FirstOrDefault(),
                    Bathrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 3).FirstOrDefault(),
                    Parking = x.PROPERTYFEATUREs.Select(y => new { y.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).Where(z => z.FEATUREID == 3).FirstOrDefault(),
                    Picture = x.LISTINGPICTUREs.Select(y => new { y.LISTINGPICTUREID, y.LISTINGPICTUREIMAGE }).OrderByDescending(y => y.LISTINGPICTUREID).ToList(),
                    Employee = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USER.USEREMAIL }),
                    PropertyPOI = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.POINTOFINTEREST, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION, SUBURBID = (int?)y.SUBURB.SUBURBID, y.SUBURB.SUBURBNAME }).ToList(),
                    Otherbuildingdetail = x.PROPERTYOTHERBUILDINGDETAILs.Select(y => new { OTHERBUILDINGDETAILID = (int?)y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILID, y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILDESCRIPTION }).ToList(),
                    Agent = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USER.USERID, y.EMPLOYEE.USER.USERNAME, y.EMPLOYEE.USER.USERSURNAME, y.EMPLOYEE.USER.USEREMAIL }).FirstOrDefault(),
                    Terms = db.TERMs.Where(z => z.TERMID <= x.TERM.TERMID && z.TERMID >= x.TERM1.TERMID),
                }).FirstOrDefault();

                if (property == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(property);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
