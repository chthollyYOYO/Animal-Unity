namespace Framework
{
    public interface IInputController
    {
        int sortingOrder => 0;

        void OnEnable() { }

        void OnDisable() { }

        void OnInputUpdate(float deltaTime) { }

        void OnGUI() { }
    }
}
