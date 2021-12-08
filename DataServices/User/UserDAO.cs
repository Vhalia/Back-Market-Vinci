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
         public void CreateUser(IUserDTO user)
        {
            _usersTable.InsertOne((User)user);
        }

        public List<IUserDTO> GetUsers()
        {
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u =>
                new User(u.Name, u.Surname)).ToList<IUserDTO>();
            return allUsers;
        }


    }
}
