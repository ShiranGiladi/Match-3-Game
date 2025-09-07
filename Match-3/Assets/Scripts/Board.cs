using NUnit.Framework;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] dotsPrefab;
    private BackgroundTile[,] tiles;
    public GameObject[,] dots;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiles = new BackgroundTile[width, height];
        dots = new GameObject[width, height];
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUp()
    {
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                Vector2 position = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, position, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                int dotToUse = Random.Range(0, dotsPrefab.Length);
                GameObject dot = Instantiate(dotsPrefab[dotToUse], position, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";

                dots[i, j] = dot;
            } 
        }
    }
}
