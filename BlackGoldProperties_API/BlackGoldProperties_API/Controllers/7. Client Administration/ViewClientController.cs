using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._7._Client_Administration
{
    public class ViewClientController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/viewclient")]
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all clients
                    var clients = db.CLIENTs.Select(x => new { 
                        x.USERID, 
                        x.USER.USERNAME, 
                        x.USER.USERSURNAME, 
                        x.USER.USERCONTACTNUMBER, 
                        x.USER.USEREMAIL, 
                        x.USER.USERADDRESS, 
                        x.CLIENTTYPEID, 
                        x.CLIENTTYPE.CLIENTTYPEDESCRIPTION,
                        idcopy = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 1).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        passportcopy = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 2).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        proofofresidence = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 3).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        creditcheckreport = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 4).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        bankstatement = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 5).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault()
                    }).ToList();

                    if (clients == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(clients);
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
        [Route("api/viewclient")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/))
                {
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified client
                    var client = db.CLIENTs.Where(z => z.USERID == id).Select(x => new {
                        x.USERID,
                        x.USER.USERNAME,
                        x.USER.USERSURNAME,
                        x.USER.USERCONTACTNUMBER,
                        x.USER.USEREMAIL,
                        x.USER.USERADDRESS,
                        x.USER.USERIDNUMBER,
                        x.USER.USERPASSPORTNUMBER,
                        x.CLIENTTYPEID,
                        x.CLIENTTYPE.CLIENTTYPEDESCRIPTION,
                        idcopy = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 1).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        passportcopy = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 2).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        proofofresidence = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 3).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        creditcheckreport = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 4).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault(),
                        bankstatement = x.CLIENTDOCUMENTs.Where(y => y.CLIENTDOCUMENTTYPEID == 5).Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).FirstOrDefault()
                    }).ToList();

                    if (client == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(client);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //UPLOAD CREDIT CHECK REPORT//
        [HttpPost]
        [Route("api/viewclient")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int clientid, [FromBody] DocumentController.UploadClass creditcheckreport)
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
                    if (clientid < 1 || string.IsNullOrEmpty(clientid.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Upload credit check report document
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.clientDocumentsContainer, creditcheckreport);

                    //Add credit check report document
                    db.CLIENTDOCUMENTs.Add(new CLIENTDOCUMENT
                    {
                        CLIENTDOCUMENTTYPEID = 4, //Sets to 'Credit Check Report'
                        CLIENTDOCUMENT1 = documenturi,
                        USERID = clientid,
                        CLIENTDOCUMENTUPLOADDATE = DateTime.Now,
                        CLIENTDOCUMENTUPLOADEXPIRY = DateTime.Now.AddMonths(12)
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
