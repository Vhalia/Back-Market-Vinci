using Back_Market_Vinci.DataServices.ProductDAO;
using Back_Market_Vinci.Domaine;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Back_Market_Vinci.DataServices
{
    public interface IUserDAO
    {
        public List<IUserDTO> GetUsers();
        public List<IBadgesDTO> GetBadges(); 
        public IUserDTO GetUserByMail(string mail);
        public IUserDTO Register(IUserDTO user);
        public void DeleteUser(string id);
        public IUserDTO GetUserById(string id);
        public IUserDTO UpdateUser(IUserDTO modifiedUser);

    }
}
