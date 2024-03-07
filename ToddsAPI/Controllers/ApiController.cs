using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using ToddsAPI.Models;

namespace ToddsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly toddsdbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiController> _logger;
        string connectionString = string.Empty;
        

        public ApiController(ILogger<ApiController> logger, toddsdbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("postdata")]
        public async Task<ActionResult<Sample>> PostData([FromBody] Sample data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Sample.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetData), new { id = data.Id }, data);
        }
        
        [HttpGet("getdata")]
        public async Task<ActionResult<IEnumerable<Sample>>> GetData()
        {
            // Retrieve data from the Sample table using Entity Framework Core
            var samples = await _context.Sample.ToListAsync();

            // Return the retrieved data as a response
            return Ok(samples);
        }
    }
}