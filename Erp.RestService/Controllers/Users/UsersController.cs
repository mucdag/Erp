using Erp.Business.WriteManagers.Users;
using Erp.Resource.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Erp.RestService.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserWriteManager _userWriteManager;

        public UsersController(IUserWriteManager userWriteManager)
        {
            _userWriteManager = userWriteManager;
        }

        [HttpPost]
        [Route("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserConfirmResource userResource)
        {
            return Ok(await _userWriteManager.LoginAsync(userResource));
        }

        [HttpPost]
        [Route("passwordResetRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordResetRequestAsync(PasswordResetResource passwordResetResource)
        {
            await _userWriteManager.PasswordResetRequestAsync(passwordResetResource);
            return Ok();
        }

        [HttpPut]
        [Route("passwordReset")]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordResetAsync(PasswordResetResource passwordResetResource)
        {
            await _userWriteManager.PasswordResetAsync(passwordResetResource);
            return Ok();
        }

    }
}
