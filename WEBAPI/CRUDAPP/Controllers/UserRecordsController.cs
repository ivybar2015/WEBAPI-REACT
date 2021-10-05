using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CRUDAPP.Models;
using System.Web.Http.Cors;

namespace CRUDAPP.Controllers
{
    // ADD THIS FOR ALL APP CAN ACCESS IT
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    // RoutePrefix for all inside of UsersController class
    [RoutePrefix("api/user")]
    public class UserRecordsController : ApiController
    {
        private UserDatabaseEntities db = new UserDatabaseEntities();

        //////////  ALL ACTION METHODS   ////////////////////

        [HttpGet]
        [Route("getallusers")]
        public object GetAllUser()
        {
            try
            {
                var getRecords = db.UserRecords.Where(col => col.IsActive == true).ToList();
                if (getRecords == null)
                {
                    // No Match
                    return new { status = "401", message = "cannot get all user" };
                }
                else
                {
                    return new { db = getRecords };
                    // User is authenticated                    
                }

            }
            catch (Exception e)
            {
                return new { status = "401", message = "Server Error " + e.InnerException };

            }
        }

 
       
        /// /////////////////////////////////////////////////////
        
        // PUT: api/UserRecords/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserRecord(int id, UserRecord userRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userRecord.Id)
            {
                return BadRequest();
            }

            db.Entry(userRecord).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserRecords
        [ResponseType(typeof(UserRecord))]
        public IHttpActionResult PostUserRecord(UserRecord userRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserRecords.Add(userRecord);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userRecord.Id }, userRecord);
        }

        // DELETE: api/UserRecords/5
        [ResponseType(typeof(UserRecord))]
        public IHttpActionResult DeleteUserRecord(int id)
        {
            UserRecord userRecord = db.UserRecords.Find(id);
            if (userRecord == null)
            {
                return NotFound();
            }

            db.UserRecords.Remove(userRecord);
            db.SaveChanges();

            return Ok(userRecord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRecordExists(int id)
        {
            return db.UserRecords.Count(e => e.Id == id) > 0;
        }
    }
}