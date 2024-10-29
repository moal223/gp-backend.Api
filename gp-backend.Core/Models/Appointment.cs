
namespace gp_backend.Core.Models
{

    /*
        id
        time
        patient
        doctor
    */
    public class Appointment
    {
        public int Id {get; set;}
        public DateTime AppointmentDate {get; set;}
        public ApplicationUser? Doctor {get; set;}
        public ApplicationUser? Patient {get; set;}
    }
}