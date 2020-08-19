using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction { LEFT, RIGHT, UP, DOWN}



public class Snake : MonoBehaviour
{
    public int snakeLenght = 0;
    public List<TailPosition> snakeTailPositions;
    public Direction direction;
    private Vector2Int moveDirection;
    private RectTransform rectTransform;
    private Vector2Int pos = new Vector2Int(0, 0);
    private float moveTimer;
    private float moveTimerMax;
    //Touch Controlls
    private float maxSwipeTime;
    private float minSwipeDistance;
    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;
    private float swipeLenght;
    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;


    // Start is called before the first frame update
    void Awake()
    {
        moveTimerMax = 0.2f;
        moveTimer = moveTimerMax;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(pos.x, pos.y);
        moveDirection = new Vector2Int(0, 100);
        snakeTailPositions = new List<TailPosition>();
        direction = Direction.UP;
        maxSwipeTime = 0.5f;
        minSwipeDistance = 50;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.SetSnakeTailsPositions(snakeTailPositions.Select(x => x.position).ToList());

        snakeLenght = GameManager.Instance.AppleCount;

        GetInput();

        Move();

        CheckIfDead();
    }

    private void  CheckIfDead()
    {
        if (snakeTailPositions.Any(x => x.position == rectTransform.anchoredPosition) || (rectTransform.anchoredPosition.x < -500 || rectTransform.anchoredPosition.x > 500) || (rectTransform.anchoredPosition.y > 900 || rectTransform.anchoredPosition.y < -900))
        {
            GameManager.Instance.EndGame();
        }
    }

    private void GetInput()
    {
        SwipeTest();

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameManager.Instance.PlayTurnSound();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection.y != -100)
        {
            
            direction = Direction.UP;
            moveDirection.x = 0;
            moveDirection.y = 100;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection.y != 100)
        {
            direction = Direction.DOWN;
            moveDirection.x = 0;
            moveDirection.y = -100;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection.x != 100)
        {
            direction = Direction.LEFT;
            moveDirection.x = -100;
            moveDirection.y = 0;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection.x != -100)
        {
            direction = Direction.RIGHT;
            moveDirection.x = 100;
            moveDirection.y = 0;
        }
    }

    void SwipeTest()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time;
                startSwipePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEndTime = Time.time;
                endSwipePosition = touch.position;
                swipeTime = swipeEndTime - swipeStartTime;
                swipeLenght = (endSwipePosition - startSwipePosition).magnitude;
                if (swipeTime < maxSwipeTime && swipeLenght > minSwipeDistance)
                {
                    SwipeControll();
                }
            }
        }
    }

    void SwipeControll()
    {
        GameManager.Instance.PlayTurnSound();

        Vector2 distance = endSwipePosition - startSwipePosition;
        float xDistance = Mathf.Abs(distance.x);
        float yDistance = Mathf.Abs(distance.y);
        if (xDistance > yDistance)
        {
            if (distance.x > 0 && moveDirection.x != -100)
            {
                direction = Direction.RIGHT;
                moveDirection.x = 100;
                moveDirection.y = 0;
            }
            else if (distance.x < 0 && moveDirection.x != 100)
            {
                direction = Direction.LEFT;
                moveDirection.x = -100;
                moveDirection.y = 0;
            }
        }
        else if (yDistance > xDistance)
        {
            if (distance.y > 0 && moveDirection.y != -100)
            {
                direction = Direction.UP;
                moveDirection.x = 0;
                moveDirection.y = 100;
            }
            else if (distance.y < 0 && moveDirection.y != 100)
            {
                direction = Direction.DOWN;
                moveDirection.x = 0;
                moveDirection.y = -100;
            }
        }
    }

    private void Move()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveTimerMax)
        {
            GameManager.Instance.GetAllPartsBack();

            moveTimer -= moveTimerMax;

            if (snakeTailPositions.Count < 1)
            {
                snakeTailPositions.Insert(0, new TailPosition(direction, pos, direction));
            }
            else
            {
                snakeTailPositions.Insert(0, new TailPosition(snakeTailPositions[0].direction, pos, direction));
            }

            pos += moveDirection;
        
        if (snakeTailPositions.Count >= snakeLenght + 1)
        {
            snakeTailPositions.RemoveAt(snakeTailPositions.Count - 1);
        }

            for (int i = 0; i < snakeTailPositions.Count; i++)
            {
                TailPosition curretTail = snakeTailPositions[i];

                if (curretTail.direction != curretTail.previousDir)
                {
                    float angle = GetCurvedAngle(curretTail.previousDir, curretTail.direction);

                    Vector2Int snakeMovePos = curretTail.position;

                    GameManager.Instance.SetTailToPosition(snakeMovePos.x, snakeMovePos.y, angle + 180, true);
                }
                else
                {

                    Vector2Int snakeMovePos = curretTail.position;

                    float angle = i == 0 ? GetAngleFromPos(moveDirection) - 90 : snakeMovePos.x == snakeTailPositions[i - 1].position.x ? 0 : 90; 

                    GameManager.Instance.SetTailToPosition(snakeMovePos.x, snakeMovePos.y, angle, false);
                }

            }
        }

        rectTransform.anchoredPosition = new Vector3(pos.x, pos.y);
        rectTransform.eulerAngles = new Vector3(0, 0, GetAngleFromPos(moveDirection) - 90);
    }

    private float GetCurvedAngle(Direction prev, Direction current)
    {
        float angle = 0;
        if (prev == Direction.UP)
        {
            if (current == Direction.LEFT)
            {
                angle = 0;
            }
            else if (current == Direction.RIGHT)
            {
                angle = 90;
            }
        }
        else if (prev == Direction.DOWN)
        {
            if (current == Direction.RIGHT)
            {
                angle = 180;
            }
            else if (current == Direction.LEFT)
            {
                angle = -90;
            }
        }
        else if (prev == Direction.LEFT)
        {
            if (current == Direction.UP)
            {
                angle = 180;
            }
            else if (current == Direction.DOWN)
            {
                angle = 90;
            }
        }
        else if (prev == Direction.RIGHT)
        {
            if (current == Direction.DOWN)
            {
                angle = 0;
            }
            else if (current == Direction.UP)
            {
                angle = -90;
            }
        }

        return angle;
    }

    private float GetAngleFromPos(Vector2Int pos)
    {
        float n = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

    
}

public class TailPosition
{
    public Vector2Int position;
    public Direction direction;
    public Direction previousDir;

    public TailPosition(Direction previousDir, Vector2Int position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
        this.previousDir = previousDir;
    }
}
