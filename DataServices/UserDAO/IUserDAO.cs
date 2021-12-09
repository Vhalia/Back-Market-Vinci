using Back_Market_Vinci.DataServices.ProductDAO;
using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;

namespace Back_Market_Vinci.DataServices
{
    public interface IUserDAO
    {
        public List<IUserDTO> GetUsers();

        public IUserDTO GetUserByMail(string mail);

        public IUserDTO Register(IUserDTO user);

        public void DeleteUser(string id);

        public IUserDTO UpdateUser(IUserDTO user, string id);

        public IUserDTO Login(IUserDTO user);
        public IUserDTO GetUserById(string id);
    }
}
