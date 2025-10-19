using Newtonsoft.Json;

namespace OnShelfGTDL.Models
{
    public class DBContext
    {
        private readonly string _filePath;

        public DBContext()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "DBsettings.json");
        }

        public string LoadConfig()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"Configuration file not found at {_filePath}");
            }

            var json = File.ReadAllText(_filePath);

            var config = JsonConvert.DeserializeObject<sqlCfg>(json);

            if (config?.ConnectionStrings == null || string.IsNullOrWhiteSpace(config.ConnectionStrings.LMS))
            {
                throw new Exception("Invalid configuration file or missing connection string.");
            }

            return config.ConnectionStrings.LMS;
        }
    }
}
