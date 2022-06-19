using System.Collections.Generic;

namespace cwiczenia9_mp_s23525.Models.DTO
{
    public class PrescriptionModel
    {
        public string PatientsName { get; set; }
        public string PatientsLastName { get; set; }
        public string DoctorsName { get; set; }
        public string DoctorsLastName { get; set; }
        public IEnumerable<SomeSortOfMeds> Meds { get; set; }
    }
}
