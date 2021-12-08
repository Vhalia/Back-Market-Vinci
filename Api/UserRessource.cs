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
        private IUserUCC userUCC;

        public UserRessource(IUserUCC userUCC) {
            this.userUCC = userUCC;
        }


        [HttpGet]
        [Route("/users")]
        public List<IUserDTO> GetUsers() {
            return userUCC.GetUsers();
        }

        [HttpGet]
        [Route("/users/{mail}")]
        public IUserDTO GetUserByMail(string mail) {
            return userUCC.GetUserByMail(mail);
        }

    }
}
