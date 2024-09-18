using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments()
{
    try
    {
        var query = _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Container)
            .Include(a => a.Terminal)
            .Include(a => a.Driver)
            .Include(a => a.TruckCompany)
            .ToQueryString(); // Logs the SQL query

        Console.WriteLine(query);

        var appointments = await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Container)
            .Include(a => a.Terminal)
            .Include(a => a.Driver)
            .Include(a => a.TruckCompany)
            .ToListAsync();
        
        return Ok(appointments);
    }
    catch (Exception ex)
    {
        // Log the exception or handle it appropriately
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(int id)
    {
        // Retrieve a specific appointment by ID
        var appointment = await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Container)
            .Include(a => a.Terminal)
            .Include(a => a.Driver)
            .Include(a => a.TruckCompany)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Set the creation and update timestamps
        appointment.CreatedAt = DateTime.UtcNow;

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment appointment)
    {
        if (id != appointment.AppointmentId)
        {
            return BadRequest();
        }

        // Check if the appointment exists
        var existingAppointment = await _context.Appointments.FindAsync(id);
        if (existingAppointment == null)
        {
            return NotFound();
        }

        // Update the appointment details
        existingAppointment.UserId = appointment.UserId;
        existingAppointment.ContainerId = appointment.ContainerId;
        existingAppointment.TerminalId = appointment.TerminalId;
        existingAppointment.DriverId = appointment.DriverId;
        existingAppointment.CompanyId = appointment.CompanyId;
        existingAppointment.StartDate = appointment.StartDate;
        existingAppointment.Status = appointment.Status;
        existingAppointment.TotalCost = appointment.TotalCost;
        existingAppointment.TicketNumber = appointment.TicketNumber;
        existingAppointment.MoveType = appointment.MoveType;
        existingAppointment.GateCode = appointment.GateCode;
        existingAppointment.AppointmentStatus = appointment.AppointmentStatus;
        existingAppointment.GateStatus = appointment.GateStatus;
        existingAppointment.Line = appointment.Line;
        existingAppointment.CheckIn = appointment.CheckIn;
        existingAppointment.TransportType = appointment.TransportType;

        _context.Entry(existingAppointment).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
