using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Overworld grid;
    public static (int x, int y) playerAt = (0, 0);
    public static List<(int x, int y)> directions;
    public static List<Living> followers;
    public static CarTile car;
    // Add enum for object and elements

    // Start is called before the first frame update
    void Awake()
    {
        directions = new List<(int x, int y)>();
        followers = new List<Living> { new Follower(1, 0, false), new Follower(2, 0, false), new Follower(3, 0, false) };
        grid = GameObject.Find("Overworld").GetComponent<Overworld>();
        car = new CarTile(3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Move())
        {
            char[,] currentGrid = grid.getGrid();
            
            // Test if valid move
            for (int i = 0; i < followers.Count; i++)
            {
/*                (int x, int y) next = followers[i].Move(directions[directions.Count - i - 1]);
*/              grid.Move(followers[i].position, directions[directions.Count - i - 1], 'f');
                followers[i].position = directions[directions.Count - i - 1];

            }

            car.countDown();
            Debug.Log(car.countdown);
            if (car.gone && car.countdown == 0) {
                grid.kill(car.yPos);
            }

        }
        //print(string.Join(",", directions));
    }

    bool Move()
    {
        bool hasMoved = false;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        (int x, int y) temp = playerAt;
        if (Input.GetButtonDown("Vertical"))
        {
            if (moveVertical < 0 && playerAt.x < grid.height - 1) // South
            {
                temp = (playerAt.x + 1, playerAt.y);
            }
            else if (moveVertical > 0 && playerAt.x > 0) // North
            {
                temp = (playerAt.x - 1, playerAt.y);
            }
        }
        else if (Input.GetButtonDown("Horizontal"))
        {
            if (moveHorizontal > 0 && playerAt.y < grid.width - 1) // East
            {
                temp = (playerAt.x, playerAt.y + 1);
            }
            else if (moveHorizontal < 0 && playerAt.y > 0) // West
            {
                temp = (playerAt.x, playerAt.y - 1);
            }
        }
        if (grid.Move(playerAt, temp, 'c'))
        {
            directions.Add(playerAt);
            playerAt = temp;
            hasMoved = true;

        }
        return hasMoved;
    }

}
