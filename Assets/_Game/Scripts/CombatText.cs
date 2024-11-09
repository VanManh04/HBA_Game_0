using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    public void OnInit(float damage)
    {
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    private void OnDespawn()
    {
        Destroy(this.gameObject);
    }
}
