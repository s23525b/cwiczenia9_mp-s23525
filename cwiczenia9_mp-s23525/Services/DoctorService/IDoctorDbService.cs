using cwiczenia9_mp_s23525.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Services.DoctorService
{
    public interface IDoctorDbService
    {
        Task<IEnumerable<DoctorModel>> GetDoctors();
        Task<bool> IfDoctorExists(int idDoctor);
        Task AddDoctor(DoctorModel doctorModel);
        Task DeleteDoctor(int idDoctor);
        Task UpdateDoctor(DoctorModel doctorModel, int idDoctor);
    }
}
