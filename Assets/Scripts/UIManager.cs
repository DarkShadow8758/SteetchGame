using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpCollects;
    [SerializeField] private int collects;
    [SerializeField] private TextMeshProUGUI tmpPoints;
    void Update()
    {
        
    }

    public void IncreaseCollects()
    {
        collects += 1;
        tmpCollects.SetText(collects.ToString());
    }
    public void SetPoints(int value)
    {
        tmpPoints.SetText(value.ToString());
    }

}
