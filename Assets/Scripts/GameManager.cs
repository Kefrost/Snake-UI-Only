using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int AppleCount => appleCount;
    public List<Vector2Int> SnakeTailPositions { get; private set; }
    public RectTransform snake;
    public List<SnakeTail> snakeTailPool;
    public Sprite straightTail;
    public Sprite curvedTail;
    public Text scoreText;
    private Vector2 defaultPoolLocation;
    public static int appleCount;
    public AudioSource deathSound;
    public AudioSource turnSound;
    public AudioSource scoreSound;

    private void Awake()
    {
        Instance = this;
        snakeTailPool = new List<SnakeTail>();
        SnakeTailPositions = new List<Vector2Int>();
        defaultPoolLocation = new Vector2(-2000, 0);
        appleCount = 0;
    }

    private void Update()
    {
        scoreText.text = $"Score: {appleCount}";
    }

    public Vector2 GetSnakePosition()
    {
        return snake.anchoredPosition;
    }

    public void GetSnakePool(List<SnakeTail> list)
    {
        snakeTailPool = list;
    }

    public void SetTailToPosition(int posX, int posY, float angle, bool isCurved)
    {
        SnakeTail part = snakeTailPool.FirstOrDefault(x => x.rectTransform.anchoredPosition == defaultPoolLocation);
        part.image.sprite = isCurved ? curvedTail : straightTail;
        part.rectTransform.anchoredPosition = new Vector2(posX, posY);
        part.rectTransform.eulerAngles = new Vector3(0, 0, angle);
    }


    public void GetAllPartsBack()
    {
        snakeTailPool.ForEach(x => x.rectTransform.anchoredPosition = defaultPoolLocation);
    }

    public void IncreaseScore()
    {
        scoreSound.Play();
        ++appleCount;
    }

    public void SetSnakeTailsPositions(List<Vector2Int> list)
    {
        SnakeTailPositions = list;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayTurnSound()
    {
        turnSound.Play();
    }
}
