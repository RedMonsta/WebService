using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebService.Models;
using System.Data.Entity;
using System.IO;

namespace WebService.Controllers
{
    public class ClientController : ApiController
    {
        AtkitchenContext db = new AtkitchenContext();

        //Временно для дебага. Можно сделать для админов
        public IHttpActionResult Get()
        {
            if (db.Clients.Any()) return Ok(db.Clients.ToList());
            else return NotFound();
        }

        [Route("api/client/{login}/{password}/{status}")]
        public IHttpActionResult Get(string login, string password, string status)
        {

            Client tmpCli = db.Clients.Find(login);
            if (tmpCli == null)
            {
                var rescli = new Client { Login = "#NullClient#", Password = "#NullClient#" };
                return Ok(rescli);
            }
            else
            {
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim())
                {
                    //return Ok(new Client { Login = login, Password = "#Authorized#" });
                    if (tmpCli.Status == 2)
                    {
                        if (db.Clients.Any()) return Ok(db.Clients.ToList());
                        else return NotFound();
                    }
                    else
                    {
                        var rescli = new Client { Login = "#PermissionDenied#", Password = "#PermissionDenied#" };
                        return Ok(rescli);
                    }
                }
                else
                {
                    var rescli = new Client { Login = "#WrongPassword#", Password = "#WrongPassword#" };
                    return Ok(rescli);
                }
            }
        }

        [Route("api/client/{login}/{password}")]
        public IHttpActionResult Get(string login, string password)
        {

            using (StreamWriter sw = new StreamWriter(@"C:\Users\vambr\Desktop\serv.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(Authorizer.EncryptStringByBase64("admin"));
            }

            Client tmpCli = db.Clients.Find(login);
            if (tmpCli == null)
            {
                var rescli = new Client { Login = "#NullClient#", Password = "#NullClient#" };
                return Ok(rescli);
            }
            else
            {
                //using (StreamWriter sw = new StreamWriter(@"C:\Users\vambr\Desktop\serv2.txt", false, System.Text.Encoding.UTF8))
                //{
                //    //sw.WriteLine(password + "||" + tmpCli.Password.Trim() + "||" + Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim());
                //    //if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim())
                //    //{
                //    //    sw.WriteLine("true");
                //    //}
                //    //else sw.WriteLine("false");
                //    //if (Authorizer.IsEqualTwoSHAHashes(Authorizer.GetHashFromStringValue(tmpCli.Password, Authorizer.ServerSHAKey), Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey)))
                //    //if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue("qwerty", Authorizer.ServerSHAKey).Trim())
                //    //if (Authorizer.IsEqualTwoSHAHashes(tmpCli.Password, Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey)))
                //    //    sw.WriteLine("true");
                //    //else
                //    //    sw.WriteLine("false");
                //    //sw.WriteLine(tmpCli.Password);
                //    //sw.WriteLine(Authorizer.GetHashFromStringValue("qwerty", Authorizer.ServerSHAKey));
                //    //sw.WriteLine(Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey));
                //    //sw.WriteLine(Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(Authorizer.EncryptStringByBase64("qwerty")), Authorizer.ServerSHAKey));
                //    //sw.WriteLine(Authorizer.DecryptStringByBase64(password));
                //    //sw.WriteLine("qwerty");

                //    //sw.WriteLine("");
                //    //if (Authorizer.IsEqualTwoSHAHashes(Authorizer.GetHashFromStringValue("qwerty", Authorizer.ServerSHAKey), Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(Authorizer.EncryptStringByBase64("qwerty")), Authorizer.ServerSHAKey))) sw.WriteLine("true2");
                //    //else sw.WriteLine("false2");
                //}
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim())
                    return Ok(new Client { Login = login, Password = "#Authorized#" } );
                else
                {
                    var rescli = new Client { Login = "#WrongPassword#", Password = "#WrongPassword#" };
                    return Ok(rescli);
                }
            }
        }

        public IHttpActionResult Post(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Client getclt = new Client { Login = client.Login, Password = Authorizer.DecryptStringByBase64(client.Password) };
            //using (StreamWriter sw = new StreamWriter(@"C:\Users\vambr\Desktop\serv1.txt", false, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(getclt.Password + " " + Authorizer.EncryptStringByBase64(getclt.Password) + " " + getclt.Login + "  " + Authorizer.GetHashFromStringValue(getclt.Password, Authorizer.ServerSHAKey));
            //}
            Client tmpcli = db.Clients.Find(getclt.Login);
            if (tmpcli != null)
            {
                client.Login = "#ExistNickname#";
                client.Password = "#ExistNickname#";               
            }
            else
            {
                Client tmpuser = new Client { Login = getclt.Login, Password = Authorizer.GetHashFromStringValue(getclt.Password, Authorizer.ServerSHAKey).Trim(), Status = 1 };
                //Client tmpuser = new Client { Login = getclt.Login, Password = getclt.Password, Status = 1 };
                db.Clients.Add(tmpuser);
                db.SaveChanges();                
            }
            return Ok(client);
        }
    }
}