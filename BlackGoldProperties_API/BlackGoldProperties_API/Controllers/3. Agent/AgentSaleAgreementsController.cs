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
    public class AgentSaleAgreementsController : ApiController
    {
        //READ ALL DATA//    ---Should we put a link between sale and purchase offer? 
        [HttpGet]
        [Route("api/agentsaleagreement")]
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

                    //Get all sales  
                    var saleagreement = db.SALEs.Select(x => new {
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.SALEAGREEMENTDOCUMENT,
                        x.SALEAMOUNT,
                        x.SALEDATECONCLUDED,
                        x.PURCHASEOFFER.CLIENT.USER.USERNAME,
                        x.PURCHASEOFFER.CLIENT.USER.USERSURNAME,
                        x.SALEID
                    }).ToList();

                    if (saleagreement == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(saleagreement);
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
        [Route("api/agentsaleagreement")]
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

                    //Get specified sale 
                    var saleagreement = db.SALEs.Where(z => z.SALEID == id).Select(x => new {
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.SALEAGREEMENTDOCUMENT, 
                        x.SALEAMOUNT,
                        x.SALEDATECONCLUDED,
                        x.PURCHASEOFFER.CLIENT.USER.USERNAME,
                        x.PURCHASEOFFER.CLIENT.USER.USERSURNAME,
                        x.SALEID
                    }).FirstOrDefault();

                    if (saleagreement == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(saleagreement);
                    }
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
