using ApiApplication.Application.Commands;
using ApiApplication.Application.Queries;
using ApiApplication.Configs;
using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Infrastructure;
using ApiApplication.Models.DTO;
using ApiApplication.Options;
using ApiApplication.Services;

namespace ApiApplication.Extensions
{
    public static class WebBuilderExtensions
    {
        #region Public methods

        public static WebApplicationBuilder ConfigureApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>()
                .AddValidation<CreateShowtimeCommand, ShowtimeDTO>()
                .AddValidation<GetShowtimeQuery, ShowtimeDTO>()
                .AddValidation<GetTicketsForShowtimeQuery, IEnumerable<TicketDTO>>()
                .AddValidation<CreateReservationCommand, TicketDTO>()
                .AddValidation<ConfirmReservationCommand, TicketDTO>()
                .AddValidation<GetTicketQuery, TicketDTO>());

            return builder;
        }

        public static WebApplicationBuilder ConfigureInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigureAndValidate<ProvidedApiOptions>(builder.Configuration);
            builder.Services.ConfigureAndValidate<RedisOptions>(builder.Configuration);
            builder.Services.ConfigureAndValidate<DbOptions>(builder.Configuration);

            builder.Services.AddTransient<GrpcExceptionInterceptor>();
            builder.Services.AddTransient<GrpcAuthorizationHeaderInterceptor>();

            builder.Services.AddTransient<IShowtimesRepository, ShowtimesRepository>();
            builder.Services.AddTransient<ITicketsRepository, TicketsRepository>();
            builder.Services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();

            builder.Services.AddTransient<IMovieService, MovieService>();

            builder.Services.AddGrpcClient<MoviesApi.MoviesApiClient>((services, options) =>
            {
                var url = services.GetRequiredService<IOptions<ProvidedApiOptions>>().Value.GrpcUrl;
                options.Address = new Uri(url);
            }).AddInterceptor<GrpcAuthorizationHeaderInterceptor>().AddInterceptor<GrpcExceptionInterceptor>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            builder.Services.AddSingleton(services =>
            {
                var settings = services.GetRequiredService<IOptions<RedisOptions>>().Value;
                var configuration = ConfigurationOptions.Parse(settings.Configuration, true);

                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddDbContext<CinemaContext>((services, options) =>
            {
                var settings = services.GetRequiredService<IOptions<DbOptions>>().Value;

                options.UseInMemoryDatabase(settings.Name)
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            return builder;
        }

        #endregion
    }
}