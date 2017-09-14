namespace Control
{
    public interface IInputProvider
    {
        float GetAxis(string axisName);

        bool GetButtonDown(string buttonName);

        bool GetButtonUp(string buttonName);
    }
}