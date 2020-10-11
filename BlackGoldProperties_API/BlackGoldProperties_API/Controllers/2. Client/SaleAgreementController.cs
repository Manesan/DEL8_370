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

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class SaleAgreementController : ApiController
    { 
        //READ ALL DATA// 
        [HttpGet]
        [Route("api/saleagreement")]
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

                //Get client
                var email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                var uid = user.USERID;

                //Get all sales
                var saleagreement = db.SALEs.Where(z => z.PURCHASEOFFER.CLIENT.USERID == uid).Select(x => new {
                    x.SALEID,
                    PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.SALEAGREEMENTDOCUMENT,
                    x.SALEAMOUNT,
                    x.SALEDATECONCLUDED,
                    x.PURCHASEOFFER
                }).ToList();

                if (saleagreement == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(saleagreement);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //READ DATA OF SPECIFIC ID// 
        [HttpGet]
        [Route("api/saleagreement")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
            
            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null check
                if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                    return BadRequest();


                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get specified sale 
                var saleagreement = db.SALEs.Where(z => z.SALEID == id).Select(x => new {
                    x.SALEID,
                    PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.SALEAGREEMENTDOCUMENT,
                    x.SALEAMOUNT,
                    x.SALEDATECONCLUDED,
                    x.PURCHASEOFFER.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSID,
                    x.PURCHASEOFFER.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSDESCRIPTION,
                    x.PURCHASEOFFER.OFFERDESCRIPTION
                }).FirstOrDefault();

                if (saleagreement == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(saleagreement);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //Accept/Reject Sale Agreement//
        [HttpPatch] 
        [Route("api/saleagreement")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromBody] DocumentController.UploadClass signedagreement)
        {            

            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null checks
                if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                    return BadRequest();
                if (string.IsNullOrEmpty(accepted.ToString()))
                    return BadRequest();


                //DB context
                var db = LinkToDBController.db;
                var sale = db.SALEs.Include(x => x.PURCHASEOFFER).Include(y => y.PURCHASEOFFER.CLIENT).Include(z => z.PURCHASEOFFER.CLIENT.USER).FirstOrDefault(xx => xx.SALEID == id);
                var user = sale.PURCHASEOFFER.CLIENT.USER;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == sale.PROPERTYID).Select(y => new
                {
                    y.EMPLOYEE.USER.USEREMAIL,
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME
                }).FirstOrDefault();
                var agentAddress = new MailAddress(agent.USEREMAIL + ", " + agent.USERNAME + " " + agent.USERSURNAME);

                //Update specified rental
                if (accepted == true)
                {
                    //Upload rental agreement
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.saleAgreementDocumentsContainer, signedagreement);

                    sale.PURCHASEOFFER.PURCHASEOFFERSTATUSID = 1; //Sets to 'Approved'
                    sale.SALEAGREEMENTDOCUMENT = documenturi;

                    string newSubject = user.USERNAME + " " + user.USERSURNAME + ": Sale agreement acceptance";
                    string mailBody = user.USERNAME + " " + user.USERSURNAME + " has accepted their sale agreement for property #" + sale.PROPERTYID + ".";
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                    //Save DB changes
                    db.SaveChanges();

                    return Ok(mailSent);
                }
                else
                {
                    db.SALEs.Remove(sale);

                    string newSubject = user.USERNAME + " " + user.USERSURNAME + ": Sale agreement rejection";
                    string mailBody = user.USERNAME + " " + user.USERSURNAME + " has rejected their sale agreement for property #" + sale.PROPERTYID + ".";
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                    //Save DB changes
                    db.SaveChanges();

                    return Ok(mailSent);
                }
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }
    }
}
