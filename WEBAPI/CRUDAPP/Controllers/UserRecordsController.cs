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
using CRUDAPP.Forms;

namespace CRUDAPP.Controllers
{
    // ADD THIS FOR ALL APP CAN ACCESS IT
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    // RoutePrefix for all inside of UserRecordsController class
    [RoutePrefix("api/user")]
    public class UserRecordsController : ApiController
    {
        private UserDatabaseEntities db = new UserDatabaseEntities();

        //////////  ALL ACTION METHODS   ////////////////////

        [HttpGet]
        [Route("getallUsers")]
        public object GetAllUsers()
        {
            var getRocord = db.UserRecords.Where(col => col.IsActive == true).ToList();
            try
            {
                if (getRocord == null)
                {
                    return new { message = "There is empty lists" };
                }
                else
                    return new { db = getRocord };
            }
            catch (Exception e)
            {
                return new { message = e.InnerException};
            }
        }

        ////////////////////////////////////////////////////////

        // Add more record to the database table
        [HttpPost]
        [Route("AddNewUser")]
        public object AddUserRecord(AddUser addUser)
        {
            try { 
            // create a new OBJECT to store new user information
            UserRecord objectRecordtable = new UserRecord();
                objectRecordtable.LastName = addUser.LastName;
                objectRecordtable.FirstName = addUser.FirstName;
                objectRecordtable.Address= addUser.FirstName;
                objectRecordtable.Age = addUser.Age;
                objectRecordtable.height = addUser.height;
                objectRecordtable.IsActive = true;

                // add record to table database
                db.UserRecords.Add(objectRecordtable);
                db.SaveChanges();
                return new {db = objectRecordtable };
            }
            catch (Exception e)
            {
                return new { status = 404, msg = "Error" + e.InnerException };

            }
        }

        ////////////////////////////////////////////////////////

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