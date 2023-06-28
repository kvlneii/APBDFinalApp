using APBDBlazorApp.Server.Services;
using APBDBlazorApp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APBDBlazorApp.Server.Controllers
{
    [Authorize]
    [Route("api/users/watchlist")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersDbService _usersDbService;
        public UsersController(IUsersDbService service)
        {
            _usersDbService = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyToWatchList(CompanyPost post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _usersDbService.AddCompanyToWatchListAsync(userId, post.IdCompany);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetWatchList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usersDbService.GetUsersWatchListAsync(userId);
            return Ok(result);
        }

        [HttpDelete("{ticker}")]
        public async Task<IActionResult> DeleteCompanyFromWatchList(string ticker)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _usersDbService.DeleteCompanyFromWatchListAsync(ticker, userId);
            return NoContent();
        }
    }
}