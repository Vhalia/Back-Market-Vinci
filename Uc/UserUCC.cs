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
using Microsoft.AspNetCore.Http;
using Back_Market_Vinci.Domaine.Other;

namespace Back_Market_Vinci.Uc
{
    public class UserUCC : IUserUCC
    {
        private IUserDAO _userDAO;
        private IRatingsDAO _ratingsDAO;
        private IBlobService _blobServices;
        public UserUCC(IUserDAO userDAO, IRatingsDAO ratingsDAO, IBlobService blobServices) {
            this._ratingsDAO = ratingsDAO;
            this._userDAO = userDAO;
            this._blobServices = blobServices;
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
            List<IBadgesDTO> badges = _userDAO.GetBadges();
            user.Ratings = new List<Ratings>();
            user.IsAdmin = false;
            user.IsBanned = false;
            user.FavProduct = new List<Product>();
            user.Bought = new List<Product>();
            user.Sold = new List<Product>();

            user.Badges = badges.ConvertAll(b => (Badges)b);
            user.Badges.ElementAt(0).IsUnlocked = true;
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

        public IUserDTO SetImageWithPath(UploadFileRequest image, string id) {
            if (image.FileName == null) {
                throw new ArgumentNullException("Il manque le nom du fichier ");
            }
            if (image.FilePath == null) {
                throw new ArgumentNullException("Il manque le chemin vers le fichier");
            }
            IUserDTO user = _userDAO.GetUserById(id);
            _blobServices.UploadFileBlobAsync(image.FilePath, image.FileName);
            user.Image = "https://blobuploadimage.blob.core.windows.net/imagecontainer/" + image.FileName;
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
            _blobServices.UploadContentBlobAsync(image.Content, image.FileName);
            user.Image = "https://blobuploadimage.blob.core.windows.net/imagecontainer/" + image.FileName;
            _userDAO.UpdateUser(user);
            return user;
        }
    }
}
