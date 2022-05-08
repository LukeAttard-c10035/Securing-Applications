using Domain.Models;

namespace Application.Interfaces
{
    public interface ILogService
    {
        public void AddLog(Log model);
        public void SetupLog(string userIP, string userEmail, string message, string type);
    }
}
