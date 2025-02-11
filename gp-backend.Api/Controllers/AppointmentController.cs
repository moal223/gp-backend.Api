using gp_backend.Api.Dtos;
using gp_backend.Api.Dtos.Appointment;
using gp_backend.Core.Models;
using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gp_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        public readonly IAppointmentRepo _appointmentRepo;
        public readonly UserManager<ApplicationUser> _userManager;
        public AppointmentController(IAppointmentRepo appointmentRepo, UserManager<ApplicationUser> userManager)
        {
            _appointmentRepo = appointmentRepo;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddAppointmentDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }

            // check if user exists
            var doctor = await _userManager.FindByIdAsync(model.DoctorId);
            if (doctor == null)
                return BadRequest();

            var patient = await _userManager.FindByIdAsync(model.PatientId);
            if (patient == null)
                return BadRequest();


            var appointment = await _appointmentRepo.Insert(new Core.Models.Appointment
            {
                AppointmentDate = DateTime.Parse(model.AppointmentDate),
                Doctor = doctor,
                Patient = patient
            });
            return Ok(new BaseResponse(true, ["Success"], new GetAppointmentDetails
            {
                Id = appointment.Id,
                DoctorName = doctor.FullName,
                AppointmentDate = appointment.AppointmentDate,
                DoctorId = doctor.Id
            }));
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string doctorId)
        {
            if (doctorId == string.Empty)
                return BadRequest();

            var appointments = (await _appointmentRepo.GetAll(doctorId)).ToList();
            List<GetAppointmentDetails> appointmentsDto = [];
            foreach (var appointment in appointments)
            {
                appointmentsDto.Add(new GetAppointmentDetails
                {
                    Id = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    DoctorId = appointment.Doctor.Id,
                    DoctorName = appointment.Doctor.FullName
                }) ;
            }
            return Ok(new BaseResponse(true, ["Sucess"], appointmentsDto));
        }
    }
}
