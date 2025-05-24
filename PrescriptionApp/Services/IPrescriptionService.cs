
using System.Threading.Tasks;
using PrescriptionApp.DTOs;

namespace PrescriptionApp.Services
{
    public interface IPrescriptionService
    {
        Task AddPrescriptionAsync(PrescriptionRequestDto dto);
        Task<PatientResponseDto> GetPatientDataAsync(int id);
    }
}
