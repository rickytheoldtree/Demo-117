using Newtonsoft.Json;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.Services
{
    public interface IPrefService : IService
    {
        // 获取字符串
        string GetString(string key, string defaultValue = "");
        // 设置字符串
        void SetString(string key, string value);
        // 获取对象
        T GetObject<T>(string key, T defaultValue = default, params JsonConverter[] converters);
        // 设置对象
        void SetObject<T>(string key, T gameSave, params JsonConverter[] converters);
        // 获取bool值
        bool GetBool(string key, bool defaultValue = false);
        // 设置bool值
        void SetBool(string key, bool value);
        // 获取长整型值
        long GetLong(string key, long defaultValue = 0);
        // 设置长整型值
        void SetLong(string key, long value);
        int GetInt(string key, int value);
        void SetInt(string key, int value = 0);
    }
    public class PlayerPrefService : AbstractService, IPrefService
    {
        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public T GetObject<T>(string key, T defaultValue,  params JsonConverter[] converters)
        {
            if (!PlayerPrefs.HasKey(key)) return defaultValue;
            try
            {
                var json = PlayerPrefs.GetString(key, "");
                var obj = JsonConvert.DeserializeObject<T>(json, converters);
                return obj != null ? obj : defaultValue;
            }
            catch
            {
                Debug.LogWarning($"Failed to parse {key}");
                return defaultValue;
            }
        }

        public void SetObject<T>(string key, T gameSave, params JsonConverter[] converters)
        {
            var json = JsonConvert.SerializeObject(gameSave, converters);
            PlayerPrefs.SetString(key, json);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public long GetLong(string key, long defaultValue = 0)
        {
            var value = PlayerPrefs.GetString(key);
            return long.TryParse(value, out var result) ? result : defaultValue;
        }

        public void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }
    }
}