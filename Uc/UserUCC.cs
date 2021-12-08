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
        private IUserDAO userDAO;
        public UserUCC(IUserDAO userDAO) {
            this.userDAO = userDAO;
        }
        public List<IUserDTO> getUser()
        {
            return userDAO.GetUser();
        }

        public User register(IUserDTO userDto)
        {
            User user = (User)userDto;

            return user;
        }
    }
}
