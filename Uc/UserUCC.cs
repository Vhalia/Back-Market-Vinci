﻿using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_Market_Vinci.Config;
using System.Web.Http;
using System.Net;

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
            return _userDAO.GetUserByMail(mail);
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
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
        public void AddRating(Ratings ratings) {

            _ratingsDAO.AddRatings(ratings);


            IUserDTO modifiedUser = _userDAO.GetUserById(ratings.IdRater);
            modifiedUser.Ratings.Add(ratings);
            _userDAO.UpdateUser(modifiedUser);
        }
    }
}
