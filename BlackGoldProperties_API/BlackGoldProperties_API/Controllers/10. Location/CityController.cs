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

namespace BlackGoldProperties_API.Controllers._10._Location._10._City
{
    public class CityController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/city")]
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

                    //Get all cities
                    var city = db.CITies.Select(x => new
                    {
                        x.CITYID,
                        x.CITYNAME,
                        PROVINCEID = (int?)x.PROVINCE.PROVINCEID,
                        x.PROVINCE.PROVINCENAME
                    }).ToList();

                    if (city == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(city);
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
        [Route("api/city")]
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

                        //Get specified city
                        var city = db.CITies.Where(z => z.CITYID == id).Select(x => new { x.CITYID, x.CITYNAME, PROVINCEID = (int?)x.PROVINCE.PROVINCEID, x.PROVINCE.PROVINCENAME }).FirstOrDefault();

                        if (city == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            return Ok(city);
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
        [Route("api/city")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string cityname, [FromUri] int provinceid)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(cityname))
                return BadRequest();
            if (provinceid < 1 || string.IsNullOrEmpty(provinceid.ToString()))
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

                        var check = db.CITies.Where(x => x.CITYNAME == cityname).Select(x => x.CITYNAME).FirstOrDefault();

                        if (check != null)
                        {
                            return BadRequest();
                        }

                    //Add a city
                    db.CITies.Add(new CITY
                        {
                            CITYNAME = cityname,
                            PROVINCEID = provinceid
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
        [Route("api/city")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string cityname, [FromUri] int provinceid)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(cityname))
                return BadRequest();
            if (provinceid < 1 || string.IsNullOrEmpty(provinceid.ToString()))
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
                        var cities = db.CITies.FirstOrDefault(x => x.CITYID == id);

                        //Update specified city
                        cities.CITYNAME = cityname;
                        cities.PROVINCEID = provinceid;

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
        [Route("api/city")]
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

                        //Find city
                        var city = db.CITies.Include(y => y.SUBURBs).FirstOrDefault(x => x.CITYID == id);
                        if (city == null)
                            return NotFound();
                        if (city.SUBURBs.Count > 0)
                            return Conflict();

                        //Delete specified city
                        db.CITies.Remove(city);

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