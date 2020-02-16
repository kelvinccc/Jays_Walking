using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Overworld : MonoBehaviour
{
    private char[,] gridworld;
    public int height;
    public int width;
    public Text ian;

    // Start is called before the first frame update
    void Start()
    {
        gridworld = new char[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridworld[i, j] = '0';
            }
        }
        gridworld[GameManager.playerAt.x, GameManager.playerAt.y] = 'c';
        for (int i = 0; i < GameManager.followers.Count(); i++)
        {
            gridworld[GameManager.followers[i].position.x, GameManager.followers[i].position.y] = 'f';
        }
        ian.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        ian.text = toString();
    }

    public string toString()
    {
        string tess = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                tess += gridworld[i, j];
            }
            tess += Environment.NewLine;
        }
        return tess;
    }

    public bool Move((int x, int y) prev, (int x, int y) current, char character)
    {
        if (gridworld[current.x, current.y] == '0')
        {
            gridworld[prev.x, prev.y] = '0';
            gridworld[current.x, current.y] = character;
            return true;
        }
        return false;
    }

    public void kill(int col) {
        for (int x = 0; x < height; x++) {
            if (gridworld[x, col] != '0') {
                gridworld[x, col] = 'D';
            }
        }
    }

    public char[,] getGrid() {
        return gridworld
    }
}
