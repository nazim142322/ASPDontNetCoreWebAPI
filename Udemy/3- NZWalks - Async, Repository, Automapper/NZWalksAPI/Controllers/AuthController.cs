﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager; //to create user
        private readonly RoleManager<IdentityRole> _roleManager; //to perform task role related
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenRepository = tokenRepository;
        }


        // POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            // Validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.EmailAddress
            };

            var createResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(new { Errors = createResult.Errors.Select(e => e.Description) });
            }

            // Add roles to the user if needed
            var role = registerRequestDTO.Roles.ToUpper();

            if (role == "READER" || role == "WRITER")
            {
                var roleResult = await _userManager.AddToRoleAsync(identityUser, role);

                if (!roleResult.Succeeded)
                {
                    return BadRequest(new { Errors = roleResult.Errors.Select(e => e.Description) });
                }
            }

            return Ok(new { Message = "User registered successfully" });
        }
        //Issue: user getting added without role if not exits like reader and writer - RESOLVED BELOW

       
        // POST: api/Auth/RegisterUser
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            // Validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = registerRequestDTO.Roles?.ToUpper();

            if (role != "READER" && role != "WRITER")
            {
                return BadRequest(new {Errors = "Invalid Role Allowed Reader or Writer"});
            }

            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.EmailAddress
            };

            var createResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(new { Errors = createResult.Errors.Select(e => e.Description) });
            }

            // Add roles to the user
            var roleResult = await _userManager.AddToRoleAsync(identityUser, role);

            if (!roleResult.Succeeded)
            {
                return BadRequest(new { Errors = roleResult.Errors.Select(e => e.Description) });
            }

            return Ok(new { Message = "User registered successfully" });
        }
        // issue : roles are hard coded not validating from role table - RESOLVED BELOW



        //POST: api/Auth/RegisterUserValidRole
        [HttpPost("RegisterUserValidRole")]
        public async Task<IActionResult> RegisterUserValidRole([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            // validate the request by manually checking the properties (null empty or whitespace) if dont apply validation attributes in DTO
            if (registerRequestDTO == null || string.IsNullOrWhiteSpace(registerRequestDTO.Username) ||
                string.IsNullOrWhiteSpace(registerRequestDTO.EmailAddress) ||
                string.IsNullOrWhiteSpace(registerRequestDTO.Password) ||
                string.IsNullOrWhiteSpace(registerRequestDTO.Roles))
            {
                return BadRequest(new { Errors = new[] { "Invalid registration data." } });
            }

            var role = registerRequestDTO.Roles.ToUpper();
            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                if (!roleExists)
                {
                    return BadRequest(new
                    {
                        Errors = $"Role '{role}' does not exist."
                    });
                }
            }
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.EmailAddress
            };
            var createResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(new { Errors = createResult.Errors.Select(e => e.Description) });

            }
            // Add roles to the user
            var roleResult = await _userManager.AddToRoleAsync(identityUser, role);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new { Errors = roleResult.Errors.Select(e => e.Description) });
            }
            return Ok(new { Message = "User registered successfully" });
        }


        //Post :api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            //we have validation attribute in DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check if user exists
            var identityUser = await _userManager.FindByNameAsync(loginRequestDTO.Username);
            if (identityUser == null)
            {
                return Unauthorized(new { Message = "Invalid usernamee or password" });
            }

            //Check if password is correct return true or false
            var passwordCheck = await _userManager.CheckPasswordAsync(identityUser, loginRequestDTO.Password);//return true or false
            if (!passwordCheck)
            {
                return Unauthorized(new { Message = "Invalid Username or Passwordd" });
            }

            //get the user role
            var roles = await _userManager.GetRolesAsync(identityUser);

            //Check if user has any roles assigned
            if (roles == null || !roles.Any())
            {
               return BadRequest(new { Message = "User has no roles assigned" });
            }

            //generate jwt token
            var jwtToken = _tokenRepository.CreateJWTToken(identityUser, roles.ToList());

            //var response = new LoginResponseDTO
            //{
            //JwtToken = jwtToken,
            //};

            //return the token and user detail for later use            
            var response = new
            {
                Token = jwtToken,
                UserId = identityUser.Id,
                Username = identityUser.UserName,
                Email = identityUser.Email,
                Roles = roles
            };
            //return Ok(jwtToken);
            return Ok(response);
            //return Ok(new { Message = "login successfully" });
        }

    }
}


