using FinanceTracker.API.DTOs;
using FinanceTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        /// <summary>Get all income entries</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll()
        {
            var result = await _incomeService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>Get income entries for a specific month and year</summary>
        [HttpGet("{year:int}/{month:int}")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");
            var result = await _incomeService.GetByMonthAsync(year, month);
            return Ok(result);
        }

        /// <summary>Get a single income entry by ID</summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IncomeDto>> GetById(int id)
        {
            var result = await _incomeService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>Create a new income entry</summary>
        [HttpPost]
        public async Task<ActionResult<IncomeDto>> Create([FromBody] CreateIncomeDto dto)
        {
            var result = await _incomeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>Update an existing income entry</summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<IncomeDto>> Update(int id, [FromBody] UpdateIncomeDto dto)
        {
            var result = await _incomeService.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>Delete an income entry</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _incomeService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
