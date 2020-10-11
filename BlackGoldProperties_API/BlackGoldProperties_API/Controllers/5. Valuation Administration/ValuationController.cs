using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.IO;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers._5._Valuation_Administration
{
    public class ValuationController : ApiController
    {
        //READ ALL DATA//  
        [HttpGet]
        [Route("api/valuation")]
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(4 /*Valuer*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all valuations
                    var valuations = db.VALUATIONs.Select(x => new {
                        x.VALUATIONID,
                        x.VALUATIONDATE,
                        x.VALUATIONAMOUNT,
                        x.VALUATIONDOCUMENT,
                        x.VALUATIONDESCRIPTION,
                        PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        USERID = (int?)x.EMPLOYEE.USER.USERID,
                        x.EMPLOYEE.USER.USERNAME,
                        x.EMPLOYEE.USER.USERSURNAME,
                        IVSTATUSID = (int?)x.IVSTATU.IVSTATUSID,
                        x.IVSTATU.IVSTATUSDESCRIPTION
                    }).ToList();


                    if (valuations == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(valuations);
                    }
            }
                return Unauthorized();
            }
                catch (Exception)
            {
                return NotFound();
            }
        }

        //READ IVSTATUSES//
        [HttpPut]
        [Route("api/ivstatus")]
        public IHttpActionResult Put([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all employees
                    var ivstatuses = db.IVSTATUS.Select(x => new {
                        x.IVSTATUSID,
                        x.IVSTATUSDESCRIPTION
                    }).ToList();

                    if (ivstatuses == null)
                        return BadRequest();
                    else
                        return Ok(ivstatuses);
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/valuation")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(4 /*Valuer*/))
                {
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified valuation
                    var valuation = db.VALUATIONs.Where(z => z.VALUATIONID == id).Select(x => new {
                        x.VALUATIONID,
                        x.VALUATIONDATE,
                        x.VALUATIONAMOUNT,
                        x.VALUATIONDOCUMENT,
                        x.VALUATIONDESCRIPTION,
                        PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        USERID = (int?)x.EMPLOYEE.USER.USERID,
                        x.EMPLOYEE.USER.USERNAME,
                        x.EMPLOYEE.USER.USERSURNAME,
                        IVSTATUSID = (int?)x.IVSTATU.IVSTATUSID,
                        x.IVSTATU.IVSTATUSDESCRIPTION
                    }).FirstOrDefault();

                    if (valuation == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(valuation);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //CAPTURE VALUATION//
        [HttpPatch]
        [Route("api/valuation")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] DateTime date, [FromUri] string description, [FromUri] int userid, [FromUri] int IVid, [FromBody] DocumentController.UploadClass document)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(4 /*Valuer*/))
                {
                    //Null checks
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(date.ToString()))
                        return BadRequest();
                    if (userid < 1 || string.IsNullOrEmpty(userid.ToString()))
                        return BadRequest();
                    if (IVid < 1 || string.IsNullOrEmpty(IVid.ToString()))
                        return BadRequest();


                    //DB context
                    var db = LinkToDBController.db;
                    var valuation = db.VALUATIONs.FirstOrDefault(x => x.VALUATIONID == id);

                    //Upload valuation document
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.valuationDocumentsContainer, document);

                    //Capture valuation 
                    valuation.VALUATIONDESCRIPTION = Utilities.Trimmer(description);
                    valuation.VALUATIONDATE = date;
                    valuation.USERID = userid;
                    valuation.IVSTATUSID = IVid;
                    valuation.VALUATIONDOCUMENT = documenturi;

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                return Unauthorized();
                }
                catch (System.Exception)
                {
                    return NotFound();
                }
        }
    }
}
