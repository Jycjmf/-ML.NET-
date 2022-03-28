using Config.Net;

namespace HWRDemo.Model;

public interface ConfigModel
{
    [Option(DefaultValue = null)] public string ModelPath { get; set; }

    [Option(DefaultValue = null)] public string ModelTsv { get; set; }

    [Option(DefaultValue = null)] public string TestTsv { get; set; }

    [Option(DefaultValue = null)] public string ImgPath { get; set; }
}