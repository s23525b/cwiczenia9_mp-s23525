using cwiczenia9_mp_s23525.Services.PrescriptionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Controllers
{
    [Route("api/prescriptions")]
    [ApiController]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionDbService _prescriptiondbService;
        public PrescriptionController(IPrescriptionDbService dbService)
        {
            _prescriptiondbService = dbService;
        }

        [HttpGet]
        [Route("{idPrescription}")]
        public async Task<IActionResult> GetPrescription(int idPrescription)
        {
            if (!await _prescriptiondbService.IfPrescriptionExists(idPrescription)) return NotFound("[ERROR] Prescription doesnt exists");
            var prescriptions = await _prescriptiondbService.GetPrescription(idPrescription);
            return Ok(prescriptions);
        }
    }
}
