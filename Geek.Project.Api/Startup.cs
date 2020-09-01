using Autofac;
using Geek.ProjectCore.Common.Configs;
using Geek.ProjectCore.Common.Helpers;
using Geek.ProjectCore.Db.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Geek.Project.Api
{
    public class Startup
    {
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
