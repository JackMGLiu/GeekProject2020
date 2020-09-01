using Autofac;
using Geek.Project.Api.Enums;
using Geek.Project.Api.Logs;
using Geek.ProjectCore.Common.Configs;
using Geek.ProjectCore.Common.Helpers;
using Geek.ProjectCore.Db.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Geek.Project.Api
{
    public class Startup
    {
        private static string basePath => AppContext.BaseDirectory;
        public IConfiguration Configuration { get; }
        private readonly IHostEnvironment _env;
        private readonly ConfigHelper _configHelper;
        private readonly AppConfig _appConfig;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _configHelper = new ConfigHelper();
            _appConfig = _configHelper.Get<AppConfig>("AppConfig", env.EnvironmentName) ?? new AppConfig();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Ӧ������
            services.AddSingleton(_appConfig);
            //���ݿ�
            services.AddDb(_env).Wait();
            services.AddControllers();

            #region ������־
            if (_appConfig.Log.Operation)
            {
                services.AddSingleton<ILogHandler, LogHandler>();
            }
            #endregion

            #region Swagger Api�ĵ�


            services.AddSwaggerGen(options =>
            {       
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = "Geek.Project.Api"
                    });
                    //c.OrderActionsBy(o => o.RelativePath);
                });
                
                var xmlPath = Path.Combine(basePath, "Geek.Project.Api.xml");
                options.IncludeXmlComments(xmlPath, true);

                //var xmlCommonPath = Path.Combine(basePath, "Admin.Core.Common.xml");
                //options.IncludeXmlComments(xmlCommonPath, true);

                var xmlModelPath = Path.Combine(basePath, "Geek.Project.Domain.xml");
                options.IncludeXmlComments(xmlModelPath);

                var xmlServicesPath = Path.Combine(basePath, "Geek.Project.Service.xml");
                options.IncludeXmlComments(xmlServicesPath);

                #region �������Token�İ�ť
                //if (_appConfig.IdentityServer.Enable)
                //{
                //    //���Jwt��֤����
                //    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //        {
                //            {
                //                new OpenApiSecurityScheme
                //                {
                //                    Reference = new OpenApiReference
                //                    {
                //                        Id = "oauth2",
                //                        Type = ReferenceType.SecurityScheme
                //                    }
                //                },
                //                new List<string>()
                //            }
                //        });

                //    //ͳһ��֤
                //    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //    {
                //        Type = SecuritySchemeType.OAuth2,
                //        Description = "oauth2��¼��Ȩ",
                //        Flows = new OpenApiOAuthFlows
                //        {
                //            Implicit = new OpenApiOAuthFlow
                //            {
                //                AuthorizationUrl = new Uri($"{_appConfig.IdentityServer.Url}/connect/authorize"),
                //                Scopes = new Dictionary<string, string>
                //                    {
                //                        { "admin.server.api", "admin���api" }
                //                    }
                //            }
                //        }
                //    });
                //}
                //else
                //{
                //    //���Jwt��֤����
                //    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //        {
                //            {
                //                new OpenApiSecurityScheme
                //                {
                //                    Reference = new OpenApiReference
                //                    {
                //                        Id = "Bearer",
                //                        Type = ReferenceType.SecurityScheme
                //                    }
                //                },
                //                new List<string>()
                //            }
                //        });

                //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //    {
                //        Description = "Value: Bearer {token}",
                //        Name = "Authorization",
                //        In = ParameterLocation.Header,
                //        Type = SecuritySchemeType.ApiKey
                //    });
                //}
                #endregion
            });

            #endregion

            //��ֹNLog����״̬��Ϣ
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Swagger Api�ĵ�

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Geek.Project.Api {version}");
                });

                c.RoutePrefix = "";//ֱ�Ӹ�Ŀ¼���ʣ������IIS��������ע�͸���䣬����launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//�۵�Api
                                                                                       //c.DefaultModelsExpandDepth(-1);//����ʾModels
                });

            #endregion
        }

        #region ����ע��

        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region Repository
            var assemblyRepository = Assembly.Load("Geek.Project.Repository");
            builder.RegisterAssemblyTypes(assemblyRepository)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            #endregion

            #region Service
            var assemblyServices = Assembly.Load("Geek.Project.Service");
            builder.RegisterAssemblyTypes(assemblyServices)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            #endregion
        }

        #endregion
    }
}
