using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JamaaHub.Tests.Abstractions
{
    public abstract class BaseIntegrationTest: IAsyncDisposable
    {

        private readonly IServiceProvider _sp;
        protected ISender _mediator;
        public BaseIntegrationTest()
        {
            // Initialize the in-memory database
          
            //create new DI for registering context & settings
            var services = new ServiceCollection();

          
          
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                //cfg.RegisterServicesFromAssembly(typeof(JamaaHubDbContext).Assembly);
            });

            SetupAddtionalServices(services);

            _sp = services.BuildServiceProvider();

            _mediator = _sp.GetRequiredService<ISender>();
            
        }

        protected abstract void SetupAddtionalServices(ServiceCollection services);
        protected abstract ValueTask DisposeOtherResourcesAsync();
        public async ValueTask DisposeAsync()
        {
            await DisposeOtherResourcesAsync();
        }
    }
}
