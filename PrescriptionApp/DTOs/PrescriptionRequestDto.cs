using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionApp.DTOs
{
    public class PrescriptionRequestDto
    {
        [Required]
        public PatientDto Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public List<PrescriptionMedicamentDto> Medicaments { get; set; }
    }

    public class PatientDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }
    }

    public class PrescriptionMedicamentDto
    {
        [Required]
        public int MedicamentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Dose must be positive.")]
        public int Dose { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}

