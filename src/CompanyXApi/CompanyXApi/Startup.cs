using System.Collections.Generic;
using System.IO;
using CompanyX.Api.Infrastructure.Filters;
using CompanyX.Api.Infrastructure.JsonHelper;
using CompanyX.Api.Infrastructure.Middleware;
using CompanyX.Base.Config;
using CompanyX.Dal;
using CompanyX.Dal.LineItems;
using CompanyX.Dal.Orders;
using CompanyX.Domain.LineItems;
using CompanyX.Domain.Orders;
using CompanyX.Resource;
using CompanyX.Services;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using WebApiContrib.Core;

namespace CompanyX.Api
{
    public class Startup
    {
        /// <summary>
        /// Project startup entry
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                // now each route on every controller gets an "api/v{apiVersion}" prefix
                opt.UseGlobalRoutePrefix(new RouteAttribute("api/v{version:apiVersion}"));
            });

            services.Configure<AppOptions>(Configuration);

            //Using in-memory database

            // Add MVC Core
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV")
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                options.SerializerSettings.Converters = new List<JsonConverter> {new LineItemModelConvertor()};
            });


            #region Swagger
            services.AddSwaggerGen(options =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // integrate xml comments
                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();
                var docFile = Path.Combine(this.HostingEnvironment.ContentRootPath, "bin", "WingsOn.Api.xml");
                if (File.Exists(docFile))
                    options.IncludeXmlComments(docFile);
            });
            #endregion

            #region API Version
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
            });
            #endregion

            #region Custom services
            services.AddScoped<IRepository<WebsiteDetail>, WebsiteDetailRepository>();
            services.AddScoped<IRepository<AdWordCampaign>, AdWordCampaignRepository>();
            services.AddScoped<IRepository<LineItem>, LineItemRepository>();
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            services.AddScoped<IRepository<AdditionalInfo>, AdditionalInfoRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();

            services.AddScoped<ILineItemService, LineItemService>();
            services.AddScoped<IOrderService, OrderService>();

            #endregion
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="appLifetime"></param>
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            IApiVersionDescriptionProvider provider,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            #region logging and exception
            if (!env.IsProduction())
            {
                loggerFactory.AddSerilog();
                // Ensure any buffered events are sent at shutdown
                appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

                loggerFactory.AddFile(Configuration.GetSection("Logging:Serilog"));
            }
            else
            {
                loggerFactory.AddAzureWebAppDiagnostics();
            }

            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole();
                loggerFactory.AddDebug();
            }
            #endregion

            #region swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
                c.DefaultModelRendering(ModelRendering.Model);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.ShowExtensions();
            });
            #endregion

            #region Middle-ware
            // add middle-ware
            app.UseMiddleware<LogResponseMiddleware>();
            app.UseMiddleware<LogRequestMiddleware>();

            app.UseMiddleware<ErrorWrappingMiddleware>();

            #endregion

            app.UseMvc();
        }


        #region Private methods
        private Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var isDepreciated = description.IsDeprecated ? Global.ApiDepreciated : string.Empty;
            var info = new Info()
            {
                Title = $"{Global.ApiDescription}{isDepreciated}",
                Version = description.ApiVersion.ToString(),
                Description = Global.ApiDescription,
                License = new License() { Name = $"© {Global.Author}" }
            };

            return info;
        }
        #endregion
    }
}
