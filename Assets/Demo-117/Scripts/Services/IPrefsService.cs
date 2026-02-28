using System;
using Newtonsoft.Json;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.Services
{
    public interface IPrefsService : IService
    {
        bool HasKey(string key);
        void SetInt(string key, int value);
        void SetLong(string key, long value);
        void SetString(string key, string value);
        void SetFloat(string key, float value);
        void SetBool(string key, bool value);
        void SetEnum<T>(string key, T value);
        void Set<T>(string key, T value);
        int GetInt(string key, int defaultValue = 0);
        long GetLong(string key, long defaultValue = 0);
        string GetString(string key, string defaultValue = "");
        float GetFloat(string key, float defaultValue = 0);
        bool GetBool(string key, bool defaultValue = false);
        T GetEnum<T>(string key, T defaultValue = default) where T : Enum;
        T Get<T>(string key, T defaultValue = null) where T : class;
        
        void Save();
    }
    public class PlayerPrefsService : AbstractService, IPrefsService
    {
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public void SetEnum<T>(string key, T value)
        {
            PlayerPrefs.SetInt(key, (int)(object)value);
        }

        public void Set<T>(string key, T value)
        {
            switch (value)
            {
                default:
                    var json = JsonConvert.SerializeObject(value);
                    PlayerPrefs.SetString(key, json);
                    break;
                case int i:
                    SetInt(key, i);
                    break;
                case string str:
                    SetString(key, str);
                    break;
                case float f:
                    SetFloat(key, f);
                    break;
                case bool b:
                    SetBool(key, b);
                    break;
                case Enum e:
                    SetEnum(key, e);
                    break;
            }
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public long GetLong(string key, long defaultValue = 0)
        {
            string str = PlayerPrefs.GetString(key, defaultValue.ToString());
            return long.TryParse(str, out var result) ? result : defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public T GetEnum<T>(string key, T defaultValue = default) where T : Enum
        {
            return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
        }

        public T Get<T>(string key, T defaultValue = null) where T : class
        {
            var json = PlayerPrefs.GetString(key, "");
            return json == "" ? defaultValue : JsonConvert.DeserializeObject<T>(json);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}