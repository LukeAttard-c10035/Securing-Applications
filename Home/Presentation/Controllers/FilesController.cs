using Application.Interfaces;
using Application.ViewModels;
using Domain.Models;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;

namespace Presentation.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private IFilesService filesService;
        private IWebHostEnvironment webHostEnvironment;
        private ILogService logService;
        public FilesController(IFilesService _filesService, IWebHostEnvironment _webHostEnvironment, ILogService _logService)
        {
            logService = _logService;
            filesService = _filesService;
            webHostEnvironment = _webHostEnvironment;
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
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="User")]
        public IActionResult Create(AddFileTransferViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Please Fill out the rest of the fields";
                    logService.SetupLog(HttpContext.Connection.RemoteIpAddress.ToString(),  
                        model.UserEmail, $"{model.UserEmail} failed uploading {model.FilePath} set to expire {model.ExpiryDate}", "Error");
                    return View();
                }
                else
                {
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