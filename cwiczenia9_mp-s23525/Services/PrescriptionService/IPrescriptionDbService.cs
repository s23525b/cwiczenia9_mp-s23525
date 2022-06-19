using cwiczenia9_mp_s23525.Models.DTO;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Services.PrescriptionService
{
    public interface IPrescriptionDbService
    {
        Task<PrescriptionModel> GetPrescription(int idPrescription);
        Task<bool> IfPrescriptionExists(int idPrescription);
    }
}
