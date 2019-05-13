using MakeBuddies.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace MakeBuddies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController
    {
        private readonly IAuthRepo repo;
        public AuthController(IAuthRepo repo)
        {
            this.repo = repo;
        }

        

    }
}