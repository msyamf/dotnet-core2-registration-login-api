using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; 
using WebApi.Services;
using WebApi.Dtos;
using WebApi.Form;
using WebApi.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using WebApi.Helpers.Validation;

namespace WebApi.NewController
{

    
    [Authorize]
    [ApiController]
    [Route("new")]
    public class newController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private DataContext _context;
        internal static string error_message = "";
    

 
        public  AuthorizationFilterContext __;
        public newController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            DataContext context)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _context = context;
        }

        [HttpGet("new"), AllowAnonymous]
         public IActionResult New()
        { 
            try
            { 
                return  Ok(new {message = "Hellow Word"});
            }
            catch (Exception ex)
            {
             return Ok(ex);
            }
        }


      
    }
}
