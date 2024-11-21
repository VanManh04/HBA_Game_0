using UnityEngine;

public class CloneAttack : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim.SetBool("Random", Random.Range(0, 3) > 0 ? true : false);
    }

    void Update()
    {

    }
}
