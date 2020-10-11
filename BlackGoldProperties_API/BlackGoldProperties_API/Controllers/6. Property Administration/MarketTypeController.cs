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

namespace BlackGoldProperties_API.Controllers._6._Market_type
{
    public class MarketTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/markettype")]
        public IHttpActionResult Get()
        {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all market types
                    var markettype = db.MARKETTYPEs.Select(x => new { x.MARKETTYPEID, x.MARKETTYPEDESCRIPTION }).ToList();

                    if (markettype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(markettype);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/markettype")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified market type
                    var markettype = db.MARKETTYPEs.Where(z => z.MARKETTYPEID == id).Select(x => new { x.MARKETTYPEID, x.MARKETTYPEDESCRIPTION }).FirstOrDefault();

                    if (markettype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(markettype);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //ADD//
        [HttpPost]
        [Route("api/markettype")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string description)
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
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Add a market type
                    db.MARKETTYPEs.Add(new MARKETTYPE
                    {
                        MARKETTYPEDESCRIPTION = description
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


        //UPDATE//
        [HttpPatch]
        [Route("api/markettype")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string description)
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
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    var markettypes = db.MARKETTYPEs.FirstOrDefault(x => x.MARKETTYPEID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Update specified market type
                    markettypes.MARKETTYPEDESCRIPTION = description;

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


        //DELETE//
        [HttpDelete]
        [Route("api/markettype")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
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
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;

                    //Find market type
                    var markettype = db.MARKETTYPEs.Include(y => y.PROPERTies).FirstOrDefault(x => x.MARKETTYPEID == id);
                    if (markettype == null)
                        return NotFound();
                    if (markettype.PROPERTies.Count > 0)
                        return Conflict();

                    //Delete specified market type
                    db.MARKETTYPEs.Remove(markettype);

                    //Save DB Changes
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
