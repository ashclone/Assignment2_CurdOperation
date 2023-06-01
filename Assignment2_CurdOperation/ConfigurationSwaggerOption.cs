using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Assignment2_CurdOperation
{
    public class ConfigurationSwaggerOption : IConfigureOptions<SwaggerGenOptions>
    {

        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT First of all Please Enter Keyword " +
                  "Bearer" + "[space] " +
                  "paste your token here and enter",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {{
                new OpenApiSecurityScheme
                {
                    Reference=new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    },
                    Scheme="oauth2",
                    Name="Bearer",
                    In=ParameterLocation.Header,

                },
                new List<string>()
            }});

        }
    }
}
