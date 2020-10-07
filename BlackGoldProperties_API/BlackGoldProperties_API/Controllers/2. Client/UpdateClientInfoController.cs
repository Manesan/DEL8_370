using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class UpdateClientInfoController : ApiController
    {


        //UPLOAD CLIENT DOCUMENTS//
        [HttpPost]
        [Route("api/updateclientinfo")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int documenttypeid, [FromBody]DocumentController.UploadClass clientdocument)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (documenttypeid < 1 || string.IsNullOrEmpty(documenttypeid.ToString()))
                return BadRequest();


                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Find user from token
                    var email = TokenManager.ValidateToken(token);
                    var user = db.USERs.Where(x => x.USEREMAIL == email).FirstOrDefault();
                    var currentuser = user.USERID;

                    //Upload property document
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.clientDocumentsContainer, clientdocument);

                    db.CLIENTDOCUMENTs.Add(new CLIENTDOCUMENT
                    {
                        USERID = currentuser,
                        CLIENTDOCUMENT1 = documenturi,
                        CLIENTDOCUMENTTYPEID = documenttypeid,
                        CLIENTDOCUMENTUPLOADDATE = DateTime.Now,
                        CLIENTDOCUMENTUPLOADEXPIRY = DateTime.Now.AddMonths(12)
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


        string email = "";
        int uid = 0;
        //UPDATE//  
        [HttpPatch]
        [Route("api/updateclientinfo")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] string name, [FromUri] string surname, [FromUri] string contactnumber, [FromUri] string altcontactnumber, [FromUri] string address, [FromUri] string idnumber, [FromUri] string passportnumber )
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

                //Find user from token
                email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);

                //Null checks
                if (string.IsNullOrEmpty(name))
                    return BadRequest();
                if (string.IsNullOrEmpty(surname))
                    return BadRequest();
                if (string.IsNullOrEmpty(address))
                    return BadRequest();

                //Update specified user information
                user.USERNAME = name;
                user.USERSURNAME = surname;
                user.USERCONTACTNUMBER = Utilities.Trimmer(contactnumber);
                user.USERALTCONTACTNUMBER = Utilities.Trimmer(altcontactnumber);
                user.USERADDRESS = address;
                user.USERIDNUMBER = Utilities.Trimmer(idnumber);
                user.USERPASSPORTNUMBER = Utilities.Trimmer(passportnumber);

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


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/clientinfo")]
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

                    //Find user from token
                    email = TokenManager.ValidateToken(token);
                    var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                    uid = user.USERID;

                    //Get specified client
                    var client = db.CLIENTs.Where(z => z.USERID == uid).Select(x => new {
                        x.USERID,
                        x.USER.USERNAME,
                        x.USER.USERSURNAME,
                        USERCONTACTNUMBER = x.USER.USERCONTACTNUMBER.Trim(),
                        USERALTCONTACTNUMBER = x.USER.USERALTCONTACTNUMBER.Trim(),
                        x.USER.USEREMAIL,
                        x.USER.USERADDRESS,
                        USERIDNUMBER = x.USER.USERIDNUMBER.Trim(),
                        USERPASSPORTNUMBER = x.USER.USERPASSPORTNUMBER.Trim(),
                        ClientDocuments = x.CLIENTDOCUMENTs.Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).ToList(),
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
                catch (Exception)
                {
                    return NotFound();
                }
        }
    }
}
