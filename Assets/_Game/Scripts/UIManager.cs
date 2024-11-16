using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private Image dashImage;
    [SerializeField] private Player player;

    [SerializeField] TextMeshProUGUI textCoint;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetCooldownOf(dashImage);
        CheckCooldownOf(dashImage, player.GetDashCoundown());
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }
    public void SetCoint(float coin)=>textCoint.text = coin.ToString();

    public void SetCooldownOfDash()=> SetCooldownOf(dashImage);
}