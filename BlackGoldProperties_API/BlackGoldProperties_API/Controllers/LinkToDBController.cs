using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using Microsoft.Ajax.Utilities;

namespace BlackGoldProperties_API.Controllers
{
    public class LinkToDBController : ApiController
    {
        /* Make any changes to the UserModel class as well */
        public static BlackGoldDBEntities20 db = new BlackGoldDBEntities20();
    }
}
