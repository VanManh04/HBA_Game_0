using UnityEngine;

public class Kunai : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject hit_VFX;
    [SerializeField] private float speedMove;

    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speedMove;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);
            Instantiate(hit_VFX,transform.position, transform.rotation);
            OnDespawn();
        }
    }
}
