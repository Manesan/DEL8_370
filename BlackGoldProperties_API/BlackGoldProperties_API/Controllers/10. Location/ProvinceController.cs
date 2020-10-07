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

namespace BlackGoldProperties_API.Controllers._10._Location._10._Province
{
    public class ProvinceController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/province")]
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

                        //Get all provinces
                        var province = db.PROVINCEs.Select(x => new { x.PROVINCEID, x.PROVINCENAME }).ToList();

                        if (province == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            return Ok(province);
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
        [Route("api/province")]
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

                        //Get specified province
                        var province = db.PROVINCEs.Where(z => z.PROVINCEID == id).Select(x => new { x.PROVINCEID, x.PROVINCENAME }).FirstOrDefault();

                        if (province == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            return Ok(province);
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
        [Route("api/province")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string provincename)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(provincename))
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

                        //Add a province
                        db.PROVINCEs.Add(new PROVINCE
                        {
                            PROVINCENAME = provincename
                        });

                        //Save DB changes
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


        //UPDATE//
        [HttpPatch]
        [Route("api/province")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string provincename)
        {
            //Null checks
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            if (string.IsNullOrEmpty(provincename))
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
                        var provinces = db.PROVINCEs.FirstOrDefault(x => x.PROVINCEID == id);

                        //Update specified province
                        provinces.PROVINCENAME = provincename;

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
        [Route("api/province")]
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

                        //Find province
                        var province = db.PROVINCEs.Include(y => y.CITies).FirstOrDefault(x => x.PROVINCEID == id);
                        if (province == null)
                            return NotFound();
                        if (province.CITies.Count > 0)
                            return Conflict();

                        //Delete specified province
                        db.PROVINCEs.Remove(province);

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
