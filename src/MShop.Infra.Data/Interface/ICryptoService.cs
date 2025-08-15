using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Data.Interface
{
    public interface ICryptoService
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);
    }
}
