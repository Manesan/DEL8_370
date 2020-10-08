﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BlackGoldProperties_API.Models
{
    public static class Utilities
    {
        /// <summary>
        /// Random to be used for random string generator
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Method that takes a raw string and returns the SHA256 value thereof
        /// </summary>
        /// <param name="rawData">Raw string value to be hashed</param>
        /// <returns>SHA256 value of raw string</returns>
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (var sha256Hash = SHA256.Create())
            {
                //ComputeHash - returns byte array  
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                //Convert byte array to a string   
                var builder = new StringBuilder();
                //Loop through each byte
                foreach (var t in bytes)
                    builder.Append(t.ToString("x2"));
                //Return the string
                return builder.ToString();
            }
        }

        /// <summary>
        /// Method to generate random string of specified length
        /// </summary>
        /// <param name="length">Specified length of string</param>
        /// <returns>Random length string</returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Method to generate 4 digit long one time pin
        /// </summary>
        /// <returns></returns>
        public static string GenerateOTP()
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Method to send email to specified address with body
        /// </summary>
        /// <param name="mailBody">Text to be sent</param>
        /// <param name="to">Recipient of the email</param>
        public static bool SendMail(string mailBody, string mailSubject, MailAddress toAddress, MailAddress ccAddress)
        {
            try
            {
                var fromAddress = new MailAddress("u18320997@tuks.co.za", "Black Gold Properties");
                var ePassword1 = "odchhgrbpjqcvmqi";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, ePassword1)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = mailSubject,
                    Body = mailBody
                })
                {
                    if (ccAddress != null)
                        message.CC.Add(ccAddress);
                    string fileName = "~/Content/BGPropEmail.jpg";
                    fileName = HttpContext.Current.Server.MapPath(fileName);
                    //message.Attachments.Add(new Attachment(fileName));
                    message.IsBodyHtml = true;
                    AlternateView imgview = AlternateView.CreateAlternateViewFromString(mailBody + "<br/><br/>" + "<img src=cid:imgpath height=312 width=820>", null, "text/html");
                    LinkedResource lr = new LinkedResource(fileName);
                    lr.ContentId = "imgpath";
                    imgview.LinkedResources.Add(lr);
                    message.AlternateViews.Add(imgview);
                    //message.Body = lr.ContentId;
                    smtp.Send(message);
                }
                return true;
                    

                /*using (var smtpClient = new SmtpClient())
                {
                    var basicCredential = new NetworkCredential("u18320997@tuks.co.za", "odchhgrbpjqcvmqi");
                    var fromAddress = new MailAddress("u18320997@tuks.co.za", "Black Gold Properties");
                    using (var message = new MailMessage(fromAddress, toAddress))
                    {        
                        smtpClient.Host = "mail.gmail.co.za";
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = basicCredential;
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        message.Subject = mailSubject;

                        message.IsBodyHtml = true;

                        string fileName = "~/Content/BGPropEmail.jpg";
                        fileName = HttpContext.Current.Server.MapPath(fileName);

                        AlternateView imgview = AlternateView.CreateAlternateViewFromString(mailBody + "<br/><br/>" + "<img src=cid:imgpath height=150 width=200>", null, "text/html");
                        LinkedResource lr = new LinkedResource(fileName);
                        lr.ContentId = "imgpath";
                        imgview.LinkedResources.Add(lr);
                        message.AlternateViews.Add(imgview);

                        message.Body = lr.ContentId;
                        message.To.Add(fromAddress);

                        smtpClient.Send(message);

                        return true;
                    }
                }*/
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to convert a stream to a Base64 string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ConvertToBase64(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static string Trimmer(string attribute)
        {
            if (attribute != null)
            {
                attribute = attribute.Trim();
            }
            if (attribute == "undefined" || attribute == "null")
            {
                attribute = null;
            }
            return attribute;
        }
    }
}