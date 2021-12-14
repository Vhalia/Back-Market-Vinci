using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Exceptions;
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
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u => new User(u.Id,u.Name, u.Surname,u.Mail,
                u.Campus, u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Ratings))
                .ToList<IUserDTO>();
            return allUsers;
        }

        public IUserDTO GetUserByMail(string mail) {
            IUserDTO user = null;
            try
            {
                user = _usersTable.AsQueryable().Select(u => new User(u.Id, u.Name, u.Surname, u.Mail, u.Campus,
                    u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Ratings))
                    .Where(u => u.Mail.Equals(mail))
                    .Single<IUserDTO>();          
            }
            catch (ArgumentNullException)
            {
                throw new UserNotFoundException("L'utilisateur avec le mail " + mail + " n'a pas été trouvé");
            }
            catch(InvalidOperationException)
            {
                throw new InternalServerError("Plusieurs utilisateurs avec le même mail existe ou aucun utilisateur avec ce mail a été trouvé");
            }
            return user;

        }

        public IUserDTO Register(IUserDTO user) {
            _usersTable.InsertOne((User) user);
            return  user;
        }

        public void DeleteUser(string id)
        {
            IUserDTO userDeleted = _usersTable.FindOneAndDelete<User>(u => u.Id.Equals(id));
            if (userDeleted == null) throw new UserNotFoundException("L'utilisateur avec l'id " + id + " n'a pas été trouvé");
        }

        public IUserDTO GetUserById(string id)
        {
            IUserDTO user = _usersTable.AsQueryable().FirstOrDefault(u => u.Id.Equals(id));
            if (user == null) throw new UserNotFoundException("L'utilisateur avec l'id " + id + " n'a pas été trouvé");
            return user;
        }

        public IUserDTO UpdateUser(IUserDTO modifiedUser) {

            var res = _usersTable.ReplaceOne<User>(u => u.Id.Equals(modifiedUser.Id), (User)modifiedUser);
            if (!res.IsAcknowledged) throw new InternalServerError("Erreur lors de la mise à jour de l'utilisateur");
            return modifiedUser;

        }


    }
}
