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

        [Route("api/client/{login}/{password}")]
        public IHttpActionResult Get(string login, string password)
        {
            Client tmpCli = db.Clients.Find(login);
            if (tmpCli == null)
            {
                var rescli = new Client { Login = "#NullClient#", Password = "#NullClient#" };
                return Ok(rescli);
            }
            else
            {
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(password, Authorizer.ServerSHAKey).Trim())
                    return Ok(new Client { Login = login, Password = password } );
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
            Client getclt = new Client { Login = client.Login, Password = client.Password };
            Client tmpcli = db.Clients.Find(getclt.Login);
            if (tmpcli != null)
            {
                client.Login = "#ExistNickname#";
                client.Password = "#ExistNickname#";               
            }
            else
            {
                Client tmpuser = new Client { Login = getclt.Login, Password = Authorizer.GetHashFromStringValue(getclt.Password, Authorizer.ServerSHAKey) };
                db.Clients.Add(tmpuser);
                db.SaveChanges();                
            }
            return Ok(client);
        }
    }
}