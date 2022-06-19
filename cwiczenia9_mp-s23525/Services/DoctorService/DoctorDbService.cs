using cwiczenia9_mp_s23525.Models;
using cwiczenia9_mp_s23525.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Services.DoctorService
{
    public class DoctorDbService : IDoctorDbService
    {
        private readonly MainDbContext _mainDbContext;
        public DoctorDbService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task AddDoctor(DoctorModel doctorModel)
        {
            var doc = new Doctor()
            {
                FirstName = doctorModel.FirstName,
                LastName = doctorModel.LastName,
                Email = doctorModel.Email
            };

            _mainDbContext.Add(doc);

            await _mainDbContext.SaveChangesAsync();
        }

        public async Task DeleteDoctor(int idDoctor)
        {
            var doctor = new Doctor() { IdDoctor = idDoctor };
  
            _mainDbContext.Attach(doctor); 
            _mainDbContext.Remove(doctor);
 
            await _mainDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoctorModel>> GetDoctors()
        {
            return await _mainDbContext.Doctors.Select(e => new DoctorModel
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email
            }).ToListAsync();
        }
        public async Task<bool> IfDoctorExists(int idDoctor)
        {
            return await _mainDbContext.Doctors.Where(e => e.IdDoctor == idDoctor).AnyAsync();
        }

        public async Task UpdateDoctor(DoctorModel doctorModel, int IdDoctor)
        {
            var editDoctor = await _mainDbContext.Doctors.Where(e => e.IdDoctor == IdDoctor).FirstOrDefaultAsync();

            editDoctor.FirstName = doctorModel.FirstName;
            editDoctor.LastName = doctorModel.LastName;
            editDoctor.Email = doctorModel.Email;
                
            _mainDbContext.Attach(editDoctor);
            _mainDbContext.Update(editDoctor);

            await _mainDbContext.SaveChangesAsync();
        }
    }
}
