using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    private float moveSpeedDefault;

    [SerializeField] private GameObject attackArea;

    private IState currentState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;
    protected override void Start()
    {
        base.Start();
        moveSpeedDefault = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        if (stun)
            return;
        if (currentState != null && !IsDeath)
            currentState.OnExecute(this);
    }

    public override void OnDesPawn()
    {
        base.OnDesPawn();
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new Idle_State());
        ActiveAttackFalse();
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }


    public void ChangeState(IState _newState)
    {
        if (currentState != null)
            currentState.OnExit(this);

        currentState = _newState;

        if (currentState != null)
            currentState.OnEnter(this);

    }

    public void Moving()
    {
        ChangeAnim("Run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("Idle");
        rb.velocity = Vector2.zero;
    }

    public void StopMovingNoAnim() => rb.velocity = Vector2.zero;

    public void Attack()
    {
        ChangeAnim("Attack");
        ActiveAttackTrue();
        Invoke(nameof(ActiveAttackFalse), .5f);
    }

    public bool IsTargetRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) < attackRange)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyWall")
        {
            changeDirection(!isRight);
        }
    }

    public void changeDirection(bool _isRight)
    {
        isRight = _isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    public void ActiveAttackTrue() => attackArea.SetActive(true);

    public void ActiveAttackFalse() => attackArea.SetActive(false);
    internal void SetTarget(Character character)
    {
        this.target = character;

        if (IsTargetRange())
            ChangeState(new Attack_State());
        else if (Target != null)
            ChangeState(new PatrolState());
        else
            ChangeState(new Idle_State());
    }

    public override void Stun(bool _bool)
    {
        base.Stun(_bool);
        moveSpeed = _bool ? 0 : moveSpeedDefault;
    }
}
