using cfg;
using Cysharp.Threading.Tasks;
using SimpleJSON;
using System.Collections.Generic;

namespace Framework
{
    public partial class ConfigManager
    {
        /// <summary>
        /// 配置文件目录
        /// </summary>
        public const string CONFIG_ROOT = "Config/Tables";

        private static Tables tables;

        public static async UniTask Init_Tables()
        {
            if (tables == null)
            {
                Dictionary<string, JSONNode> dict = new();
                for (int i = 0; i < Tables.tableNames.Length; i++)
                {
                    var tableName = Tables.tableNames[i];
                    var path = $"{CONFIG_ROOT}/{tableName}";
                    var textAsset = await LoadConfig(path);
                    var json = textAsset.text;
                    var jsonNode = JSON.Parse(json);
                    dict.Add(tableName, jsonNode);
                }

                tables = new(tableName => dict[tableName]);
            }
        }
    }
}
