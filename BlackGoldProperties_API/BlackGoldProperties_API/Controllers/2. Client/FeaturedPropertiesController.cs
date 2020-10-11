﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Data;
using System.Dynamic;
using Microsoft.Ajax.Utilities;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class FeaturedPropertiesController : ApiController
    {
        //GET 3 FEATURED PROPERTIES//
        [HttpGet]
        [Route("api/featured")]
        public dynamic Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all properties
                var properties = db.PROPERTies.Where(z => z.PROPERTYSTATU.PROPERTYSTATUSID == 1 && z.PROPERTYAVAILABLEDATE <= DateTime.Now).Select(x => new {
                    x.PROPERTYID,
                    x.PROPERTYADDRESS,
                    x.PROPERTYSTATU,
                    x.PROPERTYAVAILABLEDATE,
                    SUBURBID = (int?)x.SUBURB.SUBURBID,
                    x.SUBURB.SUBURBNAME,
                    //CITYID = (int?)x.SUBURB.CITY.CITYID,
                    x.SUBURB.CITY.CITYNAME,
                    MARKETTYPEID = (int?)x.MARKETTYPE.MARKETTYPEID,
                    x.MARKETTYPE.MARKETTYPEDESCRIPTION,
                    //PROVINCEID = (int?)x.SUBURB.CITY.PROVINCE.PROVINCEID,
                    x.SUBURB.CITY.PROVINCE.PROVINCENAME,
                    PROPERTYTYPEID = (int?)x.PROPERTYTYPE.PROPERTYTYPEID,
                    x.PROPERTYTYPE.PROPERTYTYPEDESCRIPTION,
                    PropertyFeatures = x.PROPERTYFEATUREs.Select(y => new { y.FEATURE, y.PROPERTYFEATUREQUANTITY }).ToList(),
                    //InspectionStatus = x.INSPECTIONs.Select(y => new {y.INSPECTIONID, y.IVSTATUSID }).OrderByDescending(y => y.INSPECTIONID).FirstOrDefault(),
                    Price = x.PRICEs.OrderByDescending(y => y.PRICEDATE).Select(z => z.PRICEAMOUNT).FirstOrDefault(),
                    Bedrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 1).FirstOrDefault(),
                    Bathrooms = x.PROPERTYSPACEs.Select(y => new { y.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 3).FirstOrDefault(),
                    //Parking = x.PROPERTYFEATUREs.Select(y => new { y.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).Where(z => z.FEATUREID == 3).FirstOrDefault(),
                    Picture = x.LISTINGPICTUREs.Select(y => new { y.LISTINGPICTUREID, y.LISTINGPICTUREIMAGE }).OrderByDescending(y => y.LISTINGPICTUREID).FirstOrDefault(),
                    //Employee = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USER.USEREMAIL }),
                    //PropertyPOI = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.POINTOFINTEREST, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION, SUBURBID = (int?)y.SUBURB.SUBURBID, y.SUBURB.SUBURBNAME }).ToList(),
                    //Otherbuildingdetail = x.PROPERTYOTHERBUILDINGDETAILs.Select(y => new { OTHERBUILDINGDETAILID = (int?)y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILID, y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILDESCRIPTION }).ToList()
                }).ToList();

                if (properties == null)
                {
                    return BadRequest();
                }
                else
                {
                    var counter = properties.Count();
      
                    int a, b, c;
                    Random rand = new Random();
                    do
                    {
                        a = rand.Next(1, counter);
                        b = rand.Next(1, counter);
                        c = rand.Next(1, counter);
                    } while ((a == b) || (b == c) || (a == c));


                    object[] featured = new object[3];
                    featured[0] = properties[a];
                    featured[1] = properties[b];
                    featured[2] = properties[c];


                    return Ok(featured);
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
