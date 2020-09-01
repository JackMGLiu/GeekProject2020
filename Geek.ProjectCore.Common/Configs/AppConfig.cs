namespace Geek.ProjectCore.Common.Configs
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Aop配置
        /// </summary>
        public AopConfig Aop { get; set; } = new AopConfig();

        /// <summary>
        /// 日志配置
        /// </summary>
        public LogConfig Log { get; set; } = new LogConfig();
    }

    /// <summary>
    /// Aop配置
    /// </summary>
    public class AopConfig
    {
        /// <summary>
        /// 事物
        /// </summary>
        public bool Transaction { get; set; } = true;
    }


    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        public bool Operation { get; set; } = true;
    }
}
