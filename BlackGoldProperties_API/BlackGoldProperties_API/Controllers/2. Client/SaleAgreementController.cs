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

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class SaleAgreementController : ApiController
    { 
        //READ ALL DATA// 
        [HttpGet]
        [Route("api/saleagreement")]
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
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                var sale = db.SALEs.Include(x => x.PURCHASEOFFER).FirstOrDefault(x => x.SALEID == id);

                //Null checks                             --Finish this
                //if (string.IsNullOrEmpty(description))
                //    return BadRequest();

                //Update specified rental
                if (accepted == true)
                {
                    //Upload rental agreement
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.saleAgreementDocumentsContainer, signedagreement);

                    sale.PURCHASEOFFER.PURCHASEOFFERSTATUSID = 1; //Sets to 'Approved'
                    sale.SALEAGREEMENTDOCUMENT = documenturi; 
                }
                else
                {
                    db.SALEs.Remove(sale);
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
    }
}
