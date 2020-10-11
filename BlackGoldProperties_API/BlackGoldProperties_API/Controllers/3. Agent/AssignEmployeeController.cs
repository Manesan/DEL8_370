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
    public class AssignEmployeeController : ApiController
    {

        //Assign valuer to valuation//
        [HttpPatch]
        [Route("api/assignvaluer")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int valuationid, [FromUri] int userid)
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
                    //Null checks
                    if (valuationid < 1 || string.IsNullOrEmpty(valuationid.ToString()))
                        return BadRequest();
                    if (userid < 1 || string.IsNullOrEmpty(userid.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    var valuation = db.VALUATIONs.FirstOrDefault(x => x.VALUATIONID == valuationid);

                    //Assign valuer
                    valuation.USERID = userid;

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                return Unauthorized();
                }
                catch (System.Exception)
                {
                    return NotFound();
                }
        }


        //Assign inspector to inspecion//
        [HttpPost]
        [Route("api/assigninspector")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int inspectionid, [FromUri] int userid)
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
                    //Null checks
                    if (inspectionid < 1 || string.IsNullOrEmpty(inspectionid.ToString()))
                        return BadRequest();
                    if (userid < 1 || string.IsNullOrEmpty(userid.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    var inspection = db.INSPECTIONs.FirstOrDefault(x => x.INSPECTIONID == inspectionid);

                    //Assign valuer
                    inspection.USERID = userid;

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
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
