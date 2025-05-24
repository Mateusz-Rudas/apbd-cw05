
using System;
using System.Collections.Generic;

namespace PrescriptionApp.DTOs
{
    public class PatientResponseDto
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public List<PrescriptionDto> Prescriptions { get; set; }
    }

    public class PrescriptionDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
    }

    public class DoctorDto
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MedicamentDto
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }
}
