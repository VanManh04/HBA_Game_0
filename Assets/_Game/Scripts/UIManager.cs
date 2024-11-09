using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] TextMeshProUGUI textCoint;
    public void SetCoint(float coin)
    {
        textCoint.text = coin.ToString();
    }
}