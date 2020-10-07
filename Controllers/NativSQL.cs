using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper; 
using WebApi.Helpers;
using Microsoft.Extensions.Options; 
using Microsoft.AspNetCore.Authorization; 
using WebApi.Services; 
using Microsoft.AspNetCore.Mvc.Filters; 
using Microsoft.EntityFrameworkCore;

namespace WebApi.NewController
{

    
    [Authorize]
    [ApiController]
    [Route("sql")]
    public class NativSQL : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private DataContext _context;
        internal static string error_message = "";
    

 
        public  AuthorizationFilterContext __;
        public NativSQL(
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

        [HttpGet("all-user"), AllowAnonymous]
         public IActionResult New()
        { 
            try
            { 
                var series = new List<object>();
                var sql = _context.Database.GetDbConnection().CreateCommand();
                    sql.CommandText = "select * from users";
                    _context.Database.OpenConnection();
                var reader = sql.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        series.Add(new
                        {
                            id = reader.GetValue(0),
                            FristName = reader.GetValue(1),
                            LastName = reader.GetValue(2),
                            Username = reader.GetValue(3),
                            Email = reader.GetValue(4)
                        });
                    }
                }

                _context.Database.CloseConnection();
                return  Ok(new {data = series});
            }
            catch (Exception ex)
            {
             return Ok(ex);
            }
        }


      
    }
}
