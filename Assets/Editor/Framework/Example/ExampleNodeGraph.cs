using Framework.Example;
using System;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace GameEditor.Example
{
    [Serializable]
    public class ExampleNodeGraph : NodeGraph<ExampleEventCfg, ExampleConditionCfg, ExampleActionCfg>
    {
        public static ExampleNodeGraph instance;

        //[MenuItem("Editor/ExampleNodeGraph")]
        public static void OpenNodeGraphExampleEditor()
        {
            ExampleNodeGraph graph = EditorHelper.ReadOrCreateScriptableObject<ExampleNodeGraph>("Assets/EditorData/ExampleNodeGraph.asset");
            NodeEditorWindow.Open(graph);
            instance = graph;
        }
    }

    [CustomNodeGraphEditor(typeof(ExampleNodeGraph))]
    public class ExampleNodeGraphEditor : NodeGraphEditor
    {
        public override void OnGUI()
        {
            base.OnGUI();

            Rect rect = new(window.position.width - 100, 5, 80, 20);
            GUI.backgroundColor = Color.green;
            if (GUI.Button(rect, "导出数据"))
            {
                try
                {
                    ExampleNodeCfg cfg = new();
                    cfg.nodes = (target as ExampleNodeGraph).ToRuntimeCfg();
                    EditorHelper.SaveJson(cfg, ExampleNodeCfg.PATH);
                    EditorUtility.DisplayDialog("提示", $"保存成功: {ExampleNodeCfg.PATH}", "确认");
                }
                catch (Exception ex)
                {
                    EditorUtility.DisplayDialog("警告", "保存失败", "确认");
                    Debug.LogError(ex);
                }
            }
        }
    }
}