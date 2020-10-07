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
    public class AgentPurchaseOffersController : ApiController
    {
        //READ ALL DATA// 
        [HttpGet]
        [Route("api/agentpurchaseoffer")]
        public IHttpActionResult Get([FromUri] string token)
        { /*
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in */
            //if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
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
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //READ DATA OF SPECIFIC ID// 
        [HttpGet]
        [Route("api/agentpurchaseoffer")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { /*
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in */
            //if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
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
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //Accept/Reject Purchase Offer//
        [HttpPatch]
        [Route("api/agentpurchaseoffer")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromUri] string note, [FromBody] DocumentController.UploadClass saleagreement)
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
                var purchaseoffer = db.PURCHASEOFFERs.Include(x => x.SALEs).FirstOrDefault(Y => Y.PURCHASEOFFERID == id);

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
                }
                else
                {
                    purchaseoffer.PURCHASEOFFERSTATUSID = 2; //Sets to 'Rejected'
                    purchaseoffer.OFFERDESCRIPTION = note;
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
            return Unauthorized();
        }
    }
}