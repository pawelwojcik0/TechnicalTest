using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : Singleton<GravityManager>
{
    [SerializeField] private Vector2 InstantiatePositionRange;
    [SerializeField] private float waitTime;

    public int ballsAmount;
    public List<Ball> balls = new List<Ball>();

    private HUD hud;
    private int currentBallsCount;

    public int CurrentBallsCount
    {
        get { return currentBallsCount; }
        set
        {
            currentBallsCount = value;
            hud.UpdateballsCount(currentBallsCount);
        }
    }

    private void Awake()
    {
        hud = FindObjectOfType<HUD>();
        CurrentBallsCount = 0;
    }

    private void Start()
    {
        StartCoroutine(BallsInstantiate());
    }

    IEnumerator BallsInstantiate()
    {
        while (true)
        {
            if (balls.Count < ballsAmount)
            {
                Ball ball = BallObjectPool.Instance.GetBall();
                ball.transform.SetParent(null);
                balls.Add(ball);
                ball.transform.position = new Vector2(Random.Range(InstantiatePositionRange.x, InstantiatePositionRange.y), Random.Range(InstantiatePositionRange.x, InstantiatePositionRange.y));
                ball.gameObject.SetActive(true);
                CurrentBallsCount++;
                yield return new WaitForSeconds(waitTime);
            }

            yield return null;
        }
    }
}

