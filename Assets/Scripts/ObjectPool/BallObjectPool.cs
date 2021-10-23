using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObjectPool : Singleton<BallObjectPool>
{
    [SerializeField] private ObjectPool ballsPools;

    private void Awake()
    {
        ballsPools.Initialize();
    }

    public Ball GetBall()
    {
        return ballsPools.GetObject();
    }

    public void ReturnBall(Ball obj)
    {
        ballsPools.ReturnToPool(obj);
    }
}
