using Microsoft.EntityFrameworkCore;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionApp.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly AppDbContext _context;

        public PrescriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(PrescriptionRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Medicaments == null || !dto.Medicaments.Any())
                throw new ArgumentException("At least one medicament must be provided.");

            if (dto.Medicaments.Count > 10)
                throw new ArgumentException("Maximum 10 medicaments allowed.");

            if (dto.DueDate < dto.Date)
                throw new ArgumentException("DueDate must be after or equal to Date.");

            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            if (doctor == null)
                throw new ArgumentException("Doctor not found.");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.FirstName == dto.Patient.FirstName &&
                                          p.LastName == dto.Patient.LastName &&
                                          p.Birthdate == dto.Patient.Birthdate);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = dto.Patient.FirstName,
                    LastName = dto.Patient.LastName,
                    Birthdate = dto.Patient.Birthdate
                };
                _context.Patients.Add(patient);
            }

            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = dto.DoctorId,
                PrescriptionMedicaments = new List<PrescriptionMedicament>()
            };

            foreach (var m in dto.Medicaments)
            {
                var medicament = await _context.Medicaments.FindAsync(m.MedicamentId);
                if (medicament == null)
                    throw new ArgumentException($"Medicament ID {m.MedicamentId} not found.");

                prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = m.MedicamentId,
                    Dose = m.Dose,
                    Description = m.Description
                });
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<PatientResponseDto> GetPatientDataAsync(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == id);

            if (patient == null) return null;

            return new PatientResponseDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(p => p.DueDate)
                    .Select(pr => new PrescriptionDto
                    {
                        IdPrescription = pr.IdPrescription,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Doctor = new DoctorDto
                        {
                            IdDoctor = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName
                        },
                        Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Description
                        }).ToList()
                    }).ToList()
            };
        }
    }
}

