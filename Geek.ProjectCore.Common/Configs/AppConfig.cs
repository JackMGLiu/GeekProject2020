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
}
