using Polly.CircuitBreaker;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak.CircuitBreaker
{
    public class CircuitBreaker : ICircuitBreaker
    {
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        public void Start(Func<Exception, bool> exceptionPredicate, int allowBeforeBreaking, TimeSpan durationOfBreak)
        {
            _circuitBreakerPolicy = Policy
                //.Handle<RedisConnectionException>() // Exceção específica
                //.Or<Exception>() // Geral, caso necessário
                .Handle<Exception>(exceptionPredicate)
                .CircuitBreakerAsync(allowBeforeBreaking, durationOfBreak); // 1 erro em 30 segundos
        }
        public async Task<T> ExecuteActinAsync<T>(Func<Task<T>> action)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(action);    
        }

    }
}
