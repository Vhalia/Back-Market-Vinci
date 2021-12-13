using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Other;
using Microsoft.AspNetCore.Http;
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

        IUserDTO Login(IUserDTO user);

        void AddRating(IRatingsDTO ratings);

        void UpdateRatings(IRatingsDTO ratings);

        void DeleteRatings(string id);

        IUserDTO SetImage(UploadFileRequest image, string id);
    }
}
