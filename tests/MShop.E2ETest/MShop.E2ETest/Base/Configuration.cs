using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.Base
{
    public class Configuration
    {
        public static string NAME_DATA_BASE = "end2end-test-db";
        public static string URL_API_CUSTOMER = "/api/v1/customer/";
        public static string URL_API_ADDRESS = "/api/v1/address/";
        public static bool DATABASE_MEMORY = false;

        public static string URI_GRAPHQL = "https://localhost:7210/graphql";

        public const string USER_CUSTOMER_AUTH = "dev";
        public const string PASSWORD_CUSTOMER_AUTH = "123456";
        public const string CLIENT_ID_AUTH = "web-app";
    }
}
