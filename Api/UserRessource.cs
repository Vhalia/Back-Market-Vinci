using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Uc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Back_Market_Vinci.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserRessource : ControllerBase
    {
        private IUserUCC _userUCC;

        public UserRessource(IUserUCC userUCC) {
            this._userUCC = userUCC;
        }


        [HttpGet]
        [Route("/users")]
        public List<IUserDTO> GetUsers() {
            return _userUCC.GetUsers();
        }

        [HttpGet]
        [Route("/users/{mail}")]
        public IUserDTO GetUserByMail(string mail) {
            return _userUCC.GetUserByMail(mail);
        }

        [HttpPost]
        [Route("/users")]
        public IUserDTO Register(User user) {

            return _userUCC.Register(user);
        }

        [HttpDelete]
        [Route("/users/{id}")]

        public void DeleteUser(string id) {
            _userUCC.DeleteUser(id);
        
        }

        [HttpPatch]
        [Route("/users/{id}")]
        public IUserDTO UpdateUser(User user, string id) {

            return _userUCC.UpdateUser(user, id);
        }

        [HttpPost]
        [Route("/login")]
        public IUserDTO Login(User user) {
            return _userUCC.Login(user);
            
        }

        [HttpPost]
        [Route("/users/ratings")]
        public void AddRating(Ratings ratings) {
            _userUCC.AddRating(ratings);
        }

        [HttpPatch]
        [Route("/users/ratings")]
        public void UpdateRating(Ratings ratings) {
            _userUCC.UpdateRatings(ratings);
        }

        [HttpDelete]
        [Route("/users/ratings/{id}")]
        public void DeleteRatings(string id) {
            _userUCC.DeleteRatings(id);
        }

    }
}
