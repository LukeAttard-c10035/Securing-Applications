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
            });
        }
    }
}
