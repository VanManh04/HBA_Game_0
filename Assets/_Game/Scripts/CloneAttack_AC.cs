using UnityEngine;

public class CloneAttack_AC : MonoBehaviour
{
    [SerializeField] private GameObject AttackZone;
    public void Destroys() => Destroy(transform.parent.gameObject);
    public void EnbleAttackZone() => AttackZone.SetActive(true);
}
