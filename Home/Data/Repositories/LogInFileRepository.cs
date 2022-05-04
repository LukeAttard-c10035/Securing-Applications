using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;

namespace Data.Repositories
{
    public class LogInFileRepository : ILogRepository
    {
        private FileTransferContext context;
        private IWebHostEnvironment webHostEnvironment;

        public LogInFileRepository(FileTransferContext _fileTransferContext, IWebHostEnvironment _webHostEnvironment)
        {
            context = _fileTransferContext;
            webHostEnvironment = _webHostEnvironment;
        }

        public void AddLog(Log log)
        {
            //string path = @"h:\root\home\lukeattard - 001\www\ft\wwwroot\logs\logs.txt";
            string path = webHostEnvironment.WebRootPath + "\\logs\\logs.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine($"User: {log.UserEmail} IP: {log.IP} FileTransfer at: {log.Created}");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"User: {log.UserEmail} IP: {log.IP} FileTransfer at: {log.Created}");
                }
            }
        }
    }
}
