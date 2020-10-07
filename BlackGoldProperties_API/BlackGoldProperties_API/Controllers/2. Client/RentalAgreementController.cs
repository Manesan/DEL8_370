﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class RentalAgreementController : ApiController
    {
        //READ ALL DATA//    
        [HttpGet]
        [Route("api/rentalagreement")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get client
                var email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                var uid = user.USERID;

                //Get all rental agreements
                var rentalagreement = db.RENTALs.Where(z => z.CLIENT.USERID == uid).Select(x => new {
                    x.RENTALID,
                    x.RENTALDATESTART,
                    x.RENTALDATEEND,
                    x.RENTALSTATU.RENTALSTATUSID,
                    x.RENTALSTATU.RENTALSTATUSDESCRIPTION,
                    RentalAgreement = x.RENTALDOCUMENTs.Select(y => new {y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
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
        [Route("api/rentalagreement")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
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
                    RentalAgreement = x.RENTALDOCUMENTs.Select(y => new {y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
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
            catch (Exception)
            {
                return NotFound();
            }
        }

        //Accept/Reject Rental Agreement//  
        [HttpPatch]
        [Route("api/rentalagreement")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromBody] DocumentController.UploadClass signedagreement) 
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                var rental = db.RENTALs.FirstOrDefault(x => x.RENTALID == id);
                var rentalapplication = db.RENTALAPPLICATIONs.Include(x => x.PROPERTY).FirstOrDefault(x => x.RENTALID == id);                

                //Null checks                             --Finish this
                //if (string.IsNullOrEmpty(description))
                //    return BadRequest();

                //Update specified rental
                if(accepted == true)   //---- LINK RENTALID TO RENTALAPPLICATION
                {
                    rentalapplication.PROPERTY.PROPERTYSTATUSID = 3; //Sets to 'In Progress'

                    //Upload rental agreement
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.rentalDocumentsContainer, signedagreement);

                    db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                    {   
                        RENTALID = id,
                        RENTALAGREEMENTDOCUMENT = documenturi
                    });

                    if (rental.RENTALSTATUSID == 7) //'Pending Client Extension Acceptance'
                    {
                        rental.RENTALSTATUSID = 6; //Sets to 'Rented Extended'
                    }
                    else if(rental.RENTALSTATUSID == 8) //'Pending Client Renewal Acceptance'
                    {
                        rental.RENTALSTATUSID = 5; //Sets to 'Rented Renewed'
                    }
                    else //Brand new rental agreement
                    {
                        rental.RENTALSTATUSID = 2; //'Rented'                        
                        
                        //Assign an inspection to the property
                        db.INSPECTIONs.Add(new INSPECTION
                        {
                            PROPERTYID = rental.PROPERTY.PROPERTYID,
                            INSPECTIONTYPEID = 1, //Take-on inspection
                            IVSTATUSID = 1, //Outstanding
                            USERID = 19, //Default Inspector Set
                            INSPECTIONDATE = DateTime.Now
                        });
                    }
                }

                else if(accepted == false)
                {
                    db.RENTALs.Remove(rental);
                    rentalapplication.RENTALAPPLICATIONSTATUSID = 6; //Sets to 'Rejected By Client'
                    rentalapplication.PROPERTY.PROPERTYSTATUSID = 1; //Sets to 'Available'
                }

                //Save DB changes
                db.SaveChanges();

                //Return Ok
                return Ok();
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }


        //Extend/Renew Rental Agreement//
        [HttpPost]
        [Route("api/rentalagreement")]

        public IHttpActionResult Post([FromUri] string token, [FromUri] int rentalid/*, [FromUri] int termid*/) //--If its a renewal how do we cater for getting the term? (maybe front end handles this to send back term no matter what)
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                var rental = db.RENTALs.Include(x => x.TERM).FirstOrDefault(x => x.RENTALID == rentalid);


                //Null checks
                //if (string.IsNullOrEmpty(request))
                //return BadRequest();
                if (rental == null)
                    return NotFound();

                //-- do a maths check here to check if its a renewal or extension  -- need term attribute in rental entity to do this


                db.RENTALAPPLICATIONs.Add(new RENTALAPPLICATION   //--TEST THIS
                {
                    RENTALAPPLICATIONDOCUMENT = "",
                    USERID = (int)rental.USERID,
                    PROPERTYID = rental.PROPERTYID,
                    RENTALAPPLICATIONSTATUSID = 4, //Sets status to 'Pending Agent Extension'   ///---- make this depend on check above
                    RENTALAPPLICATIONDATE = DateTime.Now,
                    TERMID = rental.TERMID,
                    //TERMID = termid,
                    RENTALAPPLICATIONSTARTDATE = (DateTime)rental.RENTALDATEEND,  //--test if this works
                    RENTALID = rentalid
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


        //Terminate Rental Agreement//  
        [HttpDelete]
        [Route("api/rentalagreement")]

        public IHttpActionResult Delete([FromUri] string token, [FromUri] int rentalid, [FromUri] bool terminate)
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                var rental = db.RENTALs.Include(x => x.RENTALSTATU).FirstOrDefault(x => x.RENTALID == rentalid);


                //Null checks
                //if (string.IsNullOrEmpty(request))
                //return BadRequest();
                if (rental == null)
                    return NotFound();

                if (terminate == true)
                {
                    rental.RENTALSTATUSID = 3; //Sets to 'Pending Agent Termination'
                }

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
    }
}
