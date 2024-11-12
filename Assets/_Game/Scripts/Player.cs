using UnityEngine;

public class Player : Character
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 35;

    [SerializeField] private Kunai kunaiPrefabs;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private GameObject attackArea;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    
    private bool isDeath = false;

    private float horizontal;


    [SerializeField] private int coin = 0;

    private Vector3 savePoint;
    protected override void Start()
    {
        base.Start();
        coin = PlayerPrefs.GetInt("Coin",coin);
        UIManager.instance.SetCoint(coin);
    }

    //private void FixedUpdate()
    void Update()
    {
        if (IsDeath)
            return;

        isGrounded = CheckGrounded();
        //horizontal = Input.GetAxisRaw("Horizontal");

        //if (isAttack)
        //{
        //    rb.velocity = Vector2.zero;
        //    return;
        //}

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
            rb.velocity = Vector2.zero;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;
        SavePoint();
        ChangeAnim("Idle");
        ActiveAttackFalse();
        UIManager.instance.SetCoint(coin);
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
    public void Attack()
    {
        isAttack = true;
        ChangeAnim("Attack");
        Invoke(nameof(ResetAttack), .5f);
        ActiveAttackTrue();
        Invoke(nameof(ActiveAttackFalse), .5f);

    }
    public void Throw()
    {
        isAttack = true;
        ChangeAnim("Throw");
        Invoke(nameof(ResetAttack), .5f);

        Instantiate(kunaiPrefabs, throwPoint.position, throwPoint.rotation);
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("Jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ResetAttack()
    {
        ChangeAnim("Idle");
        isAttack = false;
    }

    public void ActiveAttackTrue() => attackArea.SetActive(true);

    public void ActiveAttackFalse()=>attackArea.SetActive(false);

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

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
}
