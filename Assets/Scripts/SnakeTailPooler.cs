using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTailPooler : MonoBehaviour
{
    public int snakeTailPoolSize;
    public GameObject snakeTailPrefab;
    public Transform snakeTailsParent;
    public List<SnakeTail> snakeTailPool;

    void Awake()
    {
        FillSnakeTailPool();
        GameManager.Instance.GetSnakePool(snakeTailPool);
    }
    private void FillSnakeTailPool()
    {
        for (int i = 0; i < snakeTailPoolSize; i++)
        {
            GameObject spawnedObj = Instantiate(snakeTailPrefab, snakeTailsParent);
            SnakeTail obj = spawnedObj.GetComponent<SnakeTail>();
            snakeTailPool.Add(obj);
        }
    }
}
