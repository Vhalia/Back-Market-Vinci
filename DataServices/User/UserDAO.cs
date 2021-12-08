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
        private IDalServices dalServices;
        public UserDAO(IDalServices dalServices) {
            this.dalServices = dalServices;
        }
         public void CreateUser(IUserDTO user)
        {
            
            var database = dalServices.GetDatabase();
            var table = database.GetCollection<User>("Users");
            table.InsertOne((User)user);
        }

        public List<IUserDTO> GetUsers()
        {
            var database = dalServices.GetDatabase();
            var tableUsers =  database.GetCollection<User>("Users");
            List<IUserDTO> allUsers = tableUsers.AsQueryable().Select(u =>
                new User(u.Id, u.Name, u.Surname, u.Mail, u.Campus, u.Password)).ToList<IUserDTO>();
            return allUsers;
        }

        public IUserDTO GetUserByMail(string mail) {
            var database = dalServices.GetDatabase();
            var tableUsers = database.GetCollection<User>("Users");
            IUserDTO user = tableUsers.AsQueryable().Single(u =>
                u.Mail.Equals(mail));
            return user;

        }


    }
}
