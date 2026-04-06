using UnityEngine;

public class InputGiroscopeExemple : MonoBehaviour
{
    Gyroscope giroscope;

    private void Start()
    {
        giroscope = Input.gyro;
        giroscope.enabled = true;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 60;
        GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.8f, Screen.height * 0.1f), "Giroscope rotation rate " + giroscope.rotationRate, style);

    }
}
