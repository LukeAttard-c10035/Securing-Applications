using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public IQueryable<FileTransfer> GetFileTransfers()
        {
            throw new NotImplementedException();
        }
    }
}
