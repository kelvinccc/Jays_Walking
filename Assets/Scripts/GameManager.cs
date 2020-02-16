﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum Direction {North, East, South, West, None};
    private Overworld grid;
    public static (int x, int y) playerStart = (0, 0);
    public static List<(int x, int y)> directions;
    public static List<Living> followers;
    public static CarTile car;
    // Add enum for object and elements


    // Start is called before the first frame update
    void Awake()
    {
        directions = new List<(int x, int y)>();
        followers = new List<Living> { 
            new Jay(playerStart.x, playerStart.y), 
            new Follower(1, 0, false), 
            new Follower(2, 0, false), 
            new Follower(3, 0, false) };
        grid = GameObject.Find("Overworld").GetComponent<Overworld>();
        for (int i = followers.Count - 2; i >= 0; i--) {
            directions.Add(followers[i].position);
        }
        car = new CarTile(3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Direction moved = Move();
        if (moved != Direction.None)
        {
            // Entering this branch assumes that move is valid.

            Tile[,] currentGrid = grid.getGrid();
            
            // Test if valid move
            for (int i = 1; i < followers.Count; i++)
            {
                /* (int x, int y) next = followers[i].Move(directions[directions.Count - i - 1]);*/
                // don't need to subtract index by 1 because start i at 1
                grid.Move(followers[i].position, directions[directions.Count - i], 'f');
                followers[i].position = directions[directions.Count - i];
                print(followers[i].position);

            }
            directions.Add(followers[0].position);
            directions.RemoveAt(0);

            car.countDown();
            Debug.Log(car.countdown);
            if (car.gone && car.countdown == 0) {
                grid.kill(car.yPos);
            }

        }
        //print(string.Join(",", directions));
    }

    // Returns the direction Jay has moved, else returns null if an invalid move occurs
    Direction Move()
    {
        Direction dir = Direction.None;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        (int x, int y) temp = followers[0].position;  // Jay's position
        if (Input.GetButtonDown("Vertical"))
        {
            if (moveVertical < 0 && temp.x < grid.height - 1)  // South
            {
                temp = (temp.x + 1, temp.y);
                if (grid.TileOccupied(temp)) dir = Direction.South;
            }
            else if (moveVertical > 0 && temp.x > 0)  // North
            {
                temp = (temp.x - 1, temp.y);
                if (grid.TileOccupied(temp)) dir = Direction.North;
            }
        }
        else if (Input.GetButtonDown("Horizontal"))
        {
            if (moveHorizontal > 0 && temp.y < grid.width - 1)  // East
            {
                temp = (temp.x, temp.y + 1);
                if (grid.TileOccupied(temp)) dir = Direction.East;
            }
            else if (moveHorizontal < 0 && temp.y > 0)  // West
            {
                temp = (temp.x, temp.y - 1);
                if (grid.TileOccupied(temp)) dir = Direction.West;
            }
        }

        if (grid.Move(followers[0].position, temp, 'c'))
        {
            followers[0].position = temp;
        }
        print(string.Join(",", directions));
        print(dir);
        return dir;
    }


}
