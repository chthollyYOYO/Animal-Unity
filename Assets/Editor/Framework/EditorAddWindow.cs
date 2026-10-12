using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEngine;

namespace GameEditor
{
    public class EditorAddWindow : OdinEditorWindow
    {
        [LabelText("ID")]
        public int id;

        [LabelText("名字")]
        public new string name;

        [HideInInspector]
        public Action onAdd;

        [Button("新增")]
        private void Add()
        {
            onAdd?.Invoke();
        }

        protected override void Initialize()
        {
            base.Initialize();
            titleContent = new GUIContent("新增窗口");
        }
    }
}
