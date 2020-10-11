using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers._4._Inspection_Administration
{
    public class InspectionController : ApiController
    {
        //READ ALL DATA// 
        [HttpGet]
        [Route("api/inspection")]   
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(3 /*Home Inspector*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all inspections
                    var inspections = db.INSPECTIONs.Select(x => new { 
                        x.INSPECTIONID, 
                        x.INSPECTIONDATE, 
                        x.INSPECTIONDOCUMENT, 
                        x.INSPECTIONCOMMENT,
                        INSPECTIONTYPEID = (int?)x.INSPECTIONTYPE.INSPECTIONTYPEID, 
                        x.INSPECTIONTYPE.INSPECTIONTYPEDESCRIPTION,
                        PROPERTYID = (int?)x.PROPERTY.PROPERTYID, 
                        x.PROPERTY.PROPERTYADDRESS,
                        USERID = (int?)x.EMPLOYEE.USER.USERID, 
                        x.EMPLOYEE.USER.USERNAME, 
                        x.EMPLOYEE.USER.USERSURNAME,
                        IVSTATUSID = (int?)x.IVSTATU.IVSTATUSID, 
                        x.IVSTATU.IVSTATUSDESCRIPTION, 
                        PropertyDefects = x.PROPERTYDEFECTs.Select(y => new { y.DEFECTID, y.DEFECT.DEFECTDESCRIPTION, y.PROPERTYDEFECTQUANTITY }).ToList() 
                    }).ToList();

                    if (inspections == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(inspections);
                    }
            }
                return Unauthorized();
            }
                catch (Exception)
            {
                return NotFound();
            }
        }

        //READ INSPECTIONTYPES//
        [HttpPut]
        [Route("api/inspectiontype")]
        public IHttpActionResult Put([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in*/
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all inspectiontypes
                    var inspectiontype = db.INSPECTIONTYPEs.Select(x => new {
                        x.INSPECTIONTYPEID,
                        x.INSPECTIONTYPEDESCRIPTION
                    }).ToList();

                    if (inspectiontype == null)
                        return BadRequest();
                    else
                        return Ok(inspectiontype);
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
        [Route("api/inspection")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(3 /*Home Inspector*/))
                {
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified inspection
                    var inspection = db.INSPECTIONs.Where(z => z.INSPECTIONID == id).Select(x => new { 
                        x.INSPECTIONID,
                        x.INSPECTIONDATE,
                        x.INSPECTIONDOCUMENT,
                        x.INSPECTIONCOMMENT,
                        INSPECTIONTYPEID = (int?)x.INSPECTIONTYPE.INSPECTIONTYPEID,
                        x.INSPECTIONTYPE.INSPECTIONTYPEDESCRIPTION,
                        PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        USERID = (int?)x.EMPLOYEE.USER.USERID,
                        x.EMPLOYEE.USER.USERNAME,  
                        x.EMPLOYEE.USER.USERSURNAME,
                        IVSTATUSID = (int?)x.IVSTATU.IVSTATUSID,
                        x.IVSTATU.IVSTATUSDESCRIPTION,
                        PropertyDefects = x.PROPERTYDEFECTs.Select(y => new { y.DEFECTID, y.DEFECT.DEFECTDESCRIPTION, y.PROPERTYDEFECTQUANTITY, y.SPACEID, y.SPACE.SPACEDESCRIPTION }).ToList()
                    }).FirstOrDefault();

                    

                    if (inspection == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(inspection);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //CAPTURE INSPECTION//
        [HttpPatch]
        [Route("api/inspection")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] DateTime date, [FromUri] string comment, [FromUri] int typeid, [FromUri] int userid, [FromUri] int IVid,[FromBody] dynamic propertydefect)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/) || TokenManager.GetRoles(token).Contains(3 /*Home Inspector*/))
                {
                    //Null checks
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(date.ToString()))
                        return BadRequest();
                    if (typeid < 1 || string.IsNullOrEmpty(typeid.ToString()))
                        return BadRequest();
                    if (userid < 1 || string.IsNullOrEmpty(userid.ToString()))
                        return BadRequest();


                    //DB context
                    var db = LinkToDBController.db;
                    var inspection = db.INSPECTIONs.Include(y => y.PROPERTY).FirstOrDefault(x => x.INSPECTIONID == id);

                    DocumentController.UploadClass document = new DocumentController.UploadClass();
                    document.FileBase64 = propertydefect[1].FileBase64;
                    document.FileExtension = propertydefect[1].FileExtension;

                    //Upload inspection document
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.inspectionDocumentsContainer, document);

                    //Capture inspection    
                    inspection.INSPECTIONDATE = date;
                    inspection.INSPECTIONCOMMENT = comment;
                    inspection.INSPECTIONTYPEID = typeid;
                    inspection.USERID = userid;
                    inspection.IVSTATUSID = 2;
                    inspection.INSPECTIONDOCUMENT = documenturi;

                    if(inspection.INSPECTIONTYPEID == 2) //'Outgoing Inspection'
                    {
                        inspection.PROPERTY.PROPERTYSTATUSID = 1; //'Available'
                    }

                    if(inspection.INSPECTIONTYPEID == 1)//'Take-on Inspection'
                    {
                        inspection.PROPERTY.PROPERTYSTATUSID = 2; //'Occupied'
                    }

                    //Save DB changes
                    db.SaveChanges();

                    Newtonsoft.Json.Linq.JArray defects = propertydefect[0];

                    //Delete old property defects
                    IQueryable<PROPERTYDEFECT> toBeDeleted = db.PROPERTYDEFECTs.Where(x => x.INSPECTIONID == id);
                    if (toBeDeleted != null)
                    {
                        db.PROPERTYDEFECTs.RemoveRange(toBeDeleted);
                        db.SaveChanges();
                    }
                    

                    foreach (dynamic defect in defects)
                    {
                        db.PROPERTYDEFECTs.Add(new PROPERTYDEFECT
                        {
                            SPACEID = defect[0].SPACEID,
                            DEFECTID = defect[0].DEFECTID,
                            PROPERTYDEFECTQUANTITY = defect[0].PROPERTYDEFECTQUANTITY,
                            INSPECTIONID = id
                        });
                    }


                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                return Unauthorized();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
        }
    }
}
