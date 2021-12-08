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

        [HttpPost]
        [Route("/users")]
        public void PostUser([FromBody] User user)
        {
            Console.WriteLine(user.Name);
        }

        [HttpGet]
        [Route("/users")]
        public List<IUserDTO> GetUsers() {
            return userUCC.GetUsers();
        }

    }
}
