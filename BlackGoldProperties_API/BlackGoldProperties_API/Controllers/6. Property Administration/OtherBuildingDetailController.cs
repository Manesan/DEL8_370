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

namespace BlackGoldProperties_API.Controllers._6._Property_Administration
{
    public class OtherBuildingDetailController : ApiController
    { 
        //READ ALL DATA//    
        [HttpGet]
        [Route("api/otherbuildingdetail")]
        public IHttpActionResult Get([FromUri] string token)
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

                    //Get all other building details
                    var otherbuildingdetail = db.OTHERBUILDINGDETAILs.Select(x => new { x.OTHERBUILDINGDETAILID, x.OTHERBUILDINGDETAILDESCRIPTION }).ToList();

                    if (otherbuildingdetail == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(otherbuildingdetail);
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
        [Route("api/otherbuildingdetail")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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

                    //Get specified other building detail
                    var otherbuildingdetail = db.OTHERBUILDINGDETAILs.Where(z => z.OTHERBUILDINGDETAILID == id).Select(x => new { x.OTHERBUILDINGDETAILID, x.OTHERBUILDINGDETAILDESCRIPTION }).FirstOrDefault();

                    if (otherbuildingdetail == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(otherbuildingdetail);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //ADD//
        [HttpPost]
        [Route("api/otherbuildingdetail")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string description)
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

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Add other building detail
                    db.OTHERBUILDINGDETAILs.Add(new OTHERBUILDINGDETAIL
                    {
                        OTHERBUILDINGDETAILDESCRIPTION = Utilities.Trimmer(description)
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
            return Unauthorized();
        }


        //UPDATE//
        [HttpPatch]
        [Route("api/otherbuildingdetail")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string description)
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
                    var otherbuildingdetails = db.OTHERBUILDINGDETAILs.FirstOrDefault(x => x.OTHERBUILDINGDETAILID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Update specified other building detail
                    otherbuildingdetails.OTHERBUILDINGDETAILDESCRIPTION = Utilities.Trimmer(description);

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


        //DELETE//
        [HttpDelete]
        [Route("api/otherbuildingdetail")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
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

                    //Find other building detail
                    var otherbuildingdetail = db.OTHERBUILDINGDETAILs.Include(y => y.PROPERTYOTHERBUILDINGDETAILs).FirstOrDefault(x => x.OTHERBUILDINGDETAILID == id);
                    if (otherbuildingdetail == null)
                        return NotFound();
                    if (otherbuildingdetail.PROPERTYOTHERBUILDINGDETAILs.Count > 0)
                        return Conflict();

                    //Delete specified other building detail
                    db.OTHERBUILDINGDETAILs.Remove(otherbuildingdetail);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }
    }
}
