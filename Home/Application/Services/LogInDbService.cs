using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;

namespace Application.Services
{
    public class LogInDbService : ILogService
    {
        private ILogRepository logRepo; 
        public LogInDbService(ILogRepository _logRepo)
        {
            logRepo = _logRepo;
        }

        public void AddLog(Log model)
        {
            logRepo.AddLog(new Log()
            {
                Created = DateTime.Now,
                IP = model.IP,
                UserEmail = model.UserEmail,
                Info = model.Info,
            });
        }

        public void SetupLog(string userIP, string userEmail, string message, string type)
        {
            logRepo.AddLog(new Log()
            {
                Created = DateTime.Now,
                IP = userIP,
                UserEmail = userEmail,
                Info = message,
                Type = type
            });
        }
    }
}
