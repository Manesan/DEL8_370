using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class PropertyController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/property")]
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
                    var property = db.PROPERTies.Select(x => new {
                        x.PROPERTYID,
                        x.PROPERTYADDRESS,
                        /*PROPERTYSTATUSID = (int?)x.PROPERTYSTATU.PROPERTYSTATUSID,
                        x.PROPERTYSTATU.PROPERTYSTATUSDESCRIPTION,
                        Price = x.PRICEs.OrderByDescending(y => y.PRICEDATE).FirstOrDefault(),
                        x.PROPERTYOWNER.PROPERTYOWNERID,
                        x.PROPERTYOWNER.PROPERTYOWNERNAME,
                        x.PROPERTYOWNER.PROPERTYOWNERSURNAME,
                        x.PROPERTYOWNER.PROPERTYOWNEREMAIL,
                        x.PROPERTYOWNER.PROPERTYOWNERADDRESS,
                        x.PROPERTYOWNER.PROPERTYOWNERIDNUMBER,
                        x.PROPERTYOWNER.PROPERTYOWNERPASSPORTNUMBER,
                        x.PROPERTYOWNER.PROPERTYOWNERCONTACTNUMBER,
                        x.PROPERTYOWNER.PROPERTYOWNERALTCONTACTNUMBER,
                        PropertyFeatures = x.PROPERTYFEATUREs.Select(y => new { y.FEATURE.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).ToList(),
                        Pointsofinterest = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.SUBURB.SUBURBID, y.SUBURB.SUBURBNAME, y.POINTOFINTEREST.POINTOFINTERESTID, y.POINTOFINTEREST.POINTOFINTERESTNAME, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEID, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION }).ToList(),
                        Mandates = x.PROPERTYMANDATEs.Select(y => new { y.PROPERTYMANDATEID, y.MANDATE.MANDATEID, y.MANDATE.MANDATEDATE, y.MANDATE.MANDATEDOCUMENT, MANDATETYPEID = (int?)y.MANDATE.MANDATETYPE.MANDATETYPEID, y.MANDATE.MANDATETYPE.MANDATETYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYMANDATEID).FirstOrDefault(),
                        Agent = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USERID, y.EMPLOYEE.USER.USERNAME, y.EMPLOYEE.USER.USERSURNAME, y.EMPLOYEE.USER.USEREMAIL }).ToList(),
                        MARKETTYPEID = (int?)x.MARKETTYPE.MARKETTYPEID,
                        x.MARKETTYPE.MARKETTYPEDESCRIPTION,
                        PROPERTYTYPEID = (int?)x.PROPERTYTYPE.PROPERTYTYPEID,
                        x.PROPERTYTYPE.PROPERTYTYPEDESCRIPTION,*/
                        SUBURBID = (int?)x.SUBURB.SUBURBID,
                        x.SUBURB.SUBURBNAME,
                        /*CITYID = (int?)x.SUBURB.CITY.CITYID,
                        x.SUBURB.CITY.CITYNAME,
                        PROVINCEID = (int?)x.SUBURB.CITY.PROVINCE.PROVINCEID,
                        x.SUBURB.CITY.PROVINCE.PROVINCENAME,
                        Spaces = x.PROPERTYSPACEs.Select(y => new { SPACEID = (int?)y.SPACE.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY }).ToList(),
                        Otherbuildingdetails = x.PROPERTYOTHERBUILDINGDETAILs.Select(y => new { y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILID, y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILDESCRIPTION }).ToList(),
                        x.BUILDINGCONDITION,
                        x.BUILDINGCONDITIONID,*/
                    }).ToList();

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
            return Unauthorized();
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/property")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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

                    //Get specified property
                    var property = db.PROPERTies.Where(z => z.PROPERTYID == id).Select(x => new {
                        x.PROPERTYID,
                        x.PROPERTYADDRESS,
                        PROPERTYSTATUSID = (int?)x.PROPERTYSTATU.PROPERTYSTATUSID,
                        x.PROPERTYSTATU.PROPERTYSTATUSDESCRIPTION,
                        Price = x.PRICEs.OrderByDescending(y => y.PRICEDATE).FirstOrDefault(),
                        x.PROPERTYOWNER.PROPERTYOWNERID,
                        x.PROPERTYOWNER.PROPERTYOWNERNAME,
                        x.PROPERTYOWNER.PROPERTYOWNERSURNAME,
                        x.PROPERTYOWNER.PROPERTYOWNEREMAIL,
                        x.PROPERTYOWNER.PROPERTYOWNERADDRESS,
                        PROPERTYOWNERIDNUMBER = x.PROPERTYOWNER.PROPERTYOWNERIDNUMBER.Trim(),
                        PROPERTYOWNERPASSPORTNUMBER = x.PROPERTYOWNER.PROPERTYOWNERPASSPORTNUMBER.Trim(),
                        PROPERTYOWNERCONTACTNUMBER = x.PROPERTYOWNER.PROPERTYOWNERCONTACTNUMBER.Trim(),
                        PROPERTYOWNERALTCONTACTNUMBER = x.PROPERTYOWNER.PROPERTYOWNERALTCONTACTNUMBER.Trim(),
                        PropertyFeatures = x.PROPERTYFEATUREs.Select(y => new { FEATUREID = (int?)y.FEATURE.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).ToList(),
                        Pointsofinterest = x.SUBURB.SUBURBPOINTOFINTERESTs.Select(y => new { y.SUBURB.SUBURBID, y.SUBURB.SUBURBNAME, y.POINTOFINTEREST.POINTOFINTERESTID, y.POINTOFINTEREST.POINTOFINTERESTNAME, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEID, y.POINTOFINTEREST.POINTOFINTERESTTYPE.POINTOFINTERESTTYPEDESCRIPTION }).ToList(),
                        Mandates = x.PROPERTYMANDATEs.Select(y => new { y.PROPERTYMANDATEID, y.MANDATE.MANDATEID, y.MANDATE.MANDATEDATE, y.MANDATE.MANDATEDOCUMENT, MANDATETYPEID = (int?)y.MANDATE.MANDATETYPE.MANDATETYPEID, y.MANDATE.MANDATETYPE.MANDATETYPEDESCRIPTION }).OrderByDescending(y => y.PROPERTYMANDATEID).FirstOrDefault(),
                        Agent = x.EMPLOYEEPROPERTies.Select(y => new { y.EMPLOYEE.USERID, y.EMPLOYEE.USER.USERNAME, y.EMPLOYEE.USER.USERSURNAME, y.EMPLOYEE.USER.USEREMAIL }).ToList(),
                        MARKETTYPEID = (int?)x.MARKETTYPE.MARKETTYPEID,
                        x.MARKETTYPE.MARKETTYPEDESCRIPTION,
                        PROPERTYTYPEID = (int?)x.PROPERTYTYPE.PROPERTYTYPEID,
                        x.PROPERTYTYPE.PROPERTYTYPEDESCRIPTION,
                        SUBURBID = (int?)x.SUBURB.SUBURBID,
                        x.SUBURB.SUBURBNAME,
                        CITYID = (int?)x.SUBURB.CITY.CITYID,
                        x.SUBURB.CITY.CITYNAME,
                        PROVINCEID = (int?)x.SUBURB.CITY.PROVINCE.PROVINCEID,
                        x.SUBURB.CITY.PROVINCE.PROVINCENAME,
                        //Spaces = x.PROPERTYSPACEs.Select(y => new { y.SPACE.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY }).ToList(),
                        Bedrooms = x.PROPERTYSPACEs.Select(y => new { SPACEID = (int?)y.SPACE.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 1).FirstOrDefault(),
                        Bathrooms = x.PROPERTYSPACEs.Select(y => new { SPACEID = (int?)y.SPACE.SPACEID, y.SPACE.SPACEDESCRIPTION, y.PROPERTYSPACEQUANTITY, y.SPACE.SPACETYPE.SPACETYPEDESCRIPTION }).Where(z => z.SPACEID == 3).FirstOrDefault(),
                        Parking = x.PROPERTYFEATUREs.Select(y => new { y.FEATUREID, y.FEATURE.FEATUREDESCRIPTION, y.PROPERTYFEATUREQUANTITY }).Where(z => z.FEATUREID == 3).FirstOrDefault(),
                        Otherbuildingdetails = x.PROPERTYOTHERBUILDINGDETAILs.Select(y => new { OTHERBUILDINGDETAILID = (int?)y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILID, y.OTHERBUILDINGDETAIL.OTHERBUILDINGDETAILDESCRIPTION }).ToList(),
                        Picture = x.LISTINGPICTUREs.Select(y => new { y.LISTINGPICTUREID, y.LISTINGPICTUREIMAGE }).OrderByDescending(y => y.LISTINGPICTUREID).FirstOrDefault(),
                        x.BUILDINGCONDITION,
                        x.BUILDINGCONDITIONID,
                        Mintermid = x.TERM.TERMID,
                        Mintermdescription = x.TERM.TERMDESCRIPTION,
                        Maxtermid = x.TERM1.TERMID,
                        Maxtermdescription = x.TERM1.TERMDESCRIPTION,
                        x.PROPERTYRATEANDTAX,
                        x.PROPERTYLEVIES,
                        Zoning = x.ZONINGs.OrderByDescending(z => z.ZONINGID).FirstOrDefault(),
                        Terms = db.TERMs.Where(z => z.TERMID <= x.TERM.TERMID && z.TERMID >= x.TERM1.TERMID),
                        x.PROPERTYAVAILABLEDATE
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
            return Unauthorized();
        }

        //ADD//   
        [HttpPost]
        [Route("api/property")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string address, [FromUri] decimal price, [FromUri] string ownername, [FromUri] string ownersurname, [FromUri] string owneremail, [FromUri] string owneraddress, [FromUri] string owneridnumber, [FromUri] string ownerpassportnumber, [FromUri] string ownercontactnumber, [FromUri] string owneraltcontactnumber, [FromUri] int markettypeid, [FromUri] int propertytypeid, [FromUri] DateTime availabledate, [FromUri] int suburbid, [FromUri] int mandatetypeid, [FromUri] DateTime mandatedate, [FromUri] int agentid, [FromBody] dynamic propertydetails, [FromUri] int minterm, [FromUri] int maxterm, [FromUri] decimal ratesandtax, [FromUri] int condition, [FromUri] decimal? municipalvaluation, [FromUri] decimal? monthlyrates, [FromUri] string period, [FromUri] string usagecategory, [FromUri] string yearofvaluation, [FromUri] string zoningusage, [FromUri] decimal? levies)
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

                    //Null checks   --- FINISH THIS
                    //if (string.IsNullOrEmpty(address))
                        //return BadRequest();

                    //Add the property owner
                    db.PROPERTYOWNERs.Add(new PROPERTYOWNER
                    {
                        PROPERTYOWNERNAME = ownername,
                        PROPERTYOWNERSURNAME = ownersurname,
                        PROPERTYOWNEREMAIL = owneremail,
                        PROPERTYOWNERIDNUMBER = Utilities.Trimmer(owneridnumber),
                        PROPERTYOWNERPASSPORTNUMBER = Utilities.Trimmer(ownerpassportnumber),
                        PROPERTYOWNERADDRESS = owneraddress,
                        PROPERTYOWNERCONTACTNUMBER = Utilities.Trimmer(ownercontactnumber),
                        PROPERTYOWNERALTCONTACTNUMBER = Utilities.Trimmer(owneraltcontactnumber)
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Get newly registered property owner
                    int lastownerid = db.PROPERTYOWNERs.Max(item => item.PROPERTYOWNERID);


                    //Set status based on available date
                    int statusID = 1; //'Available'
                    if (availabledate > DateTime.Now)
                    {
                        statusID = 4; //'Not Available'
                    }

                    //Add a property
                    db.PROPERTies.Add(new PROPERTY
                    {
                        SUBURBID = suburbid,
                        MARKETTYPEID = markettypeid,
                        PROPERTYSTATUSID = statusID, //property is flagged as 'Available'
                        PROPERTYOWNERID = lastownerid, //assign newly registered property owner to the property
                        PROPERTYTYPEID = propertytypeid,
                        PROPERTYADDRESS = address,
                        MINTERMID = minterm / 6,
                        MAXTERMID = maxterm / 6,
                        BUILDINGCONDITIONID = condition,
                        PROPERTYRATEANDTAX = ratesandtax,
                        PROPERTYLEVIES = levies,
                        PROPERTYADDEDDATE = DateTime.Now,
                        PROPERTYAVAILABLEDATE = availabledate,
                    });


                    //Save DB changes
                    db.SaveChanges();

                    //Get newly added property
                    int lastpropertyid = db.PROPERTies.Max(item => item.PROPERTYID);


                    db.ZONINGs.Add(new ZONING
                    {
                        PROPERTYID = lastpropertyid,
                        ZONINGMUNICIPALVALUATION = municipalvaluation,
                        ZONINGRATINGPERIOD = period,
                        ZONINGUSAGECATEGORY = usagecategory,
                        ZONINGYEAROFVALUATION = yearofvaluation,
                        ZONINGESTIMATEDMONTHLYRATES = monthlyrates,
                        ZONINGUSAGE = zoningusage
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Add employee to the property    -- TEST THIS
                    db.EMPLOYEEPROPERTies.Add(new EMPLOYEEPROPERTY
                    {
                        PROPERTYID = lastpropertyid,
                        USERID = agentid
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Add the price to the property
                    db.PRICEs.Add(new PRICE
                    {
                        PROPERTYID = lastpropertyid,
                        PRICEAMOUNT = price,
                        PRICEDATE = DateTime.Now //Sets the price datetime to the current date and time
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Assign property details to arrays
                    Newtonsoft.Json.Linq.JArray otherBuildingDetails = propertydetails[0];
                    Newtonsoft.Json.Linq.JArray features = propertydetails[1];
                    Newtonsoft.Json.Linq.JArray bedAndBathQuantities = propertydetails[2];

                    //Add other building details to the property 
                    foreach (dynamic otherBuildingDetail in otherBuildingDetails)
                    {
                        db.PROPERTYOTHERBUILDINGDETAILs.Add(new PROPERTYOTHERBUILDINGDETAIL
                        {
                            PROPERTYID = lastpropertyid,
                            OTHERBUILDINGDETAILID = otherBuildingDetail.OTHERBUILDINGDETAILID
                        });
                    }

                    //Save DB changes
                    db.SaveChanges();

                    //Add features to the property                    
                    for (int i = 0; i < features.Count; i++)
                    {
                        db.PROPERTYFEATUREs.Add(new PROPERTYFEATURE
                        {
                            PROPERTYID = lastpropertyid,
                            FEATUREID = (int?)features[i][0],
                            PROPERTYFEATUREQUANTITY = (int)features[i][1]
                        });
                    }

                    //Save DB changes
                    db.SaveChanges();

                    //Add Bedroom quantity
                    db.PROPERTYSPACEs.Add(new PROPERTYSPACE
                    {
                        PROPERTYID = lastpropertyid,
                        SPACEID = 1,
                        PROPERTYSPACEQUANTITY = (int)bedAndBathQuantities[0] //Index zero for bedroom quantity
                    });

                    //Save DB changes
                    db.SaveChanges();

                    // Add Bathroom quantity
                    db.PROPERTYSPACEs.Add(new PROPERTYSPACE
                    {
                        PROPERTYID = lastpropertyid,
                        SPACEID = 3,
                        PROPERTYSPACEQUANTITY = (int)bedAndBathQuantities[1] //Index zero for bathroom quantity
                    });

                    //Save DB changes
                    db.SaveChanges();


                    //Add spaces to the property
                    //foreach (dynamic quantity in propertydetails[2])
                    //{
                    //    db.PROPERTYSPACEs.Add(new PROPERTYSPACE
                    //    {
                    //        PROPERTYID = lastpropertyid,
                    //        SPACEID = item.spaceid,
                    //        PROPERTYSPACEQUANTITY = item.propertyspacequanity
                    //    });
                    //}

                    //Add mandate
                    //db.MANDATEs.Add(new MANDATE
                    //{
                    //    MANDATEDATE = mandatedate,
                    //    MANDATETYPEID = mandatetypeid,
                    //    MANDATEDOCUMENT = DocumentController.UploadFile(DocumentController.Containers.mandateDocumentsContainer, propertydetails[4])
                    //});

                    //Get newly added mandate
                    int lastmandateid = db.MANDATEs.Max(item => item.MANDATEID);

                    db.PROPERTYMANDATEs.Add(new PROPERTYMANDATE
                    {
                        PROPERTYID = lastpropertyid,
                        MANDATEID = lastmandateid
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Assign a valuation to the property
                    db.VALUATIONs.Add(new VALUATION
                    {
                        PROPERTYID = lastpropertyid,
                        USERID = 19, // Assign default inspector
                        IVSTATUSID = 3, //'Not Available'
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //UPDATE//
        [HttpPatch]
        [Route("api/property")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string address, [FromUri] decimal price, [FromUri] string ownername, [FromUri] string ownersurname, [FromUri] string owneremail, [FromUri] string owneraddress, [FromUri] string owneridnumber, [FromUri] string ownerpassportnumber, [FromUri] string ownercontactnumber, [FromUri] string owneraltcontactnumber, [FromUri] int markettypeid, [FromUri] int propertytypeid, [FromUri] DateTime availabledate, [FromUri] int suburbid, [FromUri] int? mandatetypeid, [FromUri] DateTime? mandatedate, [FromUri] int agentid, [FromBody] dynamic propertydetails, [FromUri] int minterm, [FromUri] int maxterm, [FromUri] decimal ratesandtax, [FromUri] int condition, [FromUri] decimal? municipalvaluation, [FromUri] decimal? monthlyrates, [FromUri] string period, [FromUri] string usagecategory, [FromUri] string yearofvaluation, [FromUri] string zoningusage, [FromUri] decimal? levies)
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                /*try
                {*/
                //DB context
                var db = LinkToDBController.db;
                var properties = db.PROPERTies.Include(y => y.PROPERTYOWNER).FirstOrDefault(x => x.PROPERTYID == id);

                //Null checks   --Finish this
                if (string.IsNullOrEmpty(address))
                    return BadRequest();

                //Set status based on available date
                int statusID = 1;
                if (availabledate < DateTime.Now)
                {
                    statusID = 4;
                }

                //Update specified property
                properties.PROPERTYOWNER.PROPERTYOWNERNAME = ownername;
                properties.PROPERTYOWNER.PROPERTYOWNERSURNAME = ownersurname;
                properties.PROPERTYOWNER.PROPERTYOWNEREMAIL = owneremail;
                properties.PROPERTYOWNER.PROPERTYOWNERADDRESS = owneraddress;
                properties.PROPERTYOWNER.PROPERTYOWNERIDNUMBER = Utilities.Trimmer(owneridnumber);
                properties.PROPERTYOWNER.PROPERTYOWNERPASSPORTNUMBER = Utilities.Trimmer(ownerpassportnumber);
                properties.PROPERTYOWNER.PROPERTYOWNERCONTACTNUMBER = Utilities.Trimmer(ownercontactnumber);
                properties.PROPERTYOWNER.PROPERTYOWNERALTCONTACTNUMBER = Utilities.Trimmer(owneraltcontactnumber);
                properties.SUBURBID = suburbid;
                properties.MARKETTYPEID = markettypeid;
                properties.PROPERTYTYPEID = propertytypeid;
                properties.PROPERTYADDRESS = address;
                properties.MINTERMID = minterm / 6;
                properties.MAXTERMID = maxterm / 6;
                properties.PROPERTYRATEANDTAX = ratesandtax;
                properties.BUILDINGCONDITIONID = condition;
                properties.PROPERTYAVAILABLEDATE = availabledate;
                properties.PROPERTYSTATUSID = statusID;
                //properties.LISTINGPICTUREs.Add(new LISTINGPICTURE { LISTINGPICTUREIMAGE = "C", PROPERTYID = id });

                //Save DB changes
                db.SaveChanges();

                //Find all records for zoning    --- TEST THIS 
                var zoning = db.ZONINGs.Where(x => x.PROPERTYID == id);

                //Delete zoning records
                foreach (var item in zoning)
                {
                    db.ZONINGs.Remove(item);
                }

                db.ZONINGs.Add(new ZONING
                {
                    PROPERTYID = id,
                    ZONINGMUNICIPALVALUATION = municipalvaluation,
                    ZONINGRATINGPERIOD = period,
                    ZONINGUSAGECATEGORY = usagecategory,
                    ZONINGYEAROFVALUATION = yearofvaluation,
                    ZONINGESTIMATEDMONTHLYRATES = monthlyrates,
                    ZONINGUSAGE = zoningusage
                });

                //Save DB changes
                db.SaveChanges();

                //Find all associative records for employee properties    --- TEST THIS UPDATE FOR EMPLOYEE TO PROPERTY
                var employee = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == id);

                //Delete employee properties records
                foreach (var item in employee)
                {
                    db.EMPLOYEEPROPERTies.Remove(item);
                }

                //Add updated employee properties 
                db.EMPLOYEEPROPERTies.Add(new EMPLOYEEPROPERTY
                {
                    PROPERTYID = id,
                    USERID = agentid
                });

                //Save DB changes
                db.SaveChanges();

                //Add the updated price to the property without deleting the old price
                db.PRICEs.Add(new PRICE
                {
                    PROPERTYID = id,
                    PRICEAMOUNT = price,
                    PRICEDATE = DateTime.Now //Set the price datetime to the current date and time
                });

                //Save DB changes
                db.SaveChanges();

                //Assign property details to arrays
                Newtonsoft.Json.Linq.JArray otherBuildingDetails = propertydetails[0];
                Newtonsoft.Json.Linq.JArray features = propertydetails[1];
                Newtonsoft.Json.Linq.JArray bedAndBathQuantities = propertydetails[2];

                //Find all associative records for other building details
                var otherbuildingdetails = db.PROPERTYOTHERBUILDINGDETAILs.Where(x => x.PROPERTYID == id);

                //Delete other building details records
                foreach (var item in otherbuildingdetails)
                {
                    db.PROPERTYOTHERBUILDINGDETAILs.Remove(item);
                }

                //Add updated other building details to the property 
                foreach (dynamic otherBuildingDetail in otherBuildingDetails)
                {
                    db.PROPERTYOTHERBUILDINGDETAILs.Add(new PROPERTYOTHERBUILDINGDETAIL
                    {
                        PROPERTYID = id,
                        OTHERBUILDINGDETAILID = otherBuildingDetail.OTHERBUILDINGDETAILID
                    });
                }

                //Save DB changes
                db.SaveChanges();

                //Find all associative records for features
                var featuresToDelete = db.PROPERTYFEATUREs.Where(x => x.PROPERTYID == id);

                //Delete features records
                foreach (var item in featuresToDelete)
                {
                    db.PROPERTYFEATUREs.Remove(item);
                }

                //Add updated features to the property 
                for (int i = 0; i < features.Count; i++)
                {
                    db.PROPERTYFEATUREs.Add(new PROPERTYFEATURE
                    {
                        PROPERTYID = id,
                        FEATUREID = (int?)features[i][0],
                        PROPERTYFEATUREQUANTITY = (int)features[i][1]
                    });
                }

                //Save DB changes
                db.SaveChanges();

                //Find all associative records for spaces
                var spaces = db.PROPERTYSPACEs.Where(x => x.PROPERTYID == id);

                //Delete spaces records
                foreach (var item in spaces)
                {
                    db.PROPERTYSPACEs.Remove(item);
                }

                //Add updated Bedroom quantity
                if (bedAndBathQuantities[0] != null)
                {
                    db.PROPERTYSPACEs.Add(new PROPERTYSPACE
                    {
                        PROPERTYID = id,
                        SPACEID = 1,
                        PROPERTYSPACEQUANTITY = (int)bedAndBathQuantities[0] //Index zero for bedroom quantity
                    });
                }


                //Save DB changes
                db.SaveChanges();

                // Add updated Bathroom quantity
                if (bedAndBathQuantities[1] != null)
                {
                    db.PROPERTYSPACEs.Add(new PROPERTYSPACE
                    {
                        PROPERTYID = id,
                        SPACEID = 3,
                        PROPERTYSPACEQUANTITY = (int)bedAndBathQuantities[1] //Index zero for bathroom quantity
                    });
                }


                //Save DB changes
                db.SaveChanges();

                //Add updated mandate
                //db.MANDATEs.Add(new MANDATE
                //{
                //    MANDATEDATE = mandatedate,
                //    MANDATETYPEID = mandatetypeid,
                //    MANDATEDOCUMENT = DocumentController.UploadFile(DocumentController.Containers.mandateDocumentsContainer, mandateDocument)
                //});

                //Save DB changes
                db.SaveChanges();

                //Get newly added mandate
                int lastmandateid = db.MANDATEs.Max(item => item.MANDATEID);

                db.PROPERTYMANDATEs.Add(new PROPERTYMANDATE
                {
                    PROPERTYID = id,
                    MANDATEID = lastmandateid
                });

                //Save DB changes
                db.SaveChanges();

                //Return Ok
                return Ok();
                //}
                //catch (Exception)
                //{
                //    return NotFound();
                //}
            }
            return Unauthorized();
        }


        //DELETE//
        [HttpDelete]
        [Route("api/property")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
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

                    //Find property
                    var property = db.PROPERTies.Include(y => y.RENTALs).Include(z => z.RENTALAPPLICATIONs).Include(yy => yy.PURCHASEOFFERs).Include(zz => zz.SALEs).FirstOrDefault(x => x.PROPERTYID == id);
                    if (property == null)
                        return NotFound();

                    if (property.RENTALs.Count > 0 || property.RENTALAPPLICATIONs.Count > 0 || property.PURCHASEOFFERs.Count > 0 || property.SALEs.Count > 0)
                        return Conflict();

                    /*
                    //Find all associative records for other building details
                    var otherbuildingdetails = db.PROPERTYOTHERBUILDINGDETAILs.Where(x => x.PROPERTYID == id);

                    //Delete other building details records
                    foreach (var item in otherbuildingdetails)
                    {
                        db.PROPERTYOTHERBUILDINGDETAILs.Remove(item);
                    }


                    //Find all associative records for features
                    var features = db.PROPERTYFEATUREs.Where(x => x.PROPERTYID == id);

                    //Delete features records
                    foreach (var item in features)
                    {
                        db.PROPERTYFEATUREs.Remove(item);
                    }


                    //Find all associative records for spaces
                    var spaces = db.PROPERTYSPACEs.Where(x => x.PROPERTYID == id);

                    //Delete spaces records
                    foreach (var item in spaces)
                    {
                        db.PROPERTYSPACEs.Remove(item);
                    }


                    //Find all associative records for spaces
                    var prices = db.PRICEs.Where(x => x.PROPERTYID == id);

                    //Delete spaces records
                    foreach (var item in prices)
                    {
                        db.PRICEs.Remove(item);
                    }

                    //Save DB Changes
                    db.SaveChanges();
                    */
                    //Delete specified property
                    db.PROPERTies.Remove(property);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }
    }
}
