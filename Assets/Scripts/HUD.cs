using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI ballsCount;

    public void UpdateballsCount(int count)
    {
        ballsCount.text = "" + count;
    }
}
