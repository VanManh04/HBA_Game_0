using UnityEngine;

public class Player_AC : MonoBehaviour
{
    [SerializeField] Player player;
    public void SetZeroVelocity() => player.SetZeroVelocity();
}
