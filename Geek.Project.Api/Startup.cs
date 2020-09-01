using Autofac;
using Geek.Project.Api.Enums;
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
            //应用配置
            services.AddSingleton(_appConfig);
            //数据库
            services.AddDb(_env).Wait();
            services.AddControllers();

            #region Swagger Api文档


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

                #region 添加设置Token的按钮
                //if (_appConfig.IdentityServer.Enable)
                //{
                //    //添加Jwt验证设置
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

                //    //统一认证
                //    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //    {
                //        Type = SecuritySchemeType.OAuth2,
                //        Description = "oauth2登录授权",
                //        Flows = new OpenApiOAuthFlows
                //        {
                //            Implicit = new OpenApiOAuthFlow
                //            {
                //                AuthorizationUrl = new Uri($"{_appConfig.IdentityServer.Url}/connect/authorize"),
                //                Scopes = new Dictionary<string, string>
                //                    {
                //                        { "admin.server.api", "admin后端api" }
                //                    }
                //            }
                //        }
                //    });
                //}
                //else
                //{
                //    //添加Jwt验证设置
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

            #region Swagger Api文档

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Geek.Project.Api {version}");
                });

                c.RoutePrefix = "";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                                                                                       //c.DefaultModelsExpandDepth(-1);//不显示Models
                });

            #endregion
        }

        #region 依赖注入

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
