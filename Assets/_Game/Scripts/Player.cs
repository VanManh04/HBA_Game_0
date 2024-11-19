using UnityEngine;

public class Player : Character
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 35;

    [Header("Throw")]
    [SerializeField] private Kunai kunaiPrefabs;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwCoundown;
    private float lastTimeThrow;

    [SerializeField] private GameObject attackArea;

    [Header("Dash")]
    [SerializeField] private CloneDashPlayer spriteDashPrefabs;
    [SerializeField] private float dashTimer;
    [SerializeField] private float timerSpawnSpriteDashCoundown;
    [SerializeField] private float dashCoundown;
    [SerializeField] private float dashSpeed;
    private float lastTimeDash;
    private float lastTimeSpawnSpriteDash;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDash = false;

    private bool isDeath = false;

    private float horizontal;


    [SerializeField] private int coin = 0;

    private Vector3 savePoint;
    protected override void Start()
    {
        base.Start();
        coin = PlayerPrefs.GetInt("Coin", coin);
        UIManager.instance.SetCoint(coin);
    }

    //private void FixedUpdate()
    void Update()
    {
        lastTimeThrow -= Time.deltaTime;
        lastTimeDash -= Time.deltaTime;
        isGrounded = CheckGrounded();

        if (IsDeath)
            return;

        if (isDash)
        {
            rb.velocity = new Vector2(dashSpeed * (transform.rotation.y == 0 ? 1 : -1), rb.velocity.y);
            SpawnSpriteDash();
            //Debug.Log(dashSpeed * (transform.rotation.y == 0 ? 1 : -1));
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash)
            Dash();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            horizontal = Input.GetAxisRaw("Horizontal");
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            horizontal = 0;

        //if (isAttack)
        //{
        //    rb.velocity = Vector2.zero;
        //    return;
        //}

        CheckAction();
    }

    private void CheckAction()
    {
        if (isGrounded)
        {
            //jump
            if (isJumping)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            //changeAnim Run
            if (Mathf.Abs(horizontal) > .1f)
            {
                if (!isAttack)
                    ChangeAnim("Run");
            }
            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //chawck fall
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }

        //moving
        if (Mathf.Abs(horizontal) > .1f)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded && !isJumping)
        {
            ChangeAnim("Idle");
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        SetZeroVelocity();
        horizontal = 0;
        isAttack = false;
        lastTimeDash = dashCoundown;

        transform.position = savePoint;
        SavePoint();
        ChangeAnim("Idle");
        ActiveAttackFalse();
        UIManager.instance.SetCoint(coin);
        UIManager.instance.SetCooldownOfDash();
        UIManager.instance.SetCooldownOfThrow();
    }

    public override void OnDesPawn()
    {
        base.OnDesPawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.15f, Color.yellow);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.15f, groundLayer);
        return hit.collider != null;
    }

    #region Action
    public void Attack()
    {
        if (isAttack)
            return;

        isAttack = true;
        ChangeAnim("Attack");
        Invoke(nameof(ResetAttack), .5f);
        ActiveAttackTrue();
        Invoke(nameof(ActiveAttackFalse), .5f);
    }
    public void Throw()
    {
        if (lastTimeThrow >= 0)
            return;

        UIManager.instance.SetCooldownOfThrow();
        isAttack = true;
        ChangeAnim("Throw");
        Invoke(nameof(ResetAttack), .5f);

        Instantiate(kunaiPrefabs, throwPoint.position, throwPoint.rotation);
        lastTimeThrow = throwCoundown;
    }
    public void Jump()
    {
        if (!isGrounded)
            return;
        isJumping = true;
        ChangeAnim("Jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ResetAttack()
    {
        ChangeAnim("Idle");
        isAttack = false;
    }
    #endregion

    #region Dash
    public void Dash()
    {
        if (lastTimeDash >= 0)
            return;

        UIManager.instance.SetCooldownOfDash();
        isDash = true;
        //anim.SetBool("Dash", isDash);
        //ChangeAnim("Dash");

        Invoke(nameof(DashEnd), dashTimer);
    }

    public void DashEnd()
    {
        //Debug.Log("End Dash");
        isDash = false;
        //anim.SetBool("Dash", isDash);
        rb.velocity = Vector2.zero;
        lastTimeDash = dashCoundown;
        //ChangeAnim("Idle");
    }

    public void SpawnSpriteDash()
    {
        if (Time.time >= lastTimeSpawnSpriteDash + timerSpawnSpriteDashCoundown)
        {
            CloneDashPlayer clone = Instantiate(spriteDashPrefabs, transform.position, transform.rotation);
            clone.SetSprite(this.sr);
            // Debug.Log("Spawn Sprite Dash");
            lastTimeSpawnSpriteDash = Time.time;
        }
    }

    #endregion

    public void ActiveAttackTrue() => attackArea.SetActive(true);

    public void ActiveAttackFalse() => attackArea.SetActive(false);

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("Coin", coin);
            UIManager.instance.SetCoint(coin);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DeathZone")
        {
            ChangeAnim("Death");
            Invoke(nameof(OnInit), 1f);
        }
    }

    internal void SavePoint() => savePoint = transform.position;

    public void SetZeroVelocity() => rb.velocity = Vector2.zero;
    public float GetDashCoundown()
    {
        return dashCoundown;
    }

    public float GetThrowCoundown()
    {
        return throwCoundown;
    }
}
