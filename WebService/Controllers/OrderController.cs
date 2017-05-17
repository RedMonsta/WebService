using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebService.Models;
using System.Data.Entity;

namespace WebService.Controllers
{
    public class OrderController : ApiController
    {
        AtkitchenContext db = new AtkitchenContext();

        //public IEnumerable<Order> Get()
        //{
        //    return db.Orders.ToList();
        //}

        public IHttpActionResult Get()
        {
            if (db.Orders.Any()) return Ok(db.Orders.ToList());
            else return NotFound();
        }

        public IHttpActionResult Get(int id)
        {
            if (db.Orders.Any(x => x.Id == id))
            {
                return Ok(db.Orders.Find(id));
            }
            else return NotFound();
        }

        //public Order Get(int id)
        //{
        //    return db.Orders.Find(id);
        //}

        public IHttpActionResult Post(Order ord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Orders.Add(ord);
            db.SaveChanges();
            return Ok(ord);
        }

        public IHttpActionResult Put(Order ord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(ord).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(ord);
        }

        public IHttpActionResult Delete(int id)
        {
            Order ord = db.Orders.Find(id);
            if (ord != null)
            {
                db.Orders.Remove(ord);
                db.SaveChanges();
                return Ok(ord);
            }
            return NotFound();
        }
    }
}