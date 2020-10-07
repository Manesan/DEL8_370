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

namespace BlackGoldProperties_API.Controllers._6._Feature
{
    public class FeatureController : ApiController
    {
        //READ ALL DATA//   ---- DELETE DOESNT WORK
        [HttpGet]
        [Route("api/feature")]
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

                    //Get all features
                    var feature = db.FEATUREs.Select(x => new { x.FEATUREID, x.FEATUREDESCRIPTION }).ToList();

                    if (feature == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(feature);
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
        [Route("api/feature")]
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

                    //Get specified feature
                    var feature = db.FEATUREs.Where(z => z.FEATUREID == id).Select(x => new { x.FEATUREID, x.FEATUREDESCRIPTION }).FirstOrDefault();

                    if (feature == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(feature);
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
        [Route("api/feature")]
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

                    //Add a feature
                    db.FEATUREs.Add(new FEATURE
                    {
                        FEATUREDESCRIPTION = Utilities.Trimmer(description)
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
        [Route("api/feature")]
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
                    var features = db.FEATUREs.FirstOrDefault(x => x.FEATUREID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Update specified feature
                    features.FEATUREDESCRIPTION = Utilities.Trimmer(description);

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
        [Route("api/feature")]
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

                    //Find feature
                    var feature = db.FEATUREs.Include(y => y.PROPERTYFEATUREs).FirstOrDefault(x => x.FEATUREID == id);
                    if (feature == null)
                        return NotFound();
                    if (feature.PROPERTYFEATUREs.Count > 0)
                        return Conflict();

                    //Delete specified feature
                    db.FEATUREs.Remove(feature);

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
