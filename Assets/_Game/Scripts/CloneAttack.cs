using UnityEngine;

public class CloneAttack : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;

    void Start()
    {
        anim.SetBool("Random", Random.Range(0, 3) > 0 ? true : false);
    }
    public void Set_Sr(Color color) => sr.color = color;
}
