using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CodeGenerator;

public class Appsettings
{
    /// <summary>
    /// 设置配置
    /// </summary>
    public static IConfiguration Configuration { get; set; } = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .Add(new JsonConfigurationSource
    {
        Path = "appsettings.json",
        Optional = false,
        ReloadOnChange = true
    })
    .Add(new JsonConfigurationSource
    {
        Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
        Optional = true,
        ReloadOnChange = true
    })
    .Build();

    /// <summary>
    /// 获取配置信息字符
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static string ConfigString(params string[] sections)
    {
        try
        {
            if (sections.Any())
            {
                return Configuration[string.Join(":", sections)];
            }
        }
        catch (Exception) { }

        return string.Empty;
    }

    /// <summary>
    /// 获取配置信息对象
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static T? ConfigObject<T>(params string[] sections)
    {
        try
        {
            if (sections.Any())
            {
                return Configuration.GetSection(string.Join(":", sections)).Get<T>();
            }
        }
        catch (Exception) { }

        return default(T);
    }
    /// <summary>
    /// 获取配置信息数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sections"></param>
    /// <returns></returns>
    public static List<T> ConfigList<T>(params string[] sections)
    {
        List<T> list = new List<T>();
        Configuration.Bind(string.Join(":", sections), list);
        return list;
    }
}
