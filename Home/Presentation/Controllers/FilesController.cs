using Application.Interfaces;
using Application.ViewModels;
using Domain.Models;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilter;
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
        [ValidateAntiForgeryToken()]
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
                    FileValidator magic = new FileValidator();
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
                        Cryptographic c = new Cryptographic();
                        MemoryStream ms = new MemoryStream();
                        string privateKey = usersService.GetPivateKey(User.Identity.Name);
                        file.OpenReadStream().Position = 0; //important to do this evreytime you use a stream
                        file.CopyTo(ms);
                        ms.Position = 0;
                        byte[] myFile = ms.ToArray();
                        string myFileAsString = Convert.ToBase64String(myFile);
                        model.DigitalSignature = c.DigitalSigning(myFile, privateKey);
                        MemoryStream ms1 = new MemoryStream();
                        file.CopyTo(ms1);
                        ms1.Position = 0;
                        Stream encryptedFile = c.HybridEncryption(ms1, usersService.GetPublicKey(User.Identity.Name));
                        encryptedFile.Position = 0;
                        encryptedFile.CopyTo(ms);
                        ms.Position = 0;
                        byte[] myEncryptedFile = ms.ToArray();
                        string absoluteSecurePath = webHostEnvironment.WebRootPath + "\\files-secure\\" + fileName;//fileName;
                        System.IO.File.WriteAllBytes(absoluteSecurePath, myEncryptedFile); //use convert to bytes and store it in asafe plack

                        // removed zip
                        model.Extension = Path.GetExtension(file.FileName);
                        model.FilePath = @"\files-secure\" + fileName;
                        model.FileName = file.FileName;
                    }
                    filesService.AddFileTransfer(model);
                    ViewBag.Message = "FileTransfer saved successfully";
                    logService.SetupLog(HttpContext.Connection.RemoteIpAddress.ToString(), model.UserEmail, $"{model.UserEmail} uploaded {model.FilePath} set to expire {model.ExpiryDate}", "Info");
                }
            } catch (Exception ex)
            {
                ViewBag.Error = "FileTransfer was not saved - " + ex.Message;
            }
            return RedirectToAction("Create");
        }
        [CustomAuthorization()]
        public IActionResult Download(int fileTransferId)
        {
            try
            {
                Cryptographic c = new Cryptographic();
                string publicKey = "", privateKey = "";
                var ft = filesService.GetFileTransfer(fileTransferId);
                    Stream file = System.IO.File.OpenRead(webHostEnvironment.WebRootPath + ft.FilePath);
                    if (ft.UserEmail != User.Identity.Name)
                    {
                        publicKey = usersService.GetPublicKey(ft.UserEmail);
                        privateKey = usersService.GetPivateKey(ft.UserEmail);
                    }
                    else
                    {
                        publicKey = usersService.GetPublicKey(User.Identity.Name);
                        privateKey = usersService.GetPivateKey(User.Identity.Name);
                    }
                    Stream decryptedFile = c.HybridDecryption(file, usersService.GetPivateKey(ft.UserEmail));
                    decryptedFile.Position = 0;
                    MemoryStream ms = new MemoryStream();
                    decryptedFile.CopyTo(ms);
                    ms.Position = 0;
                    byte[] data = ms.ToArray();
                    string myFileAsString = Convert.ToBase64String(data);
                    decryptedFile.Position = 0;
                    if (!c.DigitalVerification(myFileAsString, ft.DigitalSignature, publicKey))
                    {
                        throw new Exception("File has been tampered");
                    }
                    return File(decryptedFile, "application/octet-stream", Guid.NewGuid().ToString() + ft.Extension); // use the ft extension later
                // 1 get the required keys
                // 1.5 get the location of the file
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new {message = ex.Message});
            }

        }
    }
}
