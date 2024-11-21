using JetBrains.Annotations;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    Player player;
    [Header("BlackHole")]
    [SerializeField] private float timerBlackhole;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shirinkSpeed;

    [SerializeField] private bool canAttack;
    [SerializeField] private bool canShrink;

    [Header("CloneAttack")]
    [SerializeField] private CloneAttack[] cloneAttackPrefabs;
    [SerializeField] private float spawnCloneAttackCoundown;
    [SerializeField] private float countCloneAttack;
    [SerializeField] private bool isRandomColor;
    [SerializeField] private Color[] colors;
    private float timerSpawnCloneAttack;
    private Enemy target;

    void Start()
    {

    }

    void Update()
    {
        timerSpawnCloneAttack -= Time.deltaTime;
        timerBlackhole -= Time.deltaTime;
        if (!canShrink && !canAttack && timerBlackhole <= 0)
            canShrink = true;
        if (canShrink)
        {
            if(target != null)
                target.Stun(false);
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shirinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                player.SetEndBlackHole();
                Destroy(this.gameObject);
            }
        }
        else
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canAttack)
            SpawnCloneAttacK();
    }

    private void SpawnCloneAttacK()
    {
        if (countCloneAttack <= 0)
        {
            canShrink = true;
            return;
        }
        if(timerSpawnCloneAttack>0)
            return;
        countCloneAttack--;
        timerSpawnCloneAttack = spawnCloneAttackCoundown;
        CloneAttack cloneAttack = Instantiate(cloneAttackPrefabs[Random.Range(0,cloneAttackPrefabs.Length)], target.transform.position, Quaternion.identity);
        if(colors.Length > 0&&isRandomColor)
            cloneAttack.Set_Sr(colors[Random.Range(0,colors.Length)]);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            target = collision.GetComponent<Enemy>();
            target.Stun(true);
            canAttack = true;
        }
    }

    public void SetUp(Player player)
    {
        this.player = player;
    }

}
