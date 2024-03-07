using AspNetCoreWebAPI8.Models;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Security.Principal;

namespace AspNetCoreWebAPI8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneNumbersController : ControllerBase
    {
        private readonly AccountContext _accountContext;
        public PhoneNumbersController(AccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        // Get : api/PhoneNumbers
        [HttpGet(Name = "GetAll")]
        public async Task<ActionResult<IEnumerable<PhoneNumber>>> GetPhoneNumbers()
        {
            if (_accountContext.PhoneNumbers == null)
            {
                return NotFound();
            }
            var inclAcc = await _accountContext.PhoneNumbers.Include(p => p.Account).ToListAsync();
            return inclAcc.ToList();
        }

        // Get : api/PhoneNumbers/2
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneNumber>> GetPhoneNumber(int id)
        {
            if (_accountContext.PhoneNumbers is null)
            {
                return NotFound();
            }
            var phoneNumber = await _accountContext.PhoneNumbers.FindAsync(id);
            if (phoneNumber is null)
            {
                return NotFound();
            }
            return phoneNumber;
        }

        // Post : api/PhoneNumbers
        [HttpPost]
        public async Task<ActionResult<PhoneNumber>> PostPhoneNumber(PhoneNumber phoneNumber)
        {
            var newPhoneNumber = _accountContext.PhoneNumbers.Add(phoneNumber);
            foreach (var a in _accountContext.Accounts)
            {
                if (a.Id == phoneNumber.AccountId && a.Status == true)
                {
                    await _accountContext.SaveChangesAsync();
                }
            }
            return CreatedAtAction(nameof(GetPhoneNumber), new { id = phoneNumber.Id }, phoneNumber);
        }

        // Put : api/PhoneNumbers/2
        [HttpPut]
        public async Task<ActionResult<PhoneNumber>> PutPhoneNumber(int id, PhoneNumber phoneNumber)
        {
            if (id != phoneNumber.Id && !PhoneIdAccIdMatch(id))
            {
                return BadRequest();
            }
            _accountContext.PhoneNumbers.Entry(phoneNumber).State = EntityState.Modified;
            try
            {
                await _accountContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneNumberExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        //Delete : api/PhoneNumbers/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhoneNumber>> DeletePhoneNumber(int id)
        {
            if (_accountContext.PhoneNumbers is null)
            { 
                return NotFound();
            }
            var phoneNumber = await _accountContext.PhoneNumbers.FindAsync(id);
            if (phoneNumber is null)
            {
                return NotFound($"Phone Number with Id = {id} not found");
            }
            _accountContext.PhoneNumbers.Remove(phoneNumber);
            await _accountContext.SaveChangesAsync();
            return NoContent();
        }


        private bool PhoneNumberExists(long id)
        {
            return (_accountContext.PhoneNumbers?.Any(phoneNumber => phoneNumber.Id == id)).GetValueOrDefault();
        }
        private bool PhoneIdAccIdMatch(long id)
        {
            bool validation = false;
            foreach(var pn in _accountContext.PhoneNumbers.Where(pn => pn.Id == id))
            {
                foreach(var a in _accountContext.Accounts)
                {
                    if (a.Id != pn.AccountId || a.Status != false)
                    {
                        validation = true;
                    }
                }
            }
            return validation;
        }
    }
}
