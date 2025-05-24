using Xunit;
using PrescriptionApp.Services;
using PrescriptionApp.Models;
using PrescriptionApp.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionApp.Tests.Services
{
    public class PrescriptionServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            
            context.Doctors.Add(new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Doe" });
            context.Medicaments.Add(new Medicament { IdMedicament = 1, Name = "Aspirin" });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task AddPrescriptionAsync_ShouldAdd_WhenValidData()
        {
            var context = GetDbContext();
            var service = new PrescriptionService(context);

            var dto = new PrescriptionRequestDto
            {
                DoctorId = 1,
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                Patient = new PatientDto
                {
                    FirstName = "Anna",
                    LastName = "Nowak",
                    Birthdate = new DateTime(1990, 1, 1)
                },
                Medicaments = new List<PrescriptionMedicamentDto>
                {
                    new PrescriptionMedicamentDto
                    {
                        MedicamentId = 1,
                        Dose = 1,
                        Description = "1 pill daily"
                    }
                }
            };

            await service.AddPrescriptionAsync(dto);

            var patient = await context.Patients.FirstOrDefaultAsync(p => p.FirstName == "Anna");
            Assert.NotNull(patient);

            var prescription = await context.Prescriptions
                .Include(p => p.PrescriptionMedicaments)
                .FirstOrDefaultAsync();
            Assert.NotNull(prescription);
            Assert.Single(prescription.PrescriptionMedicaments);
        }

        [Fact]
        public async Task AddPrescriptionAsync_ShouldThrow_WhenTooManyMedicaments()
        {
            var context = GetDbContext();
            var service = new PrescriptionService(context);

            var medicaments = new List<PrescriptionMedicamentDto>();
            for (int i = 0; i < 11; i++)
            {
                medicaments.Add(new PrescriptionMedicamentDto { MedicamentId = 1, Dose = 1, Description = "Too many" });
            }

            var dto = new PrescriptionRequestDto
            {
                DoctorId = 1,
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(5),
                Patient = new PatientDto
                {
                    FirstName = "Marek",
                    LastName = "Zielinski",
                    Birthdate = new DateTime(1985, 5, 5)
                },
                Medicaments = medicaments
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddPrescriptionAsync(dto));
        }
    }
}
