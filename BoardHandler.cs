using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BoardHandler : MonoBehaviour
{
    [Serializable]
    public class Interval
    {
        public int min;
        public int max;

        public Interval (int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    public int row = 10;
    public int col = 10;
    public Interval obstacle = new Interval(6, 10);
    public Interval food = new Interval(2, 6);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] obstacleTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;

    private Transform board;
    private List<Vector3> pos = new List<Vector3>();

    void initList()
    {
        pos.Clear();
        for (int x = 1; x < row-1; x++)
        {
            for (int y = 1; y < col-1; y++)
            {
                pos.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void setup()
    {
        board = new GameObject("Board").transform;
        for (int x = -1; x < row+1; x++)
        {
            for (int y = -1; y < col+1; y++)
            {
                GameObject initTile = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == row || y == -1 || y == col)
                    initTile = wallTiles[Random.Range(0, wallTiles.Length)];
                
                GameObject instance = Instantiate(initTile, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(board);
            }
        }
    }

    Vector3 randomPos()
    {
        int random = Random.Range(0, pos.Count);
        Vector3 randomPos = pos[random];
        pos.RemoveAt(random);
        return randomPos;
    }

    void randomLayout(GameObject[] tileArray, int min, int max)
    {
        int objCount = Random.Range(min, max+1);
        for (int i = 0; i < objCount; i++)
            {
                Vector3 randomPosition = randomPos();
                GameObject tile = tileArray[Random.Range(0, tileArray.Length)];
                Instantiate(tile, randomPosition, Quaternion.identity);
            }
    }

    public void levelSetup(int l)
    {
        setup();
        initList();
        randomLayout (obstacleTiles, obstacle.min, obstacle.max);
	    randomLayout (foodTiles, food.min, food.max);
        int enemyCount = (int)Mathf.Log(l, 2f);
        randomLayout (enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(row-1, col-1, 0f), Quaternion.identity);
    }
}
