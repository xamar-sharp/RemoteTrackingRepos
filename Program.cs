using System;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace JsonParser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICollection<KeyValuePair<string, string>> collection = new HashSet<KeyValuePair<string, string>>(8, new KeysEqualityComparer());
            string lastKey = null;
            bool isValue = false;
            bool isKey = false;
            using (var reader = File.OpenText("C:\\configuration.json"))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    while (await jsonReader.ReadAsync())
                    {
                        if (isKey)
                        {
                            lastKey = jsonReader.Value.ToString();
                            isKey = false;
                        }
                        if (isValue)
                        {
                            collection.Add(new KeyValuePair<string, string>(lastKey, jsonReader.Value.ToString()));
                            isValue = false;
                        }
                        if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.ToString() == "key")
                        {
                            isKey = true;
                        }
                        if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.ToString() == "value")
                        {
                            isValue = true;
                        }
                    }
                }
            }
            await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(collection));
        }
    }
    internal readonly struct KeysEqualityComparer : IEqualityComparer<KeyValuePair<string, string>>
    {
        public bool Equals(KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
        {
            return pair1.Key == pair2.Key;
        }
        public int GetHashCode(KeyValuePair<string, string> pair)
        {
            return pair.Key.GetHashCode();
        }
    }
    public sealed class GitStore
    {
        public const string ID = "git://";
    }
}
