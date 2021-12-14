using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine
{
    public interface IBadgesDTO
    {
        public string Id { get; set; }
        public string Image { get; set; }

        public bool IsUnlocked { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
