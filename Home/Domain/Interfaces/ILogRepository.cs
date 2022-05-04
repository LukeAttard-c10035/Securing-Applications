using Domain.Models;
using System.Linq;

namespace Domain.Interfaces
{
    public interface ILogRepository
    {
        public void AddLog(Log log);
    }
}
