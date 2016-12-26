using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using DoctorsManagmentSystem.BusinessLogic;
using DoctorsManagmentSystem.Models;
using LinqToExcel;
using Microsoft.Ajax.Utilities;

namespace DoctorsManagmentSystem.Controllers
{
    public class YogacenterController : Controller
    {
        private DoctorsManagementSystemEntities db = new DoctorsManagementSystemEntities();

        // GET: /Yogacenter/ ........
        public ActionResult Index()
        {
            return View(db.Yogacenters.ToList());
        }
        public FileResult DownloadExcel()
        {
            const string path = "/Doc/Users.xlsx";
            return File(path, "application/vnd.ms-excel", "Users.xlsx");
        }
        // GET: /Yogacenter/Details/5
        public ActionResult UploadExcel(Yogacenter users, HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" ||
                    FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    string connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString =
                            string.Format(
                                "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;",
                                pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString =
                            string.Format(
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";",
                                pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();
                    adapter.Fill(ds, "ExcelTable");
                    var sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var yogaceneterSheet = (from a in excelFile.Worksheet<Yogacenter>(sheetName) select a);
                    foreach (var a in yogaceneterSheet)
                    {
                        try
                        {

                            Yogacenter doc = new Yogacenter();
                            doc.ProcessingNumberId = a.ProcessingNumberId;
                            doc.FullName = a.FullName;
                            doc.DateOfBirth = a.DateOfBirth;
                            doc.Gender = a.Gender;
                            doc.Nationality = a.Nationality;
                            doc.State = a.State;
                            doc.City = a.City;
                            doc.PermanantAddress = a.PermanantAddress;
                            doc.PinCode = a.PinCode;
                            doc.PhoneNumber = a.PhoneNumber;
                            doc.Date = a.Date;
                            doc.Email = a.Email;
                            doc.AcademicQualification = a.AcademicQualification;
                            doc.Course = a.Course;
                            doc.HealthStatus = a.HealthStatus;
                            db.Yogacenters.Add(doc);
                            db.SaveChanges();

                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " +
                                                   validationError.ErrorMessage);
                                }
                            }
                        }
                        if (System.IO.File.Exists(pathToExcelFile))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json("Record Insert successful", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return View(db.Yogacenters.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yogacenter yogacenter = db.Yogacenters.Find(id);
            if (yogacenter == null)
            {
                return HttpNotFound();
            }
            return View(yogacenter);
        }

        // GET: /Yogacenter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Yogacenter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ProcessingNumberId,FullName,DateOfBirth,Gender,Nationality,State,City,PermanantAddress,PinCode,PhoneNumber,Date,Email,AcademicQualification,Course,HealthStatus")] Yogacenter yogacenter)
        {
            if (ModelState.IsValid)
            {
                db.Yogacenters.Add(yogacenter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yogacenter);
        }

        // GET: /Yogacenter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yogacenter yogacenter = db.Yogacenters.Find(id);
            if (yogacenter == null)
            {
                return HttpNotFound();
            }
            return View(yogacenter);
        }

        // POST: /Yogacenter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ProcessingNumberId,FullName,DateOfBirth,Gender,Nationality,State,City,PermanantAddress,PinCode,PhoneNumber,Date,Email,AcademicQualification,Course,HealthStatus")] Yogacenter yogacenter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yogacenter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yogacenter);
        }

        // GET: /Yogacenter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yogacenter yogacenter = db.Yogacenters.Find(id);
            if (yogacenter == null)
            {
                return HttpNotFound();
            }
            return View(yogacenter);
        }

        // POST: /Yogacenter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yogacenter yogacenter = db.Yogacenters.Find(id);
            db.Yogacenters.Remove(yogacenter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
