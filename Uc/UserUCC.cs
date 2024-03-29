﻿using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;
using Back_Market_Vinci.Config;
using System.Text.RegularExpressions;
using Back_Market_Vinci.Domaine.Exceptions;
using Microsoft.AspNetCore.Http;
using Back_Market_Vinci.Domaine.Other;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Back_Market_Vinci.Uc
{
    public class UserUCC : IUserUCC
    {
        private IUserDAO _userDAO;
        private IRatingsDAO _ratingsDAO;
        private IBlobService _blobServices;
        private IProductDAO _productDAO;
        public IConfiguration Configuration { get; }
        public UserUCC(IUserDAO userDAO, IRatingsDAO ratingsDAO, IBlobService blobServices, IProductDAO productDAO, IConfiguration conf) {
            this._ratingsDAO = ratingsDAO;
            this._userDAO = userDAO;
            this._blobServices = blobServices;
            this._productDAO = productDAO;
            this.Configuration = conf;
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
            if (user.Campus == null || user.Mail == null || user.Name == null || user.Password == null
                || user.Surname == null)
                throw new MissingMandatoryInformationException("Il manque des informations obligatoires");
            string pattern = "^[A-Za-z0-9.]+@+(vinci|student.vinci)+(.be)$";
            Match match = Regex.Match(user.Mail, pattern);
            if (!match.Success) throw new ArgumentException("Le mail ne correspond pas à un mail vinci");
            if (!User.CampusAvailable.Contains(user.Campus))
                throw new ArgumentException("Le campus " + user.Campus + " n'est pas correcte");
            List<IBadgesDTO> badges = _userDAO.GetBadges();
            user.Ratings = new List<Ratings>();
            user.FavProducts = new List<string>();
            user.FavTypes = new List<Types>();
            user.IsAdmin = false;
            user.IsBanned = false;
            user.FavProducts = new List<string>();
            user.Bought = new List<string>();
            user.Sold = new List<string>();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Image = Configuration["AzureBlobProperties:DefaultProfilImage"];
            user.Badges = badges.ConvertAll(b => (Badges)b);
            user.Badges.ElementAt(0).IsUnlocked = true;
            return _userDAO.Register(user);
        }

        public void DeleteUser(string id) {
            _userDAO.DeleteUser(id);
        }

        public IUserDTO UpdateUser(IUserDTO user, string id) {
            if (user.Password != null) {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            IUserDTO userFromDB = _userDAO.GetUserById(id);
            IUserDTO modifiedUser = CheckNullFields<IUserDTO>.CheckNull(user, userFromDB);
            return _userDAO.UpdateUser(modifiedUser);
        }

        public IUserDTO Login(IUserDTO user) {
            if (user.Mail == null || user.Password == null)
                throw new MissingMandatoryInformationException("Le mail ou le mot de passe est manquant");
            
            IUserDTO userFromDB = _userDAO.GetUserByMail(user.Mail);
            if (userFromDB.IsBanned.Value)
                throw new UnauthorizedException("Vous êtes banni");

            if (BCrypt.Net.BCrypt.Verify(user.Password, userFromDB.Password))
            {
                return userFromDB;
            }
            else
            {
                throw new UnauthorizedAccessException("mail ou mot de passe errone ");
            }
        }
        public void AddRating(IRatingsDTO ratings) {
            if (ratings.IdRated == null || ratings.IdRater == null)
                throw new MissingMandatoryInformationException("Des informations obligatoires sont manquantes");
            _ratingsDAO.AddRatings(ratings);


            IUserDTO modifiedUser = _userDAO.GetUserById(ratings.IdRated);
            modifiedUser.Ratings.Add((Ratings)ratings);
            _userDAO.UpdateUser(modifiedUser);
        }

        public void UpdateRatings(IRatingsDTO ratings) {
            if (ratings.IdRated == null || ratings.IdRater == null)
                throw new MissingMandatoryInformationException("Des informations obligatoires sont manquantes");
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

        public IUserDTO SetImageWithPath(UploadFileRequest image, string id) {
            if (image.FileName == null) {
                throw new ArgumentNullException("Il manque le nom du fichier ");
            }
            if (image.FilePath == null) {
                throw new ArgumentNullException("Il manque le chemin vers le fichier");
            }
            IUserDTO user = _userDAO.GetUserById(id);
            _blobServices.UploadFileBlobAsync(image.FilePath, image.FileName, "profilsimages");
            user.Image = "https://blobuploadimage.blob.core.windows.net/profilsimages/" + image.FileName;
            _userDAO.UpdateUser(user);
            return user;
        }

        public IUserDTO SetImageWithContent(UploadContentRequest image, string id) {

            if (image.Content == null) {
                throw new ArgumentNullException("Il manque le contenu de l'image");
            }
            if (image.FileName == null) {
                throw new ArgumentNullException("Il manque le nom du fichier");
            }
            image.Content = image.Content.Substring(image.Content.IndexOf(",") + 1);
            IUserDTO user = _userDAO.GetUserById(id);
            _blobServices.UploadContentBlobAsync(image.Content, image.FileName, "profilsimages");
            user.Image = "https://blobuploadimage.blob.core.windows.net/profilsimages/" + image.FileName;
            _userDAO.UpdateUser(user);
            return user;
        }

        public List<IProductDTO> GetBoughtProduct(string id) {
            IUserDTO userFromDB = _userDAO.GetUserById(id);
            List<IProductDTO> list = new List<IProductDTO>();
            foreach (string productid in userFromDB.Bought) {
                list.Add(_productDAO.GetProductById(productid));
            }
            return list;
        }

        public List<IProductDTO> GetSoldProduct(string id) {
            IUserDTO userFromDB = _userDAO.GetUserById(id);
            List<IProductDTO> list = new List<IProductDTO>();
            foreach (string productid in userFromDB.Sold)
            {
                list.Add(_productDAO.GetProductById(productid));
            }
            return list;
        }

        public List<IProductDTO> GetFavProduct(string id) {
            IUserDTO userFromDB = _userDAO.GetUserById(id);
            List<IProductDTO> list = new List<IProductDTO>();
            foreach (string productid in userFromDB.FavProducts)
            {
                list.Add(_productDAO.GetProductById(productid));
            }
            return list;
        }

        public List<string> GetAllMails()
        {
            List<IUserDTO> users = _userDAO.GetUsers();
            List<string> emails = users.Where(u => !u.IsBanned.Value).Select(u => u.Mail).ToList<string>();
            return emails;
        }

        public List<IProductDTO> GetAllProductsPendingOfSeller(string idSeller)
        {
            IUserDTO seller = _userDAO.GetUserById(idSeller);
            if (seller.IsBanned.Value) throw new UnauthorizedException("L'utilisateur est banni");
            return _userDAO.GetAllProductsPendingOfSeller(idSeller);
        }
    }
}
