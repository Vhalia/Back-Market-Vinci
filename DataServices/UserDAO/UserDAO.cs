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
        private IMongoCollection<Badges> _badgesTable;

        public UserDAO(IDalServices dalServices)
        {
            this._dalServices = dalServices;
            this._usersTable = _dalServices.UsersCollection;
            this._badgesTable = _dalServices.BadgesCollection;
        }

        public List<IUserDTO> GetUsers()
        {
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u => new User(u.Id,u.Name, u.Surname,u.Mail, u.Campus, u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Sold, u.Bought, u.FavTypes, u.FavProducts, u.Badges, u.Image, u.Ratings)).ToList<IUserDTO>();
            return allUsers;
        }

        public List<IBadgesDTO> GetBadges() {
            List<IBadgesDTO> allBadges = _badgesTable.AsQueryable().Select(b => new Badges(b.Id, b.Image, b.IsUnlocked, b.Title, b.Description)).ToList<IBadgesDTO>();
            return allBadges;
        }

        public IUserDTO GetUserByMail(string mail) {
            IUserDTO user = _usersTable.AsQueryable().Select(u => new User(u.Id, u.Name, u.Surname, u.Mail, u.Campus, u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Sold, u.Bought, u.FavTypes, u.FavProducts, u.Badges, u.Image, u.Ratings)).Where(u => u.Mail.Equals(mail)).Single<IUserDTO>();
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
            IUserDTO user = _usersTable.AsQueryable().Select(u => new User(u.Id, u.Name, u.Surname, u.Mail, u.Campus, u.Password, u.IsBanned,u.IsAdmin, u.Sold, u.Bought, u.FavTypes, u.FavProducts, u.Badges, u.Image, u.Ratings)).Where(u => u.Id.Equals(id)).Single<IUserDTO>();
            return user;
        }

        public IUserDTO UpdateUser(IUserDTO modifiedUser) {

            _usersTable.ReplaceOne<User>(u => u.Id.Equals(modifiedUser.Id), (User)modifiedUser);

            return modifiedUser;

        }

    }
}
