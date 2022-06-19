using cwiczenia9_mp_s23525.Models;
using cwiczenia9_mp_s23525.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Services.PrescriptionService
{
    public class PrescriptionDbService : IPrescriptionDbService
    {
        private readonly MainDbContext _mainDbContext;
        public PrescriptionDbService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }
        public async Task<PrescriptionModel> GetPrescription(int idPrescription)
        {
                return await _mainDbContext.Prescriptions.Where(e => e.IdPrescription == idPrescription).
                    Select(e => new PrescriptionModel
                    {
                        PatientsName = e.Patient.FirstName,
                        PatientsLastName = e.Patient.LastName,
                        DoctorsName = e.Doctor.FirstName,
                        DoctorsLastName = e.Doctor.LastName,
                        Meds = e.PrescriptionMedicaments.Select(e => new SomeSortOfMeds { Name = e.Medicament.Name, Description = e.Medicament.Description, Type = e.Medicament.Type }).ToList()
                    }).FirstAsync();
           
        }

        public async Task<bool> IfPrescriptionExists(int idPrescription)
        {
            return await _mainDbContext.Prescriptions.Where(e => e.IdPrescription == idPrescription).AnyAsync();
        }
    }
}
