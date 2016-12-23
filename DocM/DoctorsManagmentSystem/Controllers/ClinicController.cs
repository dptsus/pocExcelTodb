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
    //yogesh last updated
    public class ClinicController : Controller
    {

        private DoctorsManagementSystemEntities db = new DoctorsManagementSystemEntities();
        //private object ClinicId;
        public ActionResult Index()
        {
            //Controllers.clinic.StatusMessage = "Uploaded";
            return View(db.Clinics.ToList());
        }
        public FileResult DownloadExcel()
        {
            const string path = "/Doc/Users.xlsx";
            return File(path, "application/vnd.ms-excel", "Users.xlsx");
        }
        //public static object ClinicName { get; private set; }

        //public static object DoctorId { get; set; }

        //public ActionResult UploadExcel()
        //{
        //    throw new NotImplementedException();
        //}
        //yogesh
        public ActionResult UploadExcel(Clinic users, HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            //var data = new List<string>();
            //var validatedclinics = new List<Clinic>();
            //var inValidatedClinics = new List<Clinic>();
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
                    const string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var clininSheet = from a in excelFile.Worksheet<Clinic>(sheetName) select a;
                    foreach (var a in clininSheet)
                    {
                        try
                        {
                            Clinic doc = new Clinic();
                            doc.ClinicId = a.ClinicId;
                            doc.DoctorId = a.DoctorId;
                            doc.ClinicName = a.ClinicName;
                            doc.Address = a.Address;
                            doc.City = a.City;
                            doc.State = a.State;
                            doc.PinCode = a.PinCode;
                            doc.Latitude = a.Latitude;
                            doc.Longitude = a.Longitude;
                            doc.ReceptionistAvaliable = a.ReceptionistAvaliable;
                            doc.LoginNeededForReceptionist = a.LoginNeededForReceptionist;
                            doc.ReceptionistLoginMobileNumber = a.ReceptionistLoginMobileNumber;
                            doc.LandlineNumberOrMobileNumber = a.LandlineNumberOrMobileNumber;
                            doc.EmailId = a.EmailId;
                            doc.MorningStartTime = a.MorningStartTime;
                            doc.MorningEndTime = a.MorningEndTime;
                            doc.EveningStartTime = a.EveningStartTime;
                            doc.EveningStartTime = a.EveningStartTime;
                            doc.Holidays = a.Holidays;
                            doc.ScheduleDeatils = a.ScheduleDeatils;
                            doc.AppointmentMode = a.AppointmentMode;
                            doc.AppointmentSlotTime = a.AppointmentSlotTime;
                            doc.AdditionalServicesInClinic = a.AdditionalServicesInClinic;
                            doc.Fees = a.Fees;
                            doc.FollowUp = a.FollowUp;
                            db.Clinics.Add(doc);
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
                        return Json("success", JsonRequestBehavior.AllowGet);
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
            return View(db.Clinics.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClinicId,DoctorId,ClinicName,Address,City,State,PinCode,Latitude,Longitude,ReceptionistAvaliable,LoginNeededForReceptionist,,Expertise,LandlineNumberOrMobileNumber,EmailId,MorningStartTime,MorningEndTime,EveningStartTime,EveningEndTime,Holidays,ScheduleDeatils,AppointmentMode,AppointmentSlotTime,AdditionalServicesInClinic,Fees,FollowUp,")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                db.Clinics.Add(clinic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clinic);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinics = db.Clinics.Find(id);
            if (clinics == null)
            {
                return HttpNotFound();
            }
            return View(clinics);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit([Bind(Include = "ClinicId,DoctorId,ClinicName,Address,City,State,PinCode,Latitude,Longitude,ReceptionistAvaliable,LoginNeededForReceptionist,,Expertise,LandlineNumberOrMobileNumber,EmailId,MorningStartTime,MorningEndTime,EveningStartTime,EveningEndTime,Holidays,ScheduleDeatils,AppointmentMode,AppointmentSlotTime,AdditionalServicesInClinic,Fees,FollowUp")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clinic);
        }

        // GET: /Doctor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            db.Clinics.Remove(clinic);
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

