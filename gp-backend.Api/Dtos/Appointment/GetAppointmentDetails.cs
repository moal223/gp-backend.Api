namespace gp_backend.Api.Dtos.Appointment
{
    public class GetAppointmentDetails
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
