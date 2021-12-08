using Back_Market_Vinci.Domaine;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.DataServices
{
    public class UserDAO : IUserDAO
    {
        private IDalServices _dalServices;
        private IMongoCollection<User> _usersTable;

        public UserDAO(IDalServices dalServices) {
            this._dalServices = dalServices;
            this._usersTable = _dalServices.UsersCollection;
        }

        public List<IUserDTO> GetUsers()
        {
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u =>
                new User(u.Id,u.Name, u.Surname,u.Mail, u.Campus, u.Password)).ToList<IUserDTO>();
            return allUsers;
        }

        public IUserDTO GetUserByMail(string mail) {
            IUserDTO user = _usersTable.AsQueryable().Single(u =>
                u.Mail.Equals(mail));
            return user;

        }


    }
}
