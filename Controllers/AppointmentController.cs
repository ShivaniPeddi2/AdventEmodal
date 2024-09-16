using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAppointments()
    {
        // Admin can retrieve all appointments
        var appointments = await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Container)
            .Include(a => a.Terminal)
            .Include(a => a.Driver)
            .Include(a => a.TruckCompany)
            .ToListAsync();
        
        return Ok(appointments);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(int id)
    {
        // Retrieve the appointment
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

        // Check if the user is authorized to access this appointment
        if (User.IsInRole("Admin") || appointment.UserId == GetUserId())
        {
            return Ok(appointment);
        }

        return Forbid(); // User does not have permission
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
        appointment.UpdatedAt = DateTime.UtcNow;

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
        existingAppointment.StartDate = appointment.StartDate;
        existingAppointment.EndDate = appointment.EndDate;
        existingAppointment.Status = appointment.Status;
        existingAppointment.TotalCost = appointment.TotalCost;
        existingAppointment.TicketNumber = appointment.TicketNumber;
        existingAppointment.UpdatedAt = DateTime.UtcNow;

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

    private int GetUserId()
    {
        // Assuming you store the UserId in the claims, you might need to adjust this based on your authentication setup
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        return int.Parse(userIdClaim?.Value ?? "0");
    }
}
