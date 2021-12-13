using Back_Market_Vinci.Domaine;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Back_Market_Vinci.DataServices
{
    public class UserDAO : IUserDAO
    {
        private IDalServices _dalServices;
        private IMongoCollection<User> _usersTable;

        public UserDAO(IDalServices dalServices)
        {
            this._dalServices = dalServices;
            this._usersTable = _dalServices.UsersCollection;
        }

        public List<IUserDTO> GetUsers()
        {
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u => new User(u.Id,u.Name, u.Surname,u.Mail, u.Campus, u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Ratings)).ToList<IUserDTO>();
            return allUsers;
        }

        public IUserDTO GetUserByMail(string mail) {
            IUserDTO user = _usersTable.AsQueryable().Single(u => u.Mail.Equals(mail));
            return user;

        }

        public IUserDTO Register(IUserDTO user) {
            _usersTable.InsertOne((User) user);
            return  user;
        }

        public void DeleteUser(string id)
        {
            _usersTable.DeleteOne<User>(u => u.Id.Equals(id));
        }

        public IUserDTO GetUserById(string id)
        {
            IUserDTO user = _usersTable.AsQueryable().FirstOrDefault(u => u.Id.Equals(id));
            return user;
        }

        public IUserDTO UpdateUser(IUserDTO modifiedUser) {

            _usersTable.ReplaceOne<User>(u => u.Id.Equals(modifiedUser.Id), (User)modifiedUser);

            return modifiedUser;

        }

    }
}
