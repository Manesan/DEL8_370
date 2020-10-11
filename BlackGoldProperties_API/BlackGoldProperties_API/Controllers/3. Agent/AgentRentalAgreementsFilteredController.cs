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
    public class AgentRentalAgreementsFilteredController : ApiController
    {
        //READ ALL RENTAL AGREEMENTS TO TERMINATE//    
        [HttpGet]
        [Route("api/rentalagreementterminations")]
        public IHttpActionResult Get([FromUri] string token)
        { 

            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in 
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all rental applications pending extension
                var rentalagreement = db.RENTALs.Where(z => z.RENTALSTATUSID == 3).Select(x => new {
                    x.RENTALID,
                    x.RENTALDATESTART,
                    x.RENTALDATEEND,
                    x.RENTALSTATU.RENTALSTATUSID,
                    x.RENTALSTATU.RENTALSTATUSDESCRIPTION,
                    RentalAgreementDocument = x.RENTALDOCUMENTs.Select(y => new { y.RENTALID, y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
                    x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.CLIENT.USER.USERID,
                    x.CLIENT.USER.USERNAME,
                    x.CLIENT.USER.USERSURNAME,
                    x.CLIENT.USER.USERCONTACTNUMBER,
                    x.CLIENT.USER.USEREMAIL
                }).ToList();

                if (rentalagreement == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(rentalagreement);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/rentalagreementterminations")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { 
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified rental agreement
                    var rentalagreement = db.RENTALs.Where(z => z.RENTALID == id).Select(x => new {
                        x.RENTALID,
                        x.RENTALDATESTART,
                        x.RENTALDATEEND,
                        x.RENTALSTATU.RENTALSTATUSID,
                        x.RENTALSTATU.RENTALSTATUSDESCRIPTION,
                        RentalAgreementDocument = x.RENTALDOCUMENTs.Select(y => new { y.RENTALID, y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.CLIENT.USER.USERID,
                        x.CLIENT.USER.USERNAME,
                        x.CLIENT.USER.USERSURNAME,
                        x.CLIENT.USER.USERCONTACTNUMBER,
                        x.CLIENT.USER.USEREMAIL
                    }).FirstOrDefault();

                    if (rentalagreement == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(rentalagreement);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }

        //Cancel Rental Agreement//
        [HttpDelete]
        [Route("api/rentalagreementterminations")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
        { 
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;

                    //Find rentalagreement
                    var rentalagreement = db.RENTALs.FirstOrDefault(x => x.RENTALID == id);
                    if (rentalagreement == null)
                        return NotFound();


                    //Delete specified rental agreement
                    rentalagreement.RENTALSTATUSID = 1;
                    rentalagreement.PROPERTY.PROPERTYSTATUSID = 3; //Sets to 'In Progress'

                    //Assign an inspection to the property
                    db.INSPECTIONs.Add(new INSPECTION
                    {
                        PROPERTYID = rentalagreement.PROPERTY.PROPERTYID,
                        INSPECTIONTYPEID = 2, //Outgoing inspection
                        IVSTATUSID = 1, //Outstanding   
                        INSPECTIONDATE = DateTime.Now,
                        USERID = 19 //Set Default Inspector
                    });

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
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
