using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak.CircuitBreaker
{
    public interface ICircuitBreaker
    {
        void Start(Func<Exception, bool> exceptionPredicate, int allowBeforeBreaking, TimeSpan durationOfBreak);
        Task<T> ExecuteActinAsync<T>(Func<Task<T>> action);
    }
}
