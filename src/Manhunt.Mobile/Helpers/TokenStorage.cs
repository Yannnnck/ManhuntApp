using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Manhunt.Mobile.Helpers
{
    static class TokenStorage
    {
        const string FileName = "token.json";
        public static string Token { get; private set; }

        public static void Save()
        {
            File.WriteAllText(
              Path.Combine(FileSystem.AppDataDirectory, FileName),
              JsonSerializer.Serialize(Token));
        }

        public static void Load()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, FileName);
            if (File.Exists(path))
                Token = JsonSerializer.Deserialize<string>(File.ReadAllText(path));
        }

        public static void Set(string token)
        {
            Token = token;
            Save();
        }

        public static void Clear()
        {
            Token = null;
            Save();
        }
    }
}

