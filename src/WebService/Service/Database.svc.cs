using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Service
{
    public class Database : IDatabase
    {
        public JuiceInfo[] FindSimilar(JuiceInfo juice)
        {
            return new JuiceInfo[]
            {
                new JuiceInfo()
                {
                    Id = 5,
                    Name = "Happy apple",
                    Location = "Warehouse Zeta",
                    Price = 430
                },
                new JuiceInfo()
                {
                    Id = 8,
                    Name = "Wonderful carrot",
                    Location = "Warehouse Lambda",
                    Price = 575
                },
            };
            
        }
    }
}
