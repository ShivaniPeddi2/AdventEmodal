using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _context;

    public AppointmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
    {
        return await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Container)
            .Include(a => a.Terminal)
            .Include(a => a.Driver)
            .Include(a => a.TruckCompany)
            .ToListAsync();
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Container)
            .Include(a => a.Terminal)
            .Include(a => a.Driver)
            .Include(a => a.TruckCompany)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        appointment.CreatedAt = DateTime.UtcNow;
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
    {
        _context.Entry(appointment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
