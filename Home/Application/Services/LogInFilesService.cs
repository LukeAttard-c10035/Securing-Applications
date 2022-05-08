﻿using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class LogInFilesService : ILogService
    {
        private ILogRepository logRepo;
        public LogInFilesService(ILogRepository _logRepo)
        {
            logRepo = _logRepo;
        }

        public void AddLog(Log log)
        {
            logRepo.AddLog(log);
        }

        public void SetupLog(string userIP, string userEmail, string message, string type)
        {
            throw new System.NotImplementedException();
        }
    }
}
