namespace gp_backend.Api.Dtos.Appointment
{
    public class GetAppointmentDetails
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
