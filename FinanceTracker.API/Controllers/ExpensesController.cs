using FinanceTracker.API.DTOs;
using FinanceTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>Get all expense entries</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
        {
            var result = await _expenseService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>Get expense entries for a specific month and year</summary>
        [HttpGet("{year:int}/{month:int}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");
            var result = await _expenseService.GetByMonthAsync(year, month);
            return Ok(result);
        }

        /// <summary>Get a single expense entry by ID</summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExpenseDto>> GetById(int id)
        {
            var result = await _expenseService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>Create a new expense entry</summary>
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseDto dto)
        {
            var result = await _expenseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>Update an existing expense entry</summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ExpenseDto>> Update(int id, [FromBody] UpdateExpenseDto dto)
        {
            var result = await _expenseService.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>Delete an expense entry</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _expenseService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
