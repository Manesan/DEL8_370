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
using System.Net.Mail;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class AgentPurchaseOffersController : ApiController
    {
        //READ ALL DATA// 
        [HttpGet]
        [Route("api/agentpurchaseoffer")]
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

                    //Get all purchase offers
                    var purchaseoffer = db.PURCHASEOFFERs.Where(x => x.PURCHASEOFFERSTATUSID == 3).Select(x => new {
                        x.PURCHASEOFFERID,
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.OFFERDATE,
                        x.OFFERDESCRIPTION,
                        x.OFFERAMOUNT,
                        x.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSID,
                        x.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSDESCRIPTION
                    }).ToList();

                    if (purchaseoffer == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(purchaseoffer);
                    }
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
        [Route("api/agentpurchaseoffer")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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

                    //Null check 
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified purchase offer
                    var purchaseoffer = db.PURCHASEOFFERs.Where(z => z.PURCHASEOFFERID == id).Select(x => new {
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.OFFERDATE,
                        x.OFFERDESCRIPTION,
                        x.OFFERAMOUNT,
                        x.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSID,
                        x.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSDESCRIPTION
                    }).FirstOrDefault();

                    if (purchaseoffer == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(purchaseoffer);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //Accept/Reject Purchase Offer//
        [HttpPatch]
        [Route("api/agentpurchaseoffer")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromUri] string note, [FromBody] DocumentController.UploadClass saleagreement)
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

                    //Null checks
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(accepted.ToString()))
                        return BadRequest();


                    //DB context
                    var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                var purchaseoffer = db.PURCHASEOFFERs.Include(x => x.SALEs).Include(y => y.CLIENT).Include(z => z.CLIENT.USER).FirstOrDefault(Y => Y.PURCHASEOFFERID == id);
                var propertyid = purchaseoffer.PROPERTYID;
                var user = purchaseoffer.CLIENT.USER;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == propertyid).Select(y => new
                {
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME,
                    y.EMPLOYEE.USER.USEREMAIL,
                    y.EMPLOYEE.USER.USERCONTACTNUMBER
                }).FirstOrDefault();

                //Null checks
                // if (string.IsNullOrEmpty(cityname))
                // return BadRequest();

                //Upload property document
                var documenturi = DocumentController.UploadFile(DocumentController.Containers.saleAgreementDocumentsContainer, saleagreement);

                //Accept specified purchase offer
                if (accepted == true)
                {
                    purchaseoffer.PURCHASEOFFERSTATUSID = 4; //Sets to 'Pending Agent Acceptance'
                    purchaseoffer.OFFERDESCRIPTION = note;

                    db.SALEs.Add(new SALE
                    {
                        PROPERTYID = purchaseoffer.PROPERTYID,
                        SALEDATECONCLUDED = DateTime.Now,
                        SALEAMOUNT = purchaseoffer.OFFERAMOUNT,
                        SALEAGREEMENTDOCUMENT = documenturi,
                        PURCHASEOFFERID = purchaseoffer.PURCHASEOFFERID
                    });

                    string newSubject = "Purchase offer for property #" + propertyid + " accepted. Please accept your sale agreement";
                    var userAddress = new MailAddress(user.USEREMAIL, user.USERNAME + " " + user.USERSURNAME);
                    string mailBody = "Dear " + user.USERNAME + " " + user.USERSURNAME + "<br/><br/>We are pleased to inform you that your purchase offer for property #" + propertyid + " has been accepted. Please login to the Black Gold Properties website to accept your sale agreement.<br/><br/>Kind regards<br/>The Black Gold Properties Team<br/><br/>Your property agent: " + agent.USERNAME + " " + agent.USERSURNAME + "<br/>" + agent.USEREMAIL + "<br/>" + agent.USERCONTACTNUMBER;
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, userAddress, null);


                        //Save DB changes
                        db.SaveChanges();

                        return Ok(mailSent);
                }
                else
                {
                    purchaseoffer.PURCHASEOFFERSTATUSID = 2; //Sets to 'Rejected'
                    purchaseoffer.OFFERDESCRIPTION = note;

                    string newSubject = "Purchase offer for property #" + propertyid + " rejected";
                    var userAddress = new MailAddress(user.USEREMAIL, user.USERNAME + " " + user.USERSURNAME);
                    string mailBody = "Dear " + user.USERNAME + " " + user.USERSURNAME + "<br/><br/>We regret to inform you that your purchase offer for property #" + propertyid + " has been rejected. Please login to the Black Gold Properties website to view further details. For queries, please contact your property agent.<br/><br/>Kind regards<br/>The Black Gold Properties Team<br/><br/>Your property agent: " + agent.USERNAME + " " + agent.USERSURNAME + "<br/>" + agent.USEREMAIL + "<br/>" + agent.USERCONTACTNUMBER;
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, userAddress, null);

                        //Save DB changes
                        db.SaveChanges();

                        return Ok(mailSent);
                }
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