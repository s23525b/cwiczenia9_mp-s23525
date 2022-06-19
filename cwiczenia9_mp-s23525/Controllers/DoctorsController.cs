using cwiczenia9_mp_s23525.Models.DTO;
using cwiczenia9_mp_s23525.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorDbService _doctordbService;
        public DoctorsController(IDoctorDbService idbService)
        {
            _doctordbService = idbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsList()
        {
            try
            {
                var docs = await _doctordbService.GetDoctors();
                return Ok(docs);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                return NotFound("[ERROR] Doctors does not exists");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorModel doctorModel)
        {
            await _doctordbService.AddDoctor(doctorModel);
            return Ok("Added new doctor...");
        }
        [HttpDelete]
        [Route("{idDoctor}")]
        public async Task<IActionResult> RemoveDoctor(int idDoctor)
        {
            if (!await _doctordbService.IfDoctorExists(idDoctor)) return NotFound("[ERROR] Doctor does not exist");
            await _doctordbService.DeleteDoctor(idDoctor);
            return Ok("Doctor " + idDoctor + " removed...");
        }
        [HttpPut]
        [Route("{idDoctor}")]
        public async Task<IActionResult> UpdateDoctor(DoctorModel doctorModel, int idDoctor)
        {
            if (!await _doctordbService.IfDoctorExists(idDoctor)) return NotFound("[ERROR] Doctor does not exist");
            await _doctordbService.UpdateDoctor(doctorModel, idDoctor);
            return Ok("Doctor " + idDoctor + " updated...");
        }
    }
}
