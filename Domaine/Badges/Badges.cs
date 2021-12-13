using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine
{
    public class Badges : IBadgesDTO
    {
        public string Image { get; set; }

        public bool IsUnlocked { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Badges(string Image, bool IsUnlocked, string Title, string Description) {
            this.Image = Image;
            this.IsUnlocked = IsUnlocked;
            this.Title = Title;
            this.Description = Description;
        
        }
    }
}
