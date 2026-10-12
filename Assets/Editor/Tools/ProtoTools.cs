using UnityEditor;
using UnityEngine;

namespace GameTools
{
    public class ProtoTools
    {
        private static readonly string workspace = $"{Application.dataPath}/..";
        private static readonly string protoRoot = $"{workspace}/Protos";
        private static readonly string clientCodeRoot = $"{Application.dataPath}/Scripts/Client/AutoGen/Proto";
        private static readonly string serverCodeRoot = $"{workspace}/ServerProject/Scripts/AutoGen/Proto";
        private static readonly string batPath = $"{workspace}/Tools/Proto/gen.bat";

        [MenuItem("Tools/Proto导出代码")]
        public static void ExportCode()
        {
            ToolsHelper.RunBat(batPath, protoRoot, clientCodeRoot);
            ToolsHelper.SvnCommit(clientCodeRoot, "提交客户端代码");

            ToolsHelper.RunBat(batPath, protoRoot, serverCodeRoot);
            ToolsHelper.SvnCommit(serverCodeRoot, "提交服务器代码");

            ToolsHelper.SvnCommit(protoRoot, "提交协议");
        }
    }
}
