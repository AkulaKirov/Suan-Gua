using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    internal class _64Gua
    {
        JObject json64Gua = JObject.Parse(File.ReadAllText("64gua.json"));
        public (string, string) Get(string name)
        {
            var a = (JObject)json64Gua[name[0].ToString()];
            var b = (JObject)a[name[1].ToString()];
            return (b.Value<string>("name"), b.Value<string>("description"));
        }

    }
}
