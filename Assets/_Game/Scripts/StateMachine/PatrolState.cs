using UnityEngine;

public class PatrolState : IState
{
    float randomTime;
    float timer;

    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);

        //enemy.StopMoving();
    }

    public void OnExecute(Enemy enemy)
    {
        Debug.Log("Patrol");
        timer += Time.deltaTime;

        if (enemy.Target != null)
        {
            enemy.changeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            //Debug.Log(enemy.IsTargetRange());
            if (enemy.IsTargetRange())
                enemy.ChangeState(new Attack_State());
            else
                enemy.Moving();

        }
        else
        {
            if (timer < randomTime)
                enemy.Moving();
            else
                enemy.ChangeState(new Idle_State());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}