using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.Common
{
    public class CustomResponse<TResult>
    {
        public TResult Data { get; set; }

        public bool Success { get; set; }

        public CustomResponse(TResult data, bool success)
        {
            Data = data;
            Success = success;
        }

    }
}
