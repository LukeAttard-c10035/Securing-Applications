using Data.Context;
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
        private FileTransferContext context;
        public UsersRepository(FileTransferContext _fileTransferContext)
        {
            context = _fileTransferContext;
        }
        public List<string> GetUsers()
        {
            List<string> usersList = new List<string>();
            foreach (var user in context.Users)
            {   
                usersList.Add(user.UserName);
            }
            return usersList;
        }

        public CustomUser GetUser(string username)
        {
            return context.Users.SingleOrDefault(user => user.UserName == username);
        }
    }
}
