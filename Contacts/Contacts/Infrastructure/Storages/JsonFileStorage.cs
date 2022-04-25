using Contacts.Infrastructure.Models;
using System.Text.Json;

namespace Contacts.Infrastructure.Storages
{
    public class JsonFileStorage<T> : IStorage<T> where T : BaseModel
    {
        private readonly string _path;
        private readonly List<T> _items;

        public JsonFileStorage()
        {
            var stringPath = $"{typeof(T).Name.ToLower()}s.json";
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), stringPath);
            if (File.Exists(_path))
            {
                using var sr = new StreamReader(_path);
                var jsonContent = sr.ReadToEnd();
                sr.Close();
                if (string.IsNullOrEmpty(jsonContent))
                    _items = new List<T>();
                else
                    _items = JsonSerializer.Deserialize<List<T>>(jsonContent);
            }
            else
                _items = new List<T>();
        }

        public T Create(T entity)
        {
            entity.Id = GetNewId();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = GetDeviceInfo();
            _items.Add(entity);
            return entity;
        }

        public List<T> GetAll(int offset, int count)
        {
            return _items.OrderBy(x => x.Id).Skip(offset).Take(count).ToList();
        }

        public T GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Remove(T entity)
        {
            _items.Remove(entity);
        }

        public void Remove(int id)
        {
            var itemToRemove = _items.FirstOrDefault(s => s.Id == id);
            if (itemToRemove == null)
                return;
            _items.Remove(itemToRemove);
        }

        public T Update(T entity)
        {
            var itemToUpdate = _items.FirstOrDefault(x => x.Id == entity.Id);
            _items.Remove(itemToUpdate);
            itemToUpdate.UpdatedAt = DateTime.Now;
            itemToUpdate.UpdatedBy = GetDeviceInfo();
            _items.Add(itemToUpdate);
            return entity;
        }

        public void Dispose()
        {
            Synchronize();
        }

        public void Synchronize()
        {
            using var sw = new StreamWriter(_path);

            var jsonContent = JsonSerializer.Serialize(_items);

            sw.Write(jsonContent);

            sw.Close();
        }

        #region Private

        private int GetNewId()
        {
            if (_items == null || _items.Count == 0)
                return 1;
            else
                return _items.OrderBy(s => s.Id).LastOrDefault().Id + 1;
        }

        private string GetDeviceInfo()
        {
            var device = DeviceInfo.Current;
            return $"{device.Name} ({device.Platform} {device.Version.Major})";
        }

        #endregion
    }
}
