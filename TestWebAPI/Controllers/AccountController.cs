using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Models;
using ShoeStoreManagement.Services;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILogger<AccountController> _logger;
        private GeneralDBContext _generalDBContext;

        public AccountController(ILogger<AccountController> logger, GeneralDBContext generalDBContext)
        {
            _logger = logger;
            _generalDBContext = generalDBContext;
        }

        // GET: api/<AccountController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Get()
        {
            return await _generalDBContext.Accounts.Select(x => x).ToListAsync();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> Get(string id)
        {
            return await _generalDBContext.Accounts.FirstAsync(x => x.AccountId.Equals(id));
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Account value)
        {
            _generalDBContext.Entry(value).State = EntityState.Added;


            try
            {
                await _generalDBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            var systemAccount = await _generalDBContext.Accounts.FindAsync(id);
            if (systemAccount == null)
            {
                return NotFound();
            }

            systemAccount.AccountType = 1;

            try
            {
                await _generalDBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(string id)
        {
            var todoItem = await _generalDBContext.Accounts.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _generalDBContext.Accounts.Remove(todoItem);
            await _generalDBContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
