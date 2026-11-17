using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textFields;

    public void SetText(int index, string text)
    {
        textFields[index].text = text;
    }
}
