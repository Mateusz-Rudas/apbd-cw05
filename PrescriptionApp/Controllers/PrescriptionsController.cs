using Microsoft.AspNetCore.Mvc;
using PrescriptionApp.DTOs;
using PrescriptionApp.Services;
using System;
using System.Threading.Tasks;

namespace PrescriptionApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionsController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddPrescriptionAsync(dto);
                return Ok("Prescription added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var result = await _service.GetPatientDataAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}

