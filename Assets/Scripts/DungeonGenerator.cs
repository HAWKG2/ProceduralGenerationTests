using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool Visited = false;
        public bool[] status = new bool[4];
    }
    public Vector2 size;
    public int startPos = 0;
    public GameObject room;
    public Vector2 offset;

    List<Cell> board;
    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenerateDungeon()
    {
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                var newDungeonRoom = Instantiate(room, new Vector3(j * offset.x, 0, i * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                Debug.Log("length" + board[Mathf.FloorToInt(i + j * size.x)].status.Length);    
                newDungeonRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status);
                newDungeonRoom.name = (i + "Room" + "-" + "Variant " + j);
                
            }
        }
    }
    void MazeGenerator()
    {
        board = new List<Cell>();

        // Fill up the board with the tiles
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }
        Debug.Log(board.Count);
        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while(k < 3000)
        {
            k++;

            board[currentCell].Visited = true;

            List<int> cellNeighbours = CheckNeighbours(currentCell);
            Debug.Log("Current cell = " + currentCell);
            foreach(int x in cellNeighbours)
            {
                Debug.Log("Neighbour = " + x);
            }

            if(cellNeighbours.Count == 0)
            {
                if(path.Count == 0)
                {
                    Debug.Log("breaking out");
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                    Debug.Log("popping " + currentCell);
                }
            }
            else
            {
                path.Push(currentCell);

                int randomCell = cellNeighbours[Random.Range(0,cellNeighbours.Count)];
                Debug.Log("picked cell " + randomCell);

                // path will go down or right
                if(randomCell > currentCell)
                {
                    if(randomCell -1 == currentCell)
                    {   
                        // right wall
                        board[currentCell].status[2] = true;
                        currentCell = randomCell;
                        // left wall
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // lower wall
                        board[currentCell].status[1] = true;
                        currentCell = randomCell;
                        // upper wall
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // path will go up or left
                    if (randomCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = randomCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = randomCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }

        }

        GenerateDungeon();
    }
    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        // check upper neighbour of current cell
        if(cell - size.x >= 0 && !board[Mathf.FloorToInt(cell-size.x)].Visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }
        // check down neighbour of current cell
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell+size.x)].Visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }

        // check right neighbour of current cell
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell+1)].Visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }
        // check left neighbour of current cell
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].Visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbours;
    }
}
