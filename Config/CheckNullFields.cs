using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Config
{
    public static class CheckNullFields <T>
    {
        public static T CheckNull(T datafromFront, T dataFromDB)
        {
            foreach (PropertyInfo pi in datafromFront.GetType().GetProperties())
            {
                var value = pi.GetValue(datafromFront);
                if (value == null)
                {
                    pi.SetValue(datafromFront, pi.GetValue(dataFromDB));
                }

            }
            return datafromFront;
        }
    }
}
