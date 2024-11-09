using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform aPoint, bPoint;
    [SerializeField] private float speed;

    Vector3 target;

    void Start()
    {
        transform.position = aPoint.position;
        target = bPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, aPoint.position) < .1f)
        {
            target = bPoint.position;
        }else if (Vector2.Distance(transform.position, bPoint.position) < .1f)
        {
            target = aPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
            collision.gameObject.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.transform.SetParent(null);
    }
}
