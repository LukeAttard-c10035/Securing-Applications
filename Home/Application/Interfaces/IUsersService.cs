using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        public List<string> GetUsers();
        public string GetPivateKey(string email);
        public string GetPublicKey(string email);

    }
}
