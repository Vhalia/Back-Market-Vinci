﻿using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Exceptions;
using Back_Market_Vinci.Domaine.Other;
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
        private IMongoCollection<Product> _productsTable;
        private IMongoCollection<Badges> _badgesTable;

        public UserDAO(IDalServices dalServices)
        {
            this._dalServices = dalServices;
            this._usersTable = _dalServices.UsersCollection;
            this._badgesTable = _dalServices.BadgesCollection;
            this._productsTable = _dalServices.ProductsCollection;
        }

        public List<IUserDTO> GetUsers()
        {
            List<IUserDTO> allUsers = _usersTable.AsQueryable().Select(u => new User(u.Id,u.Name, u.Surname,u.Mail,
                u.Campus, u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Bought,
                u.Sold, u.FavTypes, u.FavProducts, u.Badges, u.Image, u.Ratings))
                .ToList<IUserDTO>();
            return allUsers;
        }

        public List<IBadgesDTO> GetBadges() {
            List<IBadgesDTO> allBadges = _badgesTable.AsQueryable().Select(b => new Badges(b.Id, b.Image, b.IsUnlocked,
                b.Title, b.Description)).ToList<IBadgesDTO>();
            return allBadges;
        }

        public IUserDTO GetUserByMail(string mail) {
            IUserDTO user = null;
            try
            {
                user = _usersTable.AsQueryable().Select(u => new User(u.Id, u.Name, u.Surname, u.Mail, u.Campus,
                    u.Password, u.IsBanned.Value, u.IsAdmin.Value, u.Bought,
                u.Sold, u.FavTypes, u.FavProducts, u.Badges, u.Image, u.Ratings))
                    .Where(u => u.Mail.Equals(mail))
                    .Single<IUserDTO>();          
            }
            catch (ArgumentNullException)
            {
                throw new UserNotFoundException("L'utilisateur avec le mail " + mail + " n'a pas été trouvé");
            }
            catch(InvalidOperationException)
            {
                throw new UserNotFoundException("L'utilisateur avec cet mail n'a pas été trouvé");
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
            IUserDTO user = _usersTable.AsQueryable().Select(u => new User(u.Id, u.Name, u.Surname, u.Mail,
                u.Campus, u.Password, u.IsBanned,u.IsAdmin, u.Sold,
                u.Bought, u.FavTypes, u.FavProducts, u.Badges, u.Image, u.Ratings))
                .Where(u => u.Id.Equals(id)).Single<IUserDTO>();
            if (user == null) throw new UserNotFoundException("L'utilisateur avec l'id " + id + " n'a pas été trouvé");
            return user;
        }

        public IUserDTO UpdateUser(IUserDTO modifiedUser) {

            IUserDTO userModified = _usersTable.FindOneAndReplace<User>(u => u.Id.Equals(modifiedUser.Id), (User)modifiedUser);
            if (userModified == null) throw new UserNotFoundException("L'utilisateur avec l'id " + modifiedUser.Id + " n'a pas été trouvé");
            return modifiedUser;

        }

        public List<IProductDTO> GetAllProductsPendingOfSeller(string idSeller)
        {
            List<IProductDTO> productsPending = _productsTable.AsQueryable()
                .Select(p => new Product(p.Id, p.Name, p.State.Value, p.Description, p.IsValidated.Value
                , p.ReasonNotValidated, p.SellerMail, p.SellerId, p.Adress, p.SentType.Value,
                p.Price.Value, p.Type.Value, p.Medias, p.BlobMedias, p.Video, p.BlobVideo))
                .Where(p => p.SellerId.Equals(idSeller) && !p.IsValidated.Value)
                .ToList<IProductDTO>();
            for(var i = 0; i < productsPending.Count; i++)
            {
                if (productsPending[i].State.Value != States.EnAttente)
                {
                    productsPending.RemoveAt(i);
                    i--;
                }
            }
            return productsPending;
        }
    }
}
