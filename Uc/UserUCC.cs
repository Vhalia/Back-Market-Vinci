using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_Market_Vinci.Config;
using System.Web.Http;
using System.Net;
using Microsoft.AspNetCore.Builder;
using System.Text.RegularExpressions;

namespace Back_Market_Vinci.Uc
{
    public class UserUCC : IUserUCC
    {
        private IUserDAO _userDAO;
        private IRatingsDAO _ratingsDAO;
        public UserUCC(IUserDAO userDAO, IRatingsDAO ratingsDAO) {
            this._ratingsDAO = ratingsDAO;
            this._userDAO = userDAO;
        }
        public List<IUserDTO> GetUsers()
        {
            return _userDAO.GetUsers();
        }

        public IUserDTO GetUserByMail(string mail) {
            string pattern = "^[A-Za-z0-9.]+@+(vinci|student.vinci)+(.be)$";
            Match match = Regex.Match(mail, pattern);
            if (match.Success)
            {
                return _userDAO.GetUserByMail(mail);
            }
            else {
                throw new ArgumentException("Le mail ne correspond pas à un mail vinci");
            }
           
        }

        public IUserDTO Register(IUserDTO user)
        {
            user.Ratings = new List<Ratings> ();
            user.IsAdmin = false;
            user.IsBanned = false;

            return _userDAO.Register(user);
        }

        public void DeleteUser(string id) {
            _userDAO.DeleteUser(id);
        }

        public IUserDTO UpdateUser(IUserDTO user, string id) {
            IUserDTO userFromDB = _userDAO.GetUserById(id);

            IUserDTO modifiedUser = CheckNullFields<IUserDTO>.CheckNull(user, userFromDB);
            return  _userDAO.UpdateUser(modifiedUser);
        }

        public IUserDTO Login(IUserDTO user) {
            IUserDTO userFromDB = _userDAO.GetUserByMail(user.Mail);

            if (userFromDB.Password.Equals(user.Password))
            {
                return userFromDB;
            }
            else
            {
                throw new UnauthorizedAccessException("mail ou mot de passe errone ");
            }
        }
        public void AddRating(IRatingsDTO ratings) {

            _ratingsDAO.AddRatings(ratings);


            IUserDTO modifiedUser = _userDAO.GetUserById(ratings.IdRated);
            modifiedUser.Ratings.Add((Ratings)ratings);
            _userDAO.UpdateUser(modifiedUser);
        }

        public void UpdateRatings(IRatingsDTO ratings) {

            IUserDTO userFromDB = _userDAO.GetUserById(ratings.IdRated);


            _ratingsDAO.UpdateRatings(ratings);
            userFromDB.Ratings.RemoveAll(r => r.Id.Equals(ratings.Id));
            userFromDB.Ratings.Add((Ratings)ratings);
            _userDAO.UpdateUser(userFromDB);
            
        }

        public void DeleteRatings(string id) {
            IRatingsDTO ratingsFromDB = _ratingsDAO.GetRatingsById(id);
            IUserDTO userFromDB = _userDAO.GetUserById(ratingsFromDB.IdRated);

            _ratingsDAO.DeleteRatings(id);
            userFromDB.Ratings.RemoveAll(r => r.Id.Equals(id));
            _userDAO.UpdateUser(userFromDB);

        }
    }
}
