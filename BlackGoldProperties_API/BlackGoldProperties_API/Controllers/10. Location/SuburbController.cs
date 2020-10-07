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

namespace BlackGoldProperties_API.Controllers._10._Location._10._Suburb
{
    public class SuburbController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/suburb")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();

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

                        //Get all suburbs
                        var suburb = db.SUBURBs.Select(x => new { x.SUBURBID, x.SUBURBNAME, CITYID = (int?)x.CITY.CITYID, x.CITY.CITYNAME }).ToList();

                        if (suburb == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            return Ok(suburb);
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
        [Route("api/suburb")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (id < 1)
                return BadRequest();

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

                        //Get specified suburb
                        var suburb = db.SUBURBs.Where(z => z.SUBURBID == id).Select(x => new { x.SUBURBID, x.SUBURBNAME, CITYID = (int?)x.CITY.CITYID, x.CITY.CITYNAME }).FirstOrDefault();

                        if (suburb == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            return Ok(suburb);
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
        [Route("api/suburb")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string suburbname, [FromUri] int cityid)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(suburbname))
                return BadRequest();
            if (cityid < 1 || string.IsNullOrEmpty(cityid.ToString()))
                return BadRequest();

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

                        //Add a suburb
                        db.SUBURBs.Add(new SUBURB
                        {
                            SUBURBNAME = suburbname,
                            CITYID = cityid
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
        [Route("api/suburb")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string suburbname, [FromUri] int cityid)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(suburbname))
                return BadRequest();
            if (cityid < 1 || string.IsNullOrEmpty(cityid.ToString()))
                return BadRequest();
            if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

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
                        var suburbs = db.SUBURBs.FirstOrDefault(x => x.SUBURBID == id);

                        //Update specified suburb
                        suburbs.SUBURBNAME = suburbname;
                        suburbs.CITYID = cityid;

                        //Save DB changes
                        db.SaveChanges();

                        //Return ok
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
        [Route("api/suburb")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                return BadRequest();

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

                        //Find suburb
                        var suburb = db.SUBURBs.Include(y => y.PROPERTies).Include(z => z.SUBURBPOINTOFINTERESTs).FirstOrDefault(x => x.SUBURBID == id);
                        if (suburb == null)
                            return NotFound();
                        if (suburb.PROPERTies.Count > 0 || suburb.SUBURBPOINTOFINTERESTs.Count > 0)
                            return Conflict();

                        //Delete specified suburb
                        db.SUBURBs.Remove(suburb);

                        //Save DB Changes
                        db.SaveChanges();

                        //Return ok
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
