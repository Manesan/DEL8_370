using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Net.Mail;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class PurchaseOfferController : ApiController
    {

        //READ ALL DATA// 
        [HttpGet]
        [Route("api/purchaseoffer")]
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

                //Get all purchase offers
                var purchaseoffer = db.PURCHASEOFFERs.Where(z => z.CLIENT.USERID == uid).Select(x => new {
                    x.PURCHASEOFFERID,
                    PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.OFFERDATE,
                    x.OFFERDESCRIPTION,
                    x.OFFERAMOUNT,
                    PURCHASEOFFERSTATUSID = (int?)x.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSID,
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


        //READ DATA OF SPECIFIC ID// 
        [HttpGet]
        [Route("api/purchaseoffer")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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

                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get specified purchase offer
                var purchaseoffer = db.PURCHASEOFFERs.Where(z => z.PURCHASEOFFERID == id).Select(x => new {
                    x.PURCHASEOFFERID,
                    PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.OFFERDATE,
                    x.OFFERDESCRIPTION,
                    x.OFFERAMOUNT,
                    PURCHASEOFFERSTATUSID = (int?)x.PURCHASEOFFERSTATU.PURCHASEOFFERSTATUSID,
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

        //Make a Purchase Offer//
        [HttpPost]
        [Route("api/purchaseoffer")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int propertyid, [FromUri] decimal offeramount, [FromBody] dynamic[] documents)  //-- client documents should contain the many documents that will be seperated here
        {            

            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null checks
                if (propertyid < 1 || string.IsNullOrEmpty(propertyid.ToString()))
                    return BadRequest();
                if (offeramount < 1 || string.IsNullOrEmpty(offeramount.ToString()))
                    return BadRequest();

                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;


                //Get client
                var email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                var uid = user.USERID;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == propertyid).Select(y => new
                {
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME,
                    y.EMPLOYEE.USER.USEREMAIL
                }).FirstOrDefault();

                DocumentController.UploadClass bankstatement = new DocumentController.UploadClass();
                bankstatement.FileBase64 = documents[0].FileBase64;
                bankstatement.FileExtension = documents[0].FileExtension;

                DocumentController.UploadClass copyofid = new DocumentController.UploadClass();
                copyofid.FileBase64 = documents[1].FileBase64;
                copyofid.FileExtension = documents[1].FileExtension;

                //Call upload file function
                var bankstatementUri = DocumentController.UploadFile(DocumentController.Containers.clientDocumentsContainer, bankstatement);
                var copyofidUri = DocumentController.UploadFile(DocumentController.Containers.clientDocumentsContainer, copyofid);

                //Create a purchase offer
                db.PURCHASEOFFERs.Add(new PURCHASEOFFER
                {
                    OFFERAMOUNT = offeramount,
                    OFFERDATE = DateTime.Now,
                    PROPERTYID = propertyid,
                    PURCHASEOFFERSTATUSID = 3, //Sets to 'Pending Agent Acceptance'
                    USERID = uid,
                });

                //Save DB changes
                db.SaveChanges();


                //Upload bank statement
                db.CLIENTDOCUMENTs.Add(new CLIENTDOCUMENT
                {
                    USERID = uid,
                    CLIENTDOCUMENT1 = bankstatementUri,
                    CLIENTDOCUMENTTYPEID = 5,
                    CLIENTDOCUMENTUPLOADDATE = DateTime.Now,
                    CLIENTDOCUMENTUPLOADEXPIRY = DateTime.Now.AddMonths(3)
                });

                //Save DB changes
                db.SaveChanges();

                //Upload copy of ID
                db.CLIENTDOCUMENTs.Add(new CLIENTDOCUMENT
                {
                    USERID = uid,
                    CLIENTDOCUMENT1 = copyofidUri,
                    CLIENTDOCUMENTTYPEID = 1,
                    CLIENTDOCUMENTUPLOADDATE = DateTime.Now,
                    CLIENTDOCUMENTUPLOADEXPIRY = DateTime.Now.AddMonths(12)
                });

                string newSubject = user.USERNAME + " " + user.USERSURNAME + ": New purchase offer for property #" + propertyid;
                var agentAddress = new MailAddress(agent.USEREMAIL, agent.USERNAME + " " + agent.USERSURNAME);
                string mailBody = user.USERNAME + " " + user.USERSURNAME + " has made a new purchase offer for your property, #" + propertyid + ".<br/><br/>" + user.USERCONTACTNUMBER + "<br/>" + user.USEREMAIL;
                bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

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
