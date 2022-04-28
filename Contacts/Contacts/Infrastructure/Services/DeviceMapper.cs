using Contacts.Infrastructure.Models;
using System.Text.Json;

namespace Contacts.Infrastructure.Services
{
    public class DeviceMapper
    {
        private List<DeviceModel> _devices;
        private static DeviceMapper _instance;

        public static DeviceMapper GetInstance()
        {
            if (_instance == null)
                _instance = new DeviceMapper();
            return _instance;
        }

        public DeviceMapper()
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("supported_devices.json").Result;
            using var reader = new StreamReader(stream);

            var json = reader.ReadToEnd();

            _devices = JsonSerializer.Deserialize<List<DeviceModel>>(json);

            reader.Close();
        }
        public DeviceModel GetDeviceByModel(string model)
        {
            return _devices.FirstOrDefault(d => d.Model == model);
        }
    }
}