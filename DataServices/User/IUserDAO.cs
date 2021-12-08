using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;

namespace Back_Market_Vinci.DataServices
{
    public interface IUserDAO
    {
        public List<IUserDTO> GetUsers();

        public void CreateUser(IUserDTO user);

        public IUserDTO GetUserByMail(string mail);

    }
}
