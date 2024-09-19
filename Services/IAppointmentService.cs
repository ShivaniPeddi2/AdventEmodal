public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAppointmentsAsync();
    Task<Appointment> GetAppointmentByIdAsync(int id);
    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(int id);
}
