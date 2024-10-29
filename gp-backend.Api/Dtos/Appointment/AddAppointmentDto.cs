namespace gp_backend.Api.Dtos.Appointment
{
    public class AddAppointmentDto
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string AppointmentDate { get; set; }
    }
}
