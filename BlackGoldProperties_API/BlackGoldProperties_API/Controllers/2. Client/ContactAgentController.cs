using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Net.Mail;
using System.Net;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class ContactAgentController : ApiController
    {
        public int oldUserID = 0;
        //Contact agent// - [Potential client]   ---TEST ALL THIS
        [HttpPost]
        [Route("api/contactagent")]
        public IHttpActionResult Post([FromUri] string email, [FromUri] string name, [FromUri] string surname, [FromUri] string subject, [FromUri] string message, [FromUri] string contactnumber)
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Null checks
                if (string.IsNullOrEmpty(email))
                    return BadRequest();
                if (string.IsNullOrEmpty(name))
                    return BadRequest();
                if (string.IsNullOrEmpty(surname))
                    return BadRequest();
                if (string.IsNullOrEmpty(subject))
                    return BadRequest();
                if (string.IsNullOrEmpty(message))
                    return BadRequest();
                if (string.IsNullOrEmpty(contactnumber))
                    return BadRequest();

                //Check if client already exists as a potential client
                var potential = db.POTENTIALCLIENTs.FirstOrDefault(x => x.POTENTIALCLIENTEMAIL == email);

                //Check if client already exists as a registered client
                var oldclient = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);

                if (oldclient != null)
                {
                    oldUserID = oldclient.USERID;
                }

                if (potential != null)
                {
                    //Delete specified potential client
                    db.POTENTIALCLIENTs.Remove(potential);

                    //Save DB Changes
                    db.SaveChanges();
                }

                if (oldUserID != 0)
                {
                    //Add a potential client with existing user id
                    db.POTENTIALCLIENTs.Add(new POTENTIALCLIENT
                    {
                        POTENTIALCLIENTEMAIL = email,
                        POTENTIALCLIENTNAME = name,
                        POTENTIALCLIENTSURNAME = surname,
                        POTENTIALCLIENTSUBJECT = subject,
                        POTENTIALCLIENTMESSAGE = message,
                        POTENTIALCLIENTCONTACTNUMBER = contactnumber,
                        USERID = oldUserID
                    });
                }
                else
                {
                    //Add a potential client 
                    db.POTENTIALCLIENTs.Add(new POTENTIALCLIENT
                    {
                        POTENTIALCLIENTEMAIL = email,
                        POTENTIALCLIENTNAME = name,
                        POTENTIALCLIENTSURNAME = surname,
                        POTENTIALCLIENTSUBJECT = subject,
                        POTENTIALCLIENTMESSAGE = message,
                        POTENTIALCLIENTCONTACTNUMBER = contactnumber
                    });
                }

                //Save DB changes
                db.SaveChanges();

                //Send email, temp
                //string mName = name;
                //string mSurname = surname;
                //string mEmail = email;
                //string mSubject = subject;
                //string mMessage = message;
                //string mContactnumber = contactnumber;
                string newSubject = name + " " + surname + ": " + subject;
                var toAddress = new MailAddress("u18320997@tuks.co.za", "Black Gold Properties");
                string mailBody = "You have recieved a message from " + name + " " + surname + ":" + "<br/><br/>" + message + "<br/><br/>" + contactnumber + "<br/>" + email;
                bool mailSent = Utilities.SendMail(mailBody, newSubject, toAddress, null);

                /*char[] Alphabet1 = "qazxswedcvfrtgbnhyujmkilop".ToCharArray();
                string pw;
                Random random = new Random();
                string password = "";
                for (int i = 0; i < 8; i++)
                {
                    int choose = random.Next(1, 26);
                    password += Alphabet1[choose];
                }*/

                /*var senderEmail1 = new MailAddress("u18320997@tuks.co.za", "Black Gold Properties");
                var receiverEmail1 = new MailAddress("u18320997@tuks.co.za", "Black Gold Properties");
                var ePassword1 = "odchhgrbpjqcvmqi";
                var smtp1 = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail1.Address, ePassword1)
                };
                using (var mess1 = new MailMessage(senderEmail1, receiverEmail1)
                {
                    Subject = mSubject,
                    Body = "You have recieved a message from " + mName + " " + mSurname + ":" + Environment.NewLine + Environment.NewLine + mMessage + Environment.NewLine + Environment.NewLine + mContactnumber + Environment.NewLine + mEmail
                })
                {
                    smtp1.Send(mess1);
                }
                /*
                //Send email, future
                var senderEmail = new MailAddress("info@bgprop.co.za", "Black Gold System");
                var receiverEmail = new MailAddress("info@bgprop.co.za", "Black Gold Properties");
                var ePassword = "JesusFDS3";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 465,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, ePassword)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = subject,
                    Body = "You have recieved a message from " + name + " " + surname + ":" + Environment.NewLine + Environment.NewLine + message + Environment.NewLine + Environment.NewLine + contactnumber + Environment.NewLine + email
                })
                {
                    smtp.Send(mess);
                }*/

                if (mailSent == true)
                    return Ok(mailSent);
                else
                    return BadRequest();
            }


            catch (Exception e)
            {
                return BadRequest();
            }



            //Return Ok
            return Ok();


        }
    }
}
