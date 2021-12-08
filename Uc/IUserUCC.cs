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

        User Register(IUserDTO user);
    }
}
