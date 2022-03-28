using Config.Net;
using HWRDemo.Model;

namespace HWRDemo.Utility;

internal class ConfigUtility
{
    public ConfigModel config;

    public ConfigUtility()
    {
        config = new ConfigurationBuilder<ConfigModel>().UseIniFile("Setting.ini").Build();
    }
}