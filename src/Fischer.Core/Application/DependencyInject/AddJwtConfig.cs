using Fischer.Core.Configurations.API;
using Fischer.Core.Configurations.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddJwtConfig
{

    private const string RoleClaimType = "role";
    public static IServiceCollection AddJwt(this IServiceCollection services, ApiConfigurations config,
        IConfiguration configuration)
    {
        if (!config.UseKeyCloak)
        {
            var jwtConfig = configuration.GetSection(typeof(JWTConfig).Name).Get<JWTConfig>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig!.Secret!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer
                };
            });


            return services;
        }
        else
        {

            var keycloakConfig = configuration.GetSection(typeof(KeycloakConfig).Name).Get<KeycloakConfig>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //TODO: Remove this when the Keycloak Server has a valid certificate.
                    options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                    options.IncludeErrorDetails = true;
                    options.Authority = keycloakConfig!.JwtAuthority;
                    options.Audience = keycloakConfig.JwtAudience;
                    options.MetadataAddress = keycloakConfig!.MetadataAddress!;


                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = keycloakConfig.JwtAudience,
                        RoleClaimType = RoleClaimType,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        CryptoProviderFactory = new CryptoProviderFactory()
                        {
                            CacheSignatureProviders = false
                        }
                    };
                });

            services.AddTransient<IClaimsTransformation>(_ =>
                new KeycloakRolesClaimsTransformation(RoleClaimType, keycloakConfig!.JwtAudience!));

            // Configure authorization
            services.AddSingleton<IAuthorizationHandler, RptRequirementHandler>();

            services.AddAuthorization(options =>
            {
                #region Rpt Requirements

                options.AddPolicy("Fischer#FullAccess"
                    , builder => builder.RequireClaim("authorization").Requirements.Add(new RptRequirement("Fischer", "FullAccess"))
                );
                options.AddPolicy("Fischer#read"
                    , builder => builder.AddRequirements(new RptRequirement("Fischer", "read"))
                );
                options.AddPolicy("Fischer#delete"
                    , builder => builder.AddRequirements(new RptRequirement("Fischer", "delete"))
                );
                #endregion
            });

            return services;
        }
    }

}