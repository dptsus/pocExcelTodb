using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoctorsManagmentSystem.Interface;
using DoctorsManagmentSystem.Models;

namespace DoctorsManagmentSystem.BusinessLogic
{
    public class PopulateDataToDatabase : IPopulateDataToDatabase
    {
        private Models.DoctorsManagementSystemEntities db;
        private bool Datatype;
        private bool Datatypeof;

        public PopulateDataToDatabase(DoctorsManagementSystemEntities db)
        {
            // TODO: Complete member initialization
            this.db = db;
        }
        public void BatchInsertion(List<Doctor> doctors, string tableTable)
        {
            switch (tableTable.ToLower())
            {
                case "doctor":
                    db.Doctors.AddRange(doctors);
                    break;
                    
            }
            db.SaveChanges();
        }

        internal void BatchInsertion(List<Doctor> inValidatedClinics)
        {
            
            throw new System.NotImplementedException();
        }

        public void BatchInsertion(List<Clinic> clinics, string tableTable)
        {
            switch (tableTable.ToLower())
            {
                case "clinic":
                    db.Clinics.AddRange(clinics);
                    break;

            }
            db.SaveChanges();
        }

        internal void BatchInsertion(List<Clinic> inValidatedDoctors)
        {
            throw new System.NotImplementedException();
        }
    }
}