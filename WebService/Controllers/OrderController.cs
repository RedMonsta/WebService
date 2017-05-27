using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebService.Models;
using System.Data.Entity;
using System.Text;
using System.IO;
using System;

namespace WebService.Controllers
{
    public class OrderController : ApiController
    {
        AtkitchenContext db = new AtkitchenContext();

        //Временно для дебага. Можно сделать для админов
        //[Route("api/order")]
        //public IHttpActionResult Get()
        //{
        //    if (db.Orders.Any()) return Ok(db.Orders.ToList());
        //    else return NotFound();
        //}

        //Можно так же отправлять юзера и проверять админ или нет
        //public IHttpActionResult Get(int id)
        //{
        //    if (db.Orders.Any(x => x.Id == id))
        //    {
        //        return Ok(db.Orders.Find(id));
        //    }
        //    else return NotFound();
        //}

        [Route("api/order/{login}/{password}/{status}")]
        public IHttpActionResult Get(string login, string password, string status)
        {
            List<Order> reslist = new List<Order>();
            Client tmpCli = db.Clients.Find(login);
            if (tmpCli == null)
            {
                return Ok(reslist);
            }
            else
            {
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim())
                {
                    if (tmpCli.Status == 2)
                    {
                        if (db.Orders.Any()) return Ok(db.Orders.ToList());
                        else return NotFound();
                    }
                    else
                    {
                        return Ok(reslist);
                    }
                }
                else
                {
                    return Ok(reslist);
                }
            }
        }

        [Route("api/order/{login}/{password}")]
        public IHttpActionResult Get(string login, string password)
        {
            var orderslist = db.Orders.ToList();
            var userslist = db.Clients.ToList();
            List<Order> reslist = new List<Order>();
            foreach (var item in orderslist)
            {
                if (item.Nickname.Trim() == login.Trim()) //reslist.Add(item);
                {
                    var tmpuser = userslist.Find(x => x.Login == login);
                    if (tmpuser != null)
                    {
                        if (tmpuser.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim()) reslist.Add(item);
                    }
                }
            }
            return Ok(reslist);
        }

        [Route("api/order")]
        //public IHttpActionResult Post(Order ord)
        public IHttpActionResult Post(ClientPlusOrder cltord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = cltord.client;
            var order = cltord.order;

            Client tmpCli = db.Clients.Find(client.Login);
            if (tmpCli != null)
            {
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(client.Password), Authorizer.ServerSHAKey).Trim())
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return Ok(order);
                }
                else return Ok(new Order { Name = "#ErrorClient#"});

            }
            return Ok(new Order { Name = "#ErrorClient#" });
        }

        [Route("api/order")]
        public IHttpActionResult Put(ClientPlusOrder cltord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = cltord.client;
            var order = cltord.order;

            Client tmpCli = db.Clients.Find(client.Login);
            if (tmpCli != null)
            {
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(client.Password), Authorizer.ServerSHAKey).Trim())
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return Ok(order);
                }
                else return Ok(new Order { Name = "#ErrorClient#" });
            }
            return Ok(new Order { Name = "#ErrorClient#" });
        }

        [Route("api/order/{id}/{login}/{password}")]
        public IHttpActionResult Delete(string id, string login, string password)
        //public IHttpActionResult Delete(int id)
        {          
            Client tmpCli = db.Clients.Find(login);
            if (tmpCli != null)
            {
                if (tmpCli.Password.Trim() == Authorizer.GetHashFromStringValue(Authorizer.DecryptStringByBase64(password), Authorizer.ServerSHAKey).Trim())
                {
                    var intid = Convert.ToInt32(id);
                    Order ord = db.Orders.Find(intid);
                    if (ord != null)
                    {
                        db.Orders.Remove(ord);
                        db.SaveChanges();
                        return Ok(ord);
                    }
                    return NotFound();
                }
                else return Ok(new Order { Name = "#ErrorClient#" });
            }
            return Ok(new Order { Name = "#ErrorClient#" });

            
        }
    }
}