using gp_backend.Core.Models;
using gp_backend.EF.MySql.Data;
using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MySql.Repositories
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly MySqlDbContext _context;
        public AppointmentRepo(MySqlDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Appointment>> GetAll(string Id)
        {
            if(Id == null)
                return Enumerable.Empty<Appointment>();
            return _context.Appointments.Include(a => a.Patient).Where(a => a.Patient.Id == Id).ToList();
        }

        public async Task<Appointment> Insert(Appointment appointment)
        {
            if (appointment == null)
                return null;
            await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            return appointment;
        }
    }
}
