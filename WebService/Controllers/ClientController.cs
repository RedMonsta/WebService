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

        public IHttpActionResult Get()
        {
            if (db.Clients.Any()) return Ok(db.Clients.ToList());
            else return NotFound();
        }

        [Route("api/client/{login}")]
        public IHttpActionResult Get(string login)
        {
            //if (db.Clients.Any(x => x.Login == login))
            {

                Client tmpCli = db.Clients.Find(login);
                using (StreamWriter sw = new StreamWriter(@"D:\Heap\servfile.txt", true, System.Text.Encoding.Default))
                {
                    if (tmpCli != null) sw.WriteLine(tmpCli.Login + " " + tmpCli.Password);
                    else sw.WriteLine("Not Found " + login);
                }

                //if (tmpCli != null) return Ok(tmpCli);
                //else return NotFound();
                //System.Web.Http.Results.Ok
                return Ok(db.Clients.Find(login));
            }
            //else return NotFound();
        }

        public IHttpActionResult Post(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client tmpcli = db.Clients.Find(client.Login);
            if (tmpcli != null)
            {
                client.Login = "__IsExist__";
                client.Password = "__IsExist__";
            }
            else
            {
                db.Clients.Add(client);
                db.SaveChanges();                
            }
            return Ok(client);
        }
    }
}