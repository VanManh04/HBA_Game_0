using UnityEngine;

public class CloneDashPlayer : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private float colorLoosingSpeed;

    private float cloneTimer;

    void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
        }

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }

    public void SetSprite(SpriteRenderer _sr) => sr.sprite = _sr.sprite;
}
