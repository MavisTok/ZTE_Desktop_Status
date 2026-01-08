namespace ZTE.Models
{
    /// <summary>
    /// 电源模式信息
    /// </summary>
    public class PowerModeInfo
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 直接供电模式启用：1-启用，0-禁用
        /// </summary>
        public string Enable { get; set; }
    }
}
