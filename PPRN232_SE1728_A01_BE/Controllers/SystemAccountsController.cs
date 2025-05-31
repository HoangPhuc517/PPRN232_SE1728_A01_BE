using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Entity;
using Services.Interface;

namespace PPRN232_SE1728_A01_BE.Controllers
{
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService _systemAccountService;
        public SystemAccountsController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }


        /// <summary>
        /// Get OData list of SystemAccounts
        /// </summary>
        /// <returns>List</returns>
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var systemAccounts = await _systemAccountService.GetAllAsync();
                return Ok(systemAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
            {
                return BadRequest("SystemAccount cannot be null");
            }
            try
            {
                var createdSystemAccount = await _systemAccountService.Create(systemAccount);
                return Created(createdSystemAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch(int key, [FromBody] Delta<SystemAccount> delta)
        {
            if (delta == null)
            {
                return BadRequest("Delta cannot be null");
            }
            try
            {
                var updatedSystemAccount = await _systemAccountService.Update(key, delta);
                return Updated(updatedSystemAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            try
            {
                await _systemAccountService.Delete(key);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login( string email, string password)
        {
            try
            {
                var systemAccount = await _systemAccountService.LoginAsync(email, password);
                if (systemAccount == null)
                {
                    return Unauthorized("Invalid email or password");
                }
                return Ok(systemAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
