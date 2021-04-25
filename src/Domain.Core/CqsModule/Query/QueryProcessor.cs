using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Core.CqsModule.Query
{
    public interface IQueryProcessor
    {
        Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query) 
            where TQuery : IQuery<TResult>;
    }

    public class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            var queryHandler = _serviceProvider.GetServices<IQueryHandler<TQuery, TResult>>()
                .FirstOrDefault();

            if (queryHandler == default)
                throw new ArgumentNullException("");

            return await queryHandler.ExecuteAsync(query);
        }
    }
}