using UnityEngine;

public class Character : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;

    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefabs;

    private float hp;
    private string currentAnimName;
    private bool IsNoDamage;

    protected bool IsDeath => hp <= 0;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        OnInit();
    }


    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    public virtual void OnDesPawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("Death");

        Invoke(nameof(OnDesPawn), 2f);
    }

    protected void ChangeAnim(string _name)
    {
        if (currentAnimName != _name)
        {
            anim.ResetTrigger(_name);
            currentAnimName = _name;
            anim.SetTrigger(currentAnimName);
        }
    }


    public void OnHit(float damage)
    {
        if (!IsDeath)
        {
            hp -= damage;
            if (IsDeath)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefabs, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

    public void SetIsNoDamage(bool _bool) => this.IsNoDamage = _bool;
}
