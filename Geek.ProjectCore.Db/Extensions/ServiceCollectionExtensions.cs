using FreeSql;
using Geek.ProjectCore.Common.Configs;
using Geek.ProjectCore.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Geek.ProjectCore.Db.Extensions
{
    public static  class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据库服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public async static Task AddDb(this IServiceCollection services, IHostEnvironment env)
        {
            var dbConfig = new ConfigHelper().Get<DbConfig>("DbConfig", env.EnvironmentName);

            var freeSqlBuilder = new FreeSqlBuilder()
               .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
               .UseAutoSyncStructure(dbConfig.SyncStructure)
               .UseLazyLoading(false)
               .UseNoneCommandParameter(true);

            var fsql = freeSqlBuilder.Build();
            //fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);
            //services.AddFreeRepository(filter => filter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false));
            services.AddScoped<UnitOfWorkManager>();
            services.AddSingleton(fsql);
        }
    }
}
