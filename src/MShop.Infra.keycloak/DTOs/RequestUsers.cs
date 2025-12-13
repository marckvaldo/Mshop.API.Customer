using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak.DTOs
{
    public class RequestUsers
    {
        public RequestUsers(string name, string email, string phone, string password)
        {
            this.name = name;
            this.email = email;
            this.phone = phone;
            this.password = password;
        }

        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
    }
}
