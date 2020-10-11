using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class PropertyFileUpdateController : ApiController
    {
        //Upload property mandate// 
        [HttpPost]
        [Route("api/propertyfileupdate")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int propertyid, [FromUri] int mandatetypeid, [FromUri] DateTime mandatedate, [FromBody] DocumentController.UploadClass mandatedocument)
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

                    //Null checks
                    if (propertyid < 1 || string.IsNullOrEmpty(propertyid.ToString()))
                        return BadRequest();
                    if (mandatetypeid < 1 || string.IsNullOrEmpty(mandatetypeid.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(mandatedate.ToString()))
                        return BadRequest();


                    //Upload mandate
                    var fileUri = DocumentController.UploadFile(DocumentController.Containers.mandateDocumentsContainer, mandatedocument);

                db.MANDATEs.Add(new MANDATE
                {
                    MANDATETYPEID = mandatetypeid,
                    MANDATEDATE = mandatedate,
                    MANDATEDOCUMENT = fileUri
                });

                //Save DB changes
                db.SaveChanges();

                //Get newly added mandate
                int lastmandate = db.MANDATEs.Max(item => item.MANDATEID);

                //Assign uploaded mandate to property
                db.PROPERTYMANDATEs.Add(new PROPERTYMANDATE
                {
                    MANDATEID = lastmandate,
                    PROPERTYID = propertyid
                });

                //Save DB changes
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


        //Update Listing Picture//  
        [HttpPatch]
        [Route("api/propertyfileupdate")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int propertyid, [FromBody] DocumentController.UploadClass picture)
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

                    //Null checks
                if (propertyid < 1 || string.IsNullOrEmpty(propertyid.ToString()))
                        return BadRequest();


                //Upload picture
                var fileUri = DocumentController.UploadFile(DocumentController.Containers.listingPicturesContainer, picture);

                db.LISTINGPICTUREs.Add(new LISTINGPICTURE
                {
                    LISTINGPICTUREIMAGE = fileUri,
                    PROPERTYID = propertyid
                });

                //Save DB changes
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
