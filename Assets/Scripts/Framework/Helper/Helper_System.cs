using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework
{
    public static partial class Helper
    {
        public static T CreateSubclass<T>(object obj)
        {
            try
            {
                string sameName = obj.GetType().Name;
                int index = sameName.IndexOf('_');
                string subTypeName = sameName[(index + 1)..];
                string baseName = typeof(T).FullName;
                string subName = $"{baseName}_{subTypeName}";
                Type subType = Type.GetType(subName);
                object instance = Activator.CreateInstance(subType);
                return (T)instance;
            }
            catch (Exception e)
            {
                LogManager.Error($"创建子对象失败！{typeof(T)} {obj.GetType().Name} {e}");
                return default;
            }
        }

        public static IEnumerable<T> GetAllSubclasses<T>(Type baseType)
        {
            try
            {
                var assembly = Assembly.GetAssembly(baseType);
                return assembly.GetTypes()
                    .Where(type => baseType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                    .Select(type => (T)Activator.CreateInstance(type));
            }
            catch (Exception e)
            {
                LogManager.Error($"获取子对象失败！{typeof(T)} {baseType.Name} {e}");
                return default;
            }
        }

        public static ValueDropdownList<T> GetSubclassesDropdownList<T>(Type baseType)
        {
            ValueDropdownList<T> result = new();
            var classes = GetAllSubclasses<T>(baseType);
            foreach (var c in classes)
            {
                var type = c.GetType();
                var attr = type.GetAttribute<LabelTextAttribute>();
                result.Add(attr == null ? type.Name : attr.Text, c);
            }
            return result;
        }

        public static ValueDropdownList<Enum> GetEnumDropdownList(Type enumType)
        {
            ValueDropdownList<Enum> result = new();
            foreach (Enum v in Enum.GetValues(enumType))
            {
                var name = Enum.GetName(enumType, v);
                var field = enumType.GetField(name);
                var attr = field.GetAttribute<LabelTextAttribute>();
                result.Add(attr == null ? name : attr.Text, v);
            }
            return result;
        }

        public static void HandleEnum<T>(Action<T> action) where T : Enum
        {
            foreach (T v in Enum.GetValues(typeof(T)))
                action(v);
        }

        public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    return i;
            }
            return -1;
        }

        public static void Add(this int[] array, int value)
        {
            TryAdd(array, value);
        }

        public static bool TryAdd(this int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0)
                {
                    array[i] = value;
                    return true;
                }
            }
            return false;
        }

        public static int Remove(this int[] array, int index)
        {
            int result = array[index];
            array[index] = 0;
            return result;
        }

        public static List<int> RemoveAll(this int[] array)
        {
            List<int> result = new();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > 0)
                    result.Add(array[i]);
                array[i] = 0;
            }
            return result;
        }
    }
}
