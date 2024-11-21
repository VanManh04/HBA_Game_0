using UnityEngine;

public class CloneAttack_AC : MonoBehaviour
{
    [SerializeField] private GameObject AttackZone;
    [SerializeField] private GameObject hit_VFX;
    public void Destroys() => Destroy(transform.parent.gameObject);
    public void EnbleAttackZone()
    {
        AttackZone.SetActive(true);
        GameObject a = Instantiate(hit_VFX, transform.position, transform.rotation);
        Destroy(a, 2f);
    }
}
