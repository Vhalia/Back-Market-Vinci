using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Other
{
    public enum States
    {
         Sent,Pending ,Online, Refused
    };

    public enum SentTypes { 
    
        ToGive, ToSell, ToSwap
    };

    public enum Types {
        Book, Tools, Furnitures, Videogames, Computers, Bikes, MusicalInstruments, Films, Toys, Phones, Hardware, Jewellery, Clothes, Sport
    };


}
