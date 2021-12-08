using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Uc
{
    public interface IUserUCC
    {
        List<IUserDTO> GetUsers();

        IUserDTO Register(IUserDTO user);

        IUserDTO GetUserByMail(string mail);

        void DeleteUser(string id);

        IUserDTO UpdateUser(IUserDTO user, string id);
    }
}
