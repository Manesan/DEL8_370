using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.IO;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers
{
    public class DownloadFilesController : ApiController
    {
        //GET INSPECTION FILE//
        [HttpGet]
        [Route("api/downloadfile")]
        public dynamic Get([FromUri] string token,[FromUri] string documenttype, [FromUri]int id)
        {
            //Null checks
            //if (string.IsNullOrEmpty(token))
            //    return BadRequest();

            //Check valid token, logged in, role
            //if (TokenManager.Validate(token) != true)
            //    return BadRequest(); // Returns as user is invalid
            //if (TokenManager.IsLoggedIn(token) != true)
            //    return BadRequest(); // Returns as user is not logged in
            //if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            //{

            var documentpath = "";
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get file
                    if(documenttype == "Inspection")
                    {
                         documentpath = db.INSPECTIONs.Where(z => z.INSPECTIONID == id).Select(x => x.INSPECTIONDOCUMENT ).FirstOrDefault();
                    }
                    else if(documenttype == "Valuation")
                    {
                         documentpath = db.VALUATIONs.Where(z => z.VALUATIONID == id).Select(x => x.VALUATIONDOCUMENT ).FirstOrDefault();
                    }
                    else if(documenttype == "SaleAgreement")
                    {
                         documentpath = db.SALEs.Where(z => z.SALEID == id).Select(x => x.SALEAGREEMENTDOCUMENT ).FirstOrDefault();
                    }
                    else if(documenttype == "RentalAgreement")
                    {
                         var placeholder = db.RENTALDOCUMENTs.Where(z => z.RENTALID == id).Select(y => new {y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault();
                         documentpath = placeholder.RENTALAGREEMENTDOCUMENT;
                    }
                    else if(documenttype == "RentalApplication")
                    {
                         documentpath = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONID == id).Select(x => x.RENTALAPPLICATIONDOCUMENT).FirstOrDefault();
                    }
                    else if(documenttype == "Mandate")
                    {
                         documentpath = db.MANDATEs.Where(z => z.MANDATEID == id).Select(x => x.MANDATEDOCUMENT).FirstOrDefault();
                    }
                    else if(documenttype == "ListingPicture")
                    {
                        var placeholder = db.LISTINGPICTUREs.Where(z => z.LISTINGPICTUREID == id).Select(y => new { y.LISTINGPICTUREID, y.LISTINGPICTUREIMAGE }).OrderByDescending(y => y.LISTINGPICTUREID).FirstOrDefault();
                        documentpath = placeholder.LISTINGPICTUREIMAGE;
                    }
                    else if(documenttype == "ClientDocument")
                    {
                         documentpath = db.CLIENTDOCUMENTs.Where(z => z.CLIENTDOCUMENTID == id).Select(x => x.CLIENTDOCUMENT1).FirstOrDefault();
                    }                   
                    else if(documenttype == "PropertyDocument")
                    {
                         documentpath = db.PROPERTYDOCUMENTs.Where(z => z.PROPERTYDOCUMENTID == id).Select(x => x.PROPERTYDOCUMENT1).FirstOrDefault();
                    }  

                    Byte[] bytes = File.ReadAllBytes(documentpath);
                    var file64 = Convert.ToBase64String(bytes);

                return file64;
                   
                }
                catch (Exception)
                {
                    return null;
                }
            //}
            //return Unauthorized();
        }


    }
}
