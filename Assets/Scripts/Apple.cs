using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Apple : MonoBehaviour
{
    private Vector2Int foodPosition;
    private RectTransform apple;
    private int width = 4;
    private int height = 9;

    private void Start()
    {
        apple = GetComponent<RectTransform>();
        SpawnFood();

    }

    void Update()
    {
        if (foodPosition == GameManager.Instance.GetSnakePosition())
        {
            GameManager.Instance.IncreaseScore();
            SpawnFood();
        }
    }

    private void SpawnFood()
    {
        do
        {
            foodPosition = new Vector2Int(Random.Range(-width, width) * 100, Random.Range(-height, height) * 100);
            apple.anchoredPosition = foodPosition;
        } while (GameManager.Instance.SnakeTailPositions.Any(x => x == foodPosition));
    }
}
