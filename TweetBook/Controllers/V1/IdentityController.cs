﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!authResponse.Success)
                return BadRequest(new AuthFailResponse { Errors = authResponse.Errors });

            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
                return BadRequest(new AuthFailResponse { Errors = authResponse.Errors });

            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }
    }
}