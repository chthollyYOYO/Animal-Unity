using FairyGUI;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class InputController_QTE : IInputController
    {
        public UnityAction<KeyCode> onKeyDown;

        public virtual void OnGUI()
        {
            if (Event.current == null)
                return;

            if (InputTextField.EatKeyEvent(Event.current))
                return;

            if (Event.current.keyCode == KeyCode.None)
                return;

            if (Event.current.rawType == EventType.KeyDown)
            {
                onKeyDown?.Invoke(Event.current.keyCode);
            }
        }
    }
}