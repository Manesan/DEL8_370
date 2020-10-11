using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Data.Entity;
using System.Data;

namespace BlackGoldProperties_API.Controllers
{
    public class TimeoutAdjustController : ApiController
    {

        //READ TIMER VALUE//
        [HttpGet]
        [Route("api/timer")]
        public IHttpActionResult Get([FromUri] string token)
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

                    //Get specified time
                    var time = db.USERLOGINTIMEOUTs.Select(x => new { x.USERLOGINTIMEOUTDESCRIPTION, x.USERLOGINTIMEOUTID }).OrderByDescending(y => y.USERLOGINTIMEOUTID).FirstOrDefault();

                    if (time == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(time);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //UPDATE//
        [HttpPatch]
        [Route("api/timer")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int duration)
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
                    if (duration < 10 || string.IsNullOrEmpty(duration.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    //var time = db.USERLOGINTIMEOUTs.FirstOrDefault();

                    //Find user from token
                    var email = TokenManager.ValidateToken(token);
                    var user = db.USERs.Where(x => x.USEREMAIL == email).FirstOrDefault();
                    var userid = user.USERID;

                    //Add new time
                    db.USERLOGINTIMEOUTs.Add(new USERLOGINTIMEOUT
                    {
                        USERLOGINTIMEOUTDESCRIPTION = duration,
                        USERID = userid
                    });

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
