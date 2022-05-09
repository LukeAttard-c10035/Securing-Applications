using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private IUsersRepository usersRepo;
        public UsersService(IUsersRepository _usersRepo)
        {
            usersRepo = _usersRepo;
        }
        public List<string> GetUsers()
        {
           return usersRepo.GetUsers();
        }

        public string GetPublicKey(string email)
        {
            var user = usersRepo.GetUser(email);
            return user.publicKey;
        }

        public string GetPivateKey(string email)
        {
            var user = usersRepo.GetUser(email);
            return user.privateKey;
        }
    }
}
