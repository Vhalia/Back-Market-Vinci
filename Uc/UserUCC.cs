using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Uc
{
    public class UserUCC : IUserUCC
    {
        private IUserDAO _userDAO;
        public UserUCC(IUserDAO userDAO) {
            this._userDAO = userDAO;
        }
        public List<IUserDTO> GetUsers()
        {
            return _userDAO.GetUsers();
        }

        public User Register(IUserDTO userDto)
        {
            User user = (User)userDto;

            return user;
        }
    }
}
