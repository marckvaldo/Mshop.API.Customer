using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak.DTOs
{
    public class ResultUser
    {
        public string Id { get; private set; }
        public string UserName { get; private set; }= string.Empty;
        public string Email { get; private set; }= string.Empty;
       
        public ResultUser(string id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }

        public static ResultUser Result(string id, string userName, string email) => new ResultUser(id, userName, email);

    }
}
