using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MicwayTechTest.Models;
using MicwayTechTest.Context;
using MicwayTechTest.Log;
using System.Web.Http.ModelBinding;

namespace MicwayTechTest.Controllers
{
    /// <summary>
    /// Drivers Controller
    /// </summary>
    public class DriversController : ApiController
    {
        private DriverContext db = new DriverContext();

        /// <summary>
        /// Get basic information of all drivers
        /// </summary>
        /// <returns>List of the drivers with id, fullName and email</returns>
        public IQueryable<DriverDTO> GetDrivers()
        {
            IQueryable<DriverDTO> drivers = null;

            try
            {
                LogUtil.Info("GetDrivers()");

                //Convert driver in DriverDTO
                drivers = from d in db.Drivers
                          select new DriverDTO()
                          {
                              Id = d.Id,
                              FullName = d.FirstName + " " + d.LastName,
                              Email = d.Email
                          };
            }
            catch (Exception e)
            {
                LogUtil.Error(e);
                throw new Exception(string.Format("The following error has occurred: {0}", e.Message));
            }

            return drivers;
        }

        /// <summary>
        /// Get full details of driver
        /// </summary>
        /// <param name="id">Driver ID</param>
        /// <returns>Full data of a specific driver</returns>
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> GetDriver(int id)
        {
            Driver driver = null;

            try
            {
                LogUtil.Info(string.Format("GetDriver({0})", id));

                driver = await db.Drivers.FindAsync(id);
                if (driver == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(e);
                throw new Exception(string.Format("The following error has occurred: {0}", e.Message));
            }

            return Ok(driver);
        }

        /// <summary>
        /// Update driver details
        /// </summary>
        /// <param name="id">Driver ID</param>
        /// <param name="driver">Driver Details</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDriver(int id, Driver driver)
        {
            try
            {
                string.Format("PutDriver({0}, Driver driver)", id);

                if (!ModelState.IsValid)
                {
                    //collect all the errors
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            LogUtil.Error(error.Exception);
                        }
                    }

                    return BadRequest(ModelState);
                }

                if (id != driver.Id)
                {
                    return BadRequest();
                }
                //Validate the data sent
                ValidateDriverDetails(driver);

                db.Entry(driver).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!DriverExists(id))
                {
                    return NotFound();
                }
                else
                {
                    LogUtil.Error(e);
                    throw new Exception(string.Format("The following error has occurred: {0}", e.Message));
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(e);
                throw new Exception(string.Format("The following error has occurred: {0}", e.Message));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <param name="driver">Driver Details</param>
        /// <returns>data of inserted driver</returns>
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> PostDriver(Driver driver)
        {
            try
            {
                LogUtil.Info("PostDriver(Driver driver)");

                if (!ModelState.IsValid)
                {
                    //collect all the errors
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            LogUtil.Error(error.Exception);
                        }
                    }

                    return BadRequest(ModelState);
                }

                //Validate the data sent
                ValidateDriverDetails(driver);

                db.Drivers.Add(driver);

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                LogUtil.Error(e);
                throw new Exception(string.Format("The following error has occurred: {0}", e.Message));
            }

            return CreatedAtRoute("DefaultApi", new { id = driver.Id }, driver);
        }

        /// <summary>
        /// Delete a driver
        /// </summary>
        /// <param name="id">Driver ID</param>
        /// <returns>deleted driver</returns>
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> DeleteDriver(int id)
        {
            Driver driver = null;

            try
            {
                LogUtil.Info(string.Format("DeleteDriver({0})", id));

                driver = await db.Drivers.FindAsync(id);
                if (driver == null)
                {
                    return NotFound();
                }

                db.Drivers.Remove(driver);
                await db.SaveChangesAsync();


            }
            catch (Exception e)
            {
                LogUtil.Error(e);
                throw new Exception(string.Format("The following error has occurred: {0}", e.Message));
            }

            return Ok(driver);
        }

        /// <summary>
        /// Dispose Method
        /// </summary>
        /// <param name="disposing">boolean if it is disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Verify if driver exist on database
        /// </summary>
        /// <param name="id">Driver ID</param>
        /// <returns>Quantity</returns>
        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Validation of the driver data
        /// </summary>
        /// <param name="driver">driver details</param>
        private void ValidateDriverDetails(Driver driver)
        {
            if(string.IsNullOrEmpty(driver.FirstName) || string.IsNullOrEmpty(driver.LastName) || driver.DateOfBirth == null || string.IsNullOrEmpty(driver.Email))
            {
                throw new Exception("All data must be filled");
            }

            if (driver.FirstName.Count() > 50)
            {
                throw new Exception("First name maximum length is 50");
            }

            if (driver.LastName.Count() > 50)
            {
                throw new Exception("Last name maximum length is 50");
            }

            if (driver.Email.Count() > 100)
            {
                throw new Exception("Email maximum length is 100");
            }

            try
            {
                //test email address
                var tempAddr = new System.Net.Mail.MailAddress(driver.Email);
            }
            catch
            {
                throw new Exception("Invalid e-mail address");
            }
        }
    }
}