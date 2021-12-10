using Back_Market_Vinci.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.DataServices
{
    public interface IRatingsDAO
    {
        void AddRatings(IRatingsDTO ratings);

        void UpdateRatings(IRatingsDTO ratings);

        void DeleteRatings(string id);

        public IRatingsDTO GetRatingsById(string id);
    }
}
