using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebService.Models;
using System.Data.Entity;

namespace WebService.Controllers
{
    public class DishController : ApiController
    {
        AtkitchenContext db = new AtkitchenContext();

        public IHttpActionResult Get()
        {
            return Ok(db.Dishes.ToList());
        }      

        //Можно так же отправлять юзера и проверять админ или нет
        //public IHttpActionResult Get(int id)
        //{
        //    if (db.Dishes.Any(x => x.Id == id))
        //    {
        //        return Ok(db.Dishes.Find(id));
        //    }
        //    else return NotFound();
        //}

        //public IHttpActionResult Post(Dish dish)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    db.Dishes.Add(dish);
        //    db.SaveChanges();
        //    return Ok(dish);
        //}

        //public IHttpActionResult Put(Dish dish)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Entry(dish).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return Ok(dish);
        //}

        //public IHttpActionResult Delete(int id)
        //{
        //    Dish dish = db.Dishes.Find(id);
        //    if (dish != null)
        //    {
        //        db.Dishes.Remove(dish);
        //        db.SaveChanges();
        //        return Ok(dish);
        //    }
        //    return NotFound();
        //}
    }
}