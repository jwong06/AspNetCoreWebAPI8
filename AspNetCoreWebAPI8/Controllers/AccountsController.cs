using AspNetCoreWebAPI8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Principal;

namespace AspNetCoreWebAPI8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountContext _accountContext;
        public AccountsController(AccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        // Get : api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            if (_accountContext.Accounts == null)
            {
                return NotFound();
            }
            return await _accountContext.Accounts.ToListAsync();
        }

        // Get : api/Accounts/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            if (_accountContext.Accounts is null)
            {
                return NotFound();
            }
            var account = await _accountContext.Accounts.FindAsync(id);
            if (account is null)
            {
                return NotFound();
            }
            return account;
        }

        // Post : api/Accounts
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _accountContext.Accounts.Add(account);
            await _accountContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        // Put : api/Accounts/2
        [HttpPut]
        public async Task<ActionResult<Account>> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }
            _accountContext.Entry(account).State = EntityState.Modified;
            try
            {
                await _accountContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool AccountExists(long id)
        {
            return (_accountContext.Accounts?.Any(account => account.Id == id)).GetValueOrDefault();
        }
    }
}
