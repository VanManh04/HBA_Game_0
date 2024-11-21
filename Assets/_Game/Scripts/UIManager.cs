using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private Player player;
    [Header("Coundown Dash")]
    [SerializeField] private Image dashImage;

    [Header("Coundown Throw")]
    [SerializeField] private Image throwImage;

    [Header("Coundown BlackHole")]
    [SerializeField] private Image BlackHoleImage;

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
        if (Input.GetKeyDown(KeyCode.C))
            SetCooldownOf(throwImage);
        CheckCooldownOf(throwImage, player.GetThrowCoundown());
        if (Input.GetKeyDown(KeyCode.R))
            SetCooldownOf(BlackHoleImage);
        CheckCooldownOf(BlackHoleImage, player.GetBlackHoleCoundown());
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
    public void SetCoint(float coin) => textCoint.text = coin.ToString();

    public void SetCooldownOfDash() => SetCooldownOf(dashImage);
    public void SetCooldownOfThrow() => SetCooldownOf(throwImage);
    public void SetCooldownOfBlackHole() => SetCooldownOf(BlackHoleImage);
}