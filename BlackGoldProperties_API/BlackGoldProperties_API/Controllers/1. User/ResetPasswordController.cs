using Nexmo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;
//using Twilio.Types;

namespace BlackGoldProperties_API.Controllers._1._User
{
    public class ResetPasswordController : ApiController
    {
        //Find user and generate OTP/
        [HttpPost]
        [Route("api/resetpassword")]
        public IHttpActionResult Post([FromUri] string email)
        {
            //Null checks
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            try
            {

                //DB context
                var db = LinkToDBController.db;
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);

                //Get specified users cellphone number
                string number = user.USERCONTACTNUMBER;

                //Generate random 4-digit OTP
                int OTP = GenerateRandomOTP();

                //Send generated OTP to user
                var client = new Client(creds: new Nexmo.Api.Request.Credentials
                {
                    ApiKey = "bb1593bd", //--Keys are specific to my registered gmail account. Register BGP email when ready to deploy
                    ApiSecret = "nH3fZn7CI1MsIB3p"
                });
                var results = client.SMS.Send(request: new SMS.SMSRequest
                {
                    from = "BGP",
                    to = "27767674103",  //change this to the users number from DB
                    text = "Black Gold Properties OTP: " + OTP
                });

                //Initialize access to Twilio
                //TwilioClient.Init("CACa4d3fb5453d0cd01c3038433eb786c22", "fcea04d8450e6209e03a2a89600b5a66");
                ////Send OTP using Twilio
                //MessageResource.CreateAsync(new PhoneNumber("27767674103"),
                //                             from: new PhoneNumber("15037446462"),
                // body: "Black Gold Properties OTP: " + OTP);

                //Store OTP in User table
                user.USEROTP = OTP;

                //Save DB changes
                db.SaveChanges();

                //Return ok
                return Ok();
            }
            catch (System.Exception e)
            {
                return NotFound();
            }
        }


        //Reset Password/
        [HttpPatch]
        [Route("api/resetpassword")]
        public IHttpActionResult Patch([FromUri] string email, [FromUri] int otp, [FromUri] string password)
        {
            //Null checks
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            if (string.IsNullOrEmpty(password))
                return BadRequest();
            if (string.IsNullOrEmpty(otp.ToString()))
                return BadRequest();


            try
            {
                //DB context
                var db = LinkToDBController.db;
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);

                //Null checks
                if (string.IsNullOrEmpty(email))
                    return BadRequest();

                //Check if OTP is correct
                if (otp == user.USEROTP)
                {
                    user.USERPASSWORD = HomeController.HashPassword(password);
                    user.USEROTP = null; 
                }
                else
                {
                    user.USEROTP = null; //---This must display on front end that they must start the whole process over again if they enterered the wrong OTP 
                    return NotFound();
                }

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

        public int GenerateRandomOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
