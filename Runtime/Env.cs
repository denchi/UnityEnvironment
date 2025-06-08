using System.Linq;

namespace Game.Environment
{
    public static class Env
    {
        // Automatically load environment variables when the class is first accessed
        static Env()
        {
            LoadEnv();
        }

        private static bool LoadEnv()
        {
            var paths = new []
            {
                UnityEngine.Application.streamingAssetsPath,
                UnityEngine.Application.dataPath
            };

            var envFilePath = paths.FirstOrDefault(path =>
            {
                var envFilePath = System.IO.Path.Combine(path, ".env");
                return System.IO.File.Exists(envFilePath);
            });

            if (string.IsNullOrEmpty(envFilePath)) 
                return false;
                
            var lines = System.IO.File.ReadAllLines(envFilePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) 
                    continue;
                    
                var parts = line.Split('=');
                if (parts.Length != 2)
                {
                    UnityEngine.Debug.LogWarning($"Invalid line in .env file: {line}");
                    continue;
                }
                
                System.Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
            }

            return true;
        }
        
        public static string GetEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}