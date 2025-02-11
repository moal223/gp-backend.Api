using gp_backend.Core.Models;

namespace gp_backend.EF.MySql.Repositories.Interfaces
{
    public interface IAppointmentRepo
    {
        Task<Appointment> Insert(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAll(string Id);
    }
}
