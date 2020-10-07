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

namespace BlackGoldProperties_API.Controllers._8._Employee_Administration
{
    public class EmployeeController : ApiController
    {
        //READ ALL DATA//     -- Fix DELETE
        [HttpGet]
        [Route("api/employee")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all employees
                    var employee = db.EMPLOYEEs.Select(x => new {
                        x.USER.USERID,
                        x.USER.USERNAME,
                        x.USER.USERSURNAME,
                        x.USER.USERCONTACTNUMBER,
                        x.USER.USERALTCONTACTNUMBER,
                        x.USER.USEREMAIL,
                        //x.USER.USERIDORPASSPORTNUMBER,
                        //x.USER.USERADDRESS,
                        //x.EMPLOYEEBANKINGDETAILS,
                        EmployeeType = x.EMPLOYEEROLEs.Select(y => new { y.EMPLOYEETYPE.EMPLOYEETYPEID, y.EMPLOYEETYPE.EMPLOYEETYPEDESCRIPTION }).ToList(),
                        //x.EMPLOYEEDATEEMPLOYED,
                        //x.EMPLOYEERENUMERATON
                    }).ToList();

                    if (employee == null)
                        return BadRequest();
                    else
                        return Ok(employee);
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
        [Route("api/employee")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified employee
                    var employee = db.EMPLOYEEs.Where(z => z.USERID == id).Select(x => new {
                        x.USER.USERID,
                        x.USER.USERNAME,
                        x.USER.USERSURNAME,
                        USERCONTACTNUMBER = x.USER.USERCONTACTNUMBER.Trim(),
                        USERALTCONTACTNUMBER = x.USER.USERALTCONTACTNUMBER.Trim(),
                        x.USER.USEREMAIL,
                        USERIDNUMBER = x.USER.USERIDNUMBER.Trim(),
                        USERPASSPORTNUMBER = x.USER.USERPASSPORTNUMBER.Trim(),
                        x.USER.USERADDRESS,
                        x.USER.USERPASSWORD,
                        x.EMPLOYEEBANKINGDETAILS,
                        EmployeeType = x.EMPLOYEEROLEs.Select(y => new { y.EMPLOYEETYPEID, y.EMPLOYEETYPE.EMPLOYEETYPEDESCRIPTION }).ToList(),
                        x.EMPLOYEEDATEEMPLOYED,
                        x.EMPLOYEERENUMERATON
                    }).FirstOrDefault();

                    if (employee == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(employee);
                    }
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }

        //ADD//   
        [HttpPost]
        [Route("api/employee")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string name, [FromUri] string surname, [FromUri] string contactnumber, [FromUri] string altcontactnumber, [FromUri] string email, [FromUri] string idnumber, [FromUri] string passportnumber, [FromUri] string address, [FromUri] string password, [FromUri] string banking, [FromBody] dynamic employeeroles, [FromUri] DateTime dateemployed, [FromUri] decimal remuneration)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Null checks
                    //if (string.IsNullOrEmpty(email))
                    //    return BadRequest();
                    //if (string.IsNullOrEmpty(name))
                    //    return BadRequest();
                    //if (string.IsNullOrEmpty(surname))
                    //    return BadRequest();
                    //if (string.IsNullOrEmpty(address))
                    //    return BadRequest();
                    //if (string.IsNullOrEmpty(password))
                    //    return BadRequest();


                    var check = db.EMPLOYEEs.Where(x => x.USER.USEREMAIL == email).Select(x => x.USER.USEREMAIL).FirstOrDefault();

                    if(check != null)
                    {
                        return null;
                    }


                    //Add an employee 
                    db.USERs.Add(new USER
                    {
                        USEREMAIL = email.ToLower(),
                        USERPASSWORD = HomeController.HashPassword(password),
                        USERNAME = name,
                        USERSURNAME = surname,
                        USERCONTACTNUMBER = Utilities.Trimmer(contactnumber),
                        USERALTCONTACTNUMBER = Utilities.Trimmer(altcontactnumber),
                        USERIDNUMBER = Utilities.Trimmer(idnumber),
                        USERPASSPORTNUMBER = Utilities.Trimmer(passportnumber),
                        USERADDRESS = address,

                        USERGUID = HomeController.GUIDActions().USERGUID,
                        USERGUIDEXPIRY = HomeController.GUIDActions().USERGUIDEXPIRY
                    });

                    //Save DB changes
                    db.SaveChanges();

                    ////Find the point of interest that was just added
                    int lastuserid = db.USERs.OrderByDescending(item => item.USERID).FirstOrDefault().USERID;

                    //Find the user id that was just registered
                    //int lastuserid = db.USERs.Max(item => item.USERID);

                    //Link the user profile to employee
                    db.EMPLOYEEs.Add(new EMPLOYEE
                    {
                        USERID = lastuserid,
                        EMPLOYEEBANKINGDETAILS = banking,
                        EMPLOYEEDATEEMPLOYED = dateemployed,
                        EMPLOYEERENUMERATON = remuneration
                    });

                    //Save DB changes
                    db.SaveChanges();

                    foreach (dynamic employeerole in employeeroles)
                    {
                        db.EMPLOYEEROLEs.Add(new EMPLOYEEROLE
                        {
                            USERID = lastuserid,
                            EMPLOYEETYPEID = employeerole.EMPLOYEETYPEID
                        });
                    }


                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //UPDATE//
        [HttpPatch]
        [Route("api/employee")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string name, [FromUri] string surname, [FromUri] string contactnumber, [FromUri] string altcontactnumber, [FromUri] string email, [FromUri] string idnumber, [FromUri] string passportnumber, [FromUri] string address, [FromUri] string banking, [FromBody] dynamic employeeroles, [FromUri] DateTime dateemployed, [FromUri] decimal remuneration)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db; // Missing the config line below?
                    db.Configuration.ProxyCreationEnabled = false;
                    var employee = db.EMPLOYEEs.Include(x => x.USER).FirstOrDefault(y => y.USERID == id);

                    //Null checks   --Finish this
                    //if (string.IsNullOrEmpty(name))
                    //    return BadRequest();

                    //Update specified employee
                    employee.USER.USERID = id;
                    employee.USER.USEREMAIL = email.ToLower();
                    //employee.USER.USERPASSWORD = HomeController.HashPassword(password); --The password is not updated in this component
                    employee.USER.USERNAME = name;
                    employee.USER.USERSURNAME = surname;
                    employee.USER.USERCONTACTNUMBER = Utilities.Trimmer(contactnumber);
                    employee.USER.USERALTCONTACTNUMBER = Utilities.Trimmer(altcontactnumber);
                    employee.USER.USERIDNUMBER = Utilities.Trimmer(idnumber);
                    employee.USER.USERPASSPORTNUMBER = Utilities.Trimmer(passportnumber);
                    employee.USER.USERADDRESS = address;
                    employee.EMPLOYEEBANKINGDETAILS = banking;
                    employee.EMPLOYEEDATEEMPLOYED = dateemployed;
                    employee.EMPLOYEERENUMERATON = remuneration;
                    employee.USER.USERGUID = HomeController.GUIDActions().USERGUID;
                    employee.USER.USERGUIDEXPIRY = HomeController.GUIDActions().USERGUIDEXPIRY;
                    //Updates GUID & GUIDExpiry for user (assumes logged in user is updating themself)

                    try
                    {
                        //Find all associative records for employee roles
                        var roles = db.EMPLOYEEROLEs.Where(x => x.USERID == id).ToList();

                        //Delete employee roles records
                        foreach (var item in roles)
                        {
                            db.EMPLOYEEROLEs.Remove(item);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    //Add updated employee roles to the employee 
                    foreach (dynamic employeerole in employeeroles)
                    {
                        db.EMPLOYEEROLEs.Add(new EMPLOYEEROLE
                        {
                            USERID = id,
                            EMPLOYEETYPEID = employeerole.EMPLOYEETYPEID
                        });
                    }
                    //foreach (EMPLOYEEROLE item in employeeroles)
                    //{
                    //    db.EMPLOYEEROLEs.Add(new EMPLOYEEROLE
                    //    {
                    //        USERID = id,
                    //        EMPLOYEETYPEID = item.EMPLOYEETYPEID
                    //    });
                    //}


                    ////Added
                    //Newtonsoft.Json.Linq.JArray rolesList = employeeroles; // Convert to use Fn Count in for loop

                    ////Delete old employee roles
                    //IQueryable<EMPLOYEEROLE> toBeDeleted = db.EMPLOYEEROLEs.Where(x => x.USERID == id);
                    //if (toBeDeleted != null)
                    //{
                    //    db.EMPLOYEEROLEs.RemoveRange(toBeDeleted);
                    //    db.SaveChanges();
                    //}


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

        //DELETE//    -DELETE ISNT WORKING 
        [HttpDelete]
        [Route("api/employee")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;

                    /*
                    //Find all associative records for employee roles
                    var roles = db.EMPLOYEEROLEs.Where(x => x.USERID == id);

                    //Delete employee roles records
                    foreach (var item in roles)
                    {
                        db.EMPLOYEEROLEs.Remove(item);
                    }

                    //Save DB Changes
                    db.SaveChanges();  */

                    //Find if employee exists and does not have assigned properties or outstanding inspections/valuations
                    var employee = db.EMPLOYEEs.Include(y => y.EMPLOYEEPROPERTies).Include(z => z.INSPECTIONs).Include(yy => yy.VALUATIONs).FirstOrDefault(x => x.USERID == id);
                    if (employee == null)
                        return NotFound();
                    if (employee.EMPLOYEEPROPERTies.Count > 0 || employee.INSPECTIONs.Where(x => x.IVSTATUSID == 1).ToList().Count > 0 || employee.VALUATIONs.Where(x => x.IVSTATUSID == 1).ToList().Count > 0)
                        return Conflict();

                    /*
                    //Delete specified employee
                    db.EMPLOYEEs.Remove(employee);

                    //Save DB Changes
                    db.SaveChanges();  */

                    //Find user
                    var user = db.USERs.FirstOrDefault(x => x.USERID == id);
                    if (user == null)
                        return NotFound();

                    //Delete specified employee
                    db.USERs.Remove(user);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }

        //READ ALL VALUERS//
        [HttpPut]
        [Route("api/employee")]
        public IHttpActionResult Put([FromUri] string token)
        {/*
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in*/
            //if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(6 /*Secretary*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all employees
                    var employee = db.EMPLOYEEs.Select(x => new {
                        x.USER.USERID,
                        x.USER.USERNAME,
                        x.USER.USERSURNAME,
                        EmployeeType = x.EMPLOYEEROLEs.Select(y => y.EMPLOYEETYPEID).ToList()
                    }).Where(z => z.EmployeeType.Contains(4)).ToList();

                    if (employee == null)
                        return BadRequest();
                    else
                        return Ok(employee);
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
