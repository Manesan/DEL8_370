using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._6._Property_Administration
{
    public class PropertyDocumentController : ApiController
    {
        //READ ALL PROPERTIES//
        [HttpGet]
        [Route("api/propertydocument")]
        public IHttpActionResult Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all properties
                var properties = db.PROPERTies.Select(x => new {
                    x.PROPERTYID,
                    x.PROPERTYADDRESS,
                    Documents = x.PROPERTYDOCUMENTs.Select(y => new { PROPERTYDOCUMENTTYPEID = (int?)y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION, PROPERTYDOCUMENTID = (int?)y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1 }).OrderByDescending(y => y.PROPERTYDOCUMENTTYPEID).ToList()
                }).ToList();

                if (properties == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(properties);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/propertydocument")]
        public IHttpActionResult Get([FromUri] int id)
        {
        
            try
            {
                //Null check
                if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                    return BadRequest();

                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get specified property
                var property = db.PROPERTies.Where(z => z.PROPERTYID == id).Select(x => new {
                    x.PROPERTYID,
                    x.PROPERTYADDRESS,
                    Documents = x.PROPERTYDOCUMENTs.Select(y => new { PROPERTYDOCUMENTTYPEID = (int?)y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEID, y.PROPERTYDOCUMENTTYPE.PROPERTYDOCUMENTTYPEDESCRIPTION, PROPERTYDOCUMENTID = (int?)y.PROPERTYDOCUMENTID, y.PROPERTYDOCUMENT1 }).OrderByDescending(y => y.PROPERTYDOCUMENTTYPEID).ToList()
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





        //UPLOAD PROPERTY DOCUMENTS//
        [HttpPost]
        [Route("api/propertydocument")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int propertyid, [FromUri] int documenttype, [FromBody] DocumentController.UploadClass propertydocument)
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
                    if (propertyid < 1 || string.IsNullOrEmpty(propertyid.ToString()))
                        return BadRequest();
                    if (documenttype < 1 || string.IsNullOrEmpty(documenttype.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    var oldDocument = db.PROPERTYDOCUMENTs.Where(x => x.PROPERTYDOCUMENTTYPEID == documenttype && x.PROPERTYID == propertyid).FirstOrDefault();

                    if(oldDocument!= null)
                    {
                        db.PROPERTYDOCUMENTs.Remove(oldDocument);

                        //Save DB changed
                        db.SaveChanges();
                    }                
                                       
                    //Upload property document
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.propertyDocumentsContainer, propertydocument);

                    //Add property document
                    db.PROPERTYDOCUMENTs.Add(new PROPERTYDOCUMENT
                    {
                        PROPERTYID = propertyid,
                        PROPERTYDOCUMENTTYPEID = documenttype,
                        PROPERTYDOCUMENT1 = documenturi
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
