using Application.Interfaces;
using Application.ViewModels;
using Domain.Models;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Linq;

namespace Presentation.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private IFilesService filesService;
        private IWebHostEnvironment webHostEnvironment;
        private ILogService logService;
        private IUsersService usersService;

        public FilesController(IFilesService _filesService, IWebHostEnvironment _webHostEnvironment, 
            ILogService _logService, IUsersService _usersService)
        {
            logService = _logService;
            filesService = _filesService;
            webHostEnvironment = _webHostEnvironment;
            usersService = _usersService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            //User.Identity.Name
            var username = HttpContext.User.Identity.Name;
            var userTransfers = filesService.GetFileTransfers(username, HttpContext.Request.Host.ToString());
            return View(userTransfers);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            var usernames = usersService.GetUsers();
            usernames.Remove(User.Identity.Name);
            ViewBag.Users = usernames;
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="User")]
        public IActionResult Create(AddFileTransferViewModel model, IFormFile file)
        {
            try
            {
                model.UserEmail = User.Identity.Name;
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Please Fill out the rest of the fields";
                    logService.SetupLog(HttpContext.Connection.RemoteIpAddress.ToString(),
                        model.UserEmail, $"{model.UserEmail} failed uploading {model.FilePath} set to expire {model.ExpiryDate}", "Error");
                    return View();
                }
                else
                {
                    Magic magic = new Magic();
                    if (!magic.MagicChecker(file))
                    {
                        ViewBag.Error = "The file is not in the correct format";
                        logService.SetupLog(HttpContext.Connection.RemoteIpAddress.ToString(),
                            model.UserEmail, $"{model.UserEmail} failed uploading {model.FilePath} set to expire {model.ExpiryDate}", "Error");
                        return View();
                    }
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        string absolutePath = webHostEnvironment.WebRootPath + "\\files\\" + fileName;
                        using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                        {
                            file.CopyTo(fs);
                            fs.Close();
                        }
                        using (ZipFile zip = new ZipFile())
                        {
                            if (!String.IsNullOrEmpty(model.Password))
                            {
                                zip.Password = model.Password;  
                            }
                            zip.AddFile(absolutePath);
                            zip.Save(absolutePath + ".zip");
                        }
                        model.FilePath = @"\files\" + fileName+".zip";
                    }
                    filesService.AddFileTransfer(model);
                    ViewBag.Message = "FileTransfer saved successfully";
                    
                    // removed email
                    /*
                    Log log = new Log
                    {
                        IP = HttpContext.Connection.RemoteIpAddress.ToString(),
                        UserEmail = model.UserEmail,
                        Created = DateTime.Now,
                        Info = "User uploading or something"

                    };
                    logService.AddLog(log);*/
                    logService.SetupLog(HttpContext.Connection.RemoteIpAddress.ToString(), model.UserEmail, $"{model.UserEmail} uploaded {model.FilePath} set to expire {model.ExpiryDate}", "Info");
                }
            } catch (Exception ex)
            {
                ViewBag.Error = "FileTransfer was not saved - " + ex.Message;
            }
            return RedirectToAction("Create");
        }
    }
}