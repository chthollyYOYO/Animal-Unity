using System.Collections.Generic;

namespace Framework
{
    public class BoolPlus
    {
        private bool defaultValue;
        private List<string> keys = new();

        public BoolPlus(bool defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public void SetValue(string key, bool value)
        {
            if (value == defaultValue)
            {
                keys.Remove(key);
            }
            else
            {
                if (!keys.Contains(key))
                    keys.Add(key);
            }
        }

        public bool GetValue()
        {
            if (keys.Count > 0)
                return !defaultValue;
            else
                return defaultValue;
        }
    }
}
