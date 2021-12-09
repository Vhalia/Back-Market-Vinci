using Back_Market_Vinci.Domaine;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Back_Market_Vinci.Config;

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
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u =>
                new User(u.Id, u.Name, u.Surname, u.Mail, u.Campus, u.Password, u.IsBanned.Value, u.Dislike.Value, u.Like.Value, u.IsAdmin.Value)).ToList<IUserDTO>();
            return allUsers;
        }

        public IUserDTO GetUserByMail(string mail)
        {
            IUserDTO user = _usersTable.AsQueryable().Single(u =>
                u.Mail.Equals(mail));
            return user;

        }

        public IUserDTO Register(IUserDTO user)
        {

            _usersTable.InsertOne((User)user);
            return user;
        }

        public void DeleteUser(string id)
        {
            _usersTable.DeleteOne<User>(u => u.Id.Equals(id));
        }

        public IUserDTO GetUserById(string id)
        {
            IUserDTO user = _usersTable.AsQueryable().Single(u => u.Id.Equals(id));
            return user;
        }

        public IUserDTO UpdateUser(IUserDTO user, string id)
        {
            IUserDTO userFromDB = this.GetUserById(id);


            IUserDTO modifiedUser = CheckNullFields<IUserDTO>.CheckNull(user, userFromDB);

            _usersTable.ReplaceOne<User>(u => u.Id.Equals(user.Id), (User)modifiedUser);

            return modifiedUser;

        }

        public IUserDTO Login(IUserDTO user)
        {
            IUserDTO userFromDB = this.GetUserByMail(user.Mail);

            if (userFromDB.Password.Equals(user.Password))
            {
                return userFromDB;
            }
            else
            {
                return null;
            }


        }

    }
}
