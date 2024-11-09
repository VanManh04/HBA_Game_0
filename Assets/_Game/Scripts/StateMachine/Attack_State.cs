using UnityEngine;

public class Attack_State : IState
{
    float timer;

    public void OnEnter(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            enemy.changeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);

            Debug.Log("Attack");
            enemy.StopMovingNoAnim();
            enemy.Attack();
        }

        timer = 0;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}