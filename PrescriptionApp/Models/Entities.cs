
using System;
using System.Collections.Generic;

namespace PrescriptionApp.Models
{
    public class Patient
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }

    public class Doctor
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }

    public class Medicament
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    }

    public class Prescription
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdPatient { get; set; }
        public Patient Patient { get; set; }
        public int IdDoctor { get; set; }
        public Doctor Doctor { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    }

    public class PrescriptionMedicament
    {
        public int IdMedicament { get; set; }
        public Medicament Medicament { get; set; }
        public int IdPrescription { get; set; }
        public Prescription Prescription { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }
}
