using System;
using System.Linq;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Models
{
    public class UserModel
    {
        public USER user;

        public void RefreshGUID(BlackGoldPropertiesDBEntities /*BlackGoldDBEntities20*/ db)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                user.USERGUID = Guid.NewGuid().ToString();
                var time = LinkToDBController.db.USERLOGINTIMEOUTs.Select(x => x.USERLOGINTIMEOUTDESCRIPTION).FirstOrDefault();
                user.USERGUIDEXPIRY = DateTime.Now.AddMinutes(Convert.ToDouble(time));
                var guids = db.USERs.Where(x => x.USERGUID == user.USERGUID).Count();
                if (guids > 0)
                    RefreshGUID(db);
                else
                {
                    var usr = db.USERs.Where(x => x.USERID == user.USERID).FirstOrDefault();
                    db.Entry(usr).CurrentValues.SetValues(user);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}