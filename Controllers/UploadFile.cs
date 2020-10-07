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
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApi.NewController
{

    
    [Authorize]
    [ApiController]
    [Route("file")]
    public class File_ : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private DataContext _context;
        internal static string error_message = "";
    

 
        public  AuthorizationFilterContext __;
        public File_(
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

        //Uploading Multiple Files
        [HttpPost("uplod-image"), AllowAnonymous]
        public IActionResult Upload(List<IFormFile> files){
            try
                {
                    var files_ = files;
                    var folderName = Path.Combine("Upload", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (files_.Any(f => f.Length == 0))
                    {
                        return BadRequest();
                    }
                    foreach (var file in files_)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    return Ok(new{ message = "All the files are successfully uploaded."});
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error {ex}");
                }
        }

        [HttpGet("download/{filename}"),AllowAnonymous]
        public IActionResult DownloadFile(string filename)
        {
            // Since this is just and example, I am using a local file located inside wwwroot
            // Afterwards file is converted into a stream
            var path = Path.Combine("Upload", "Images",filename);
            var fs = new FileStream(path, FileMode.Open);

            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", filename);
        }

      
    }
}
