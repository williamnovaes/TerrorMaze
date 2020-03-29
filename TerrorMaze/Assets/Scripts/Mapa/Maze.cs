using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
    [System.Serializable]
    public class Cell {
        public bool full = false;
        public bool visited;
        public GameObject north;//1
        public GameObject east;//2
        public GameObject west;//3
        public GameObject south;//4
        public GameObject pos;
    }

    public GameObject wall;
    public GameObject center;


    public float walllength = 64f;
    public int xSize = 4;
    public int ySize = 4;

    private Vector2 initialPos;
    private Vector2 initialPosCenter;
    private GameObject wallHolder;
    private GameObject centerHolder;

    public Cell[] cells;
    private int currentCell=0;
    public int totalCells;
    public int visitedCells = 0;
    private bool stardedBuilding = false;
    private int currentNeighbour = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;

    // Use this for initialization
      public void Start() {
        wallToBreak = 0;
        backingUp = 0;
        currentNeighbour = 0;
        stardedBuilding = false;
        visitedCells = 0;
        currentCell = 0;
        CreateWalls();
    }

    public void CreateWalls() {
        Debug.Log("CreateWalls");
        wallHolder = new GameObject();
        wallHolder.name = "MazeGame";
        centerHolder = new GameObject();
        centerHolder.name = "Center Cells";

        initialPos = new Vector2((-xSize / 2) + walllength / 2, (-ySize / 2) + walllength / 2);
        initialPosCenter= new Vector2(initialPos.x + 28, initialPos.y);
        Vector2 myPos = initialPos;
        Vector2 centerPos = initialPosCenter;
        GameObject tempWall;
        GameObject tempCenter;
        //xAxis
        for (int i = 0; i < ySize; i++) {
            for (int j = 0; j <= xSize; j++) {
                myPos = new Vector2(initialPos.x + (j * walllength) - walllength / 2, initialPos.y + (i * walllength) - walllength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
                
            }
        }
        //yAxis
        for (int i = 0; i <= ySize; i++) {
            for (int j = 0; j < xSize; j++) {
                myPos = new Vector2(initialPos.x + (j * walllength) , initialPos.y + (i * walllength) - walllength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0f, 0f, 90f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }
        //center
        for (int i = 0; i < ySize; i++) {
            for (int j = 0; j < xSize; j++) {
                centerPos = new Vector2(initialPosCenter.x + (j * walllength) - walllength / 2, initialPosCenter.y + (i * walllength) - walllength / 2);
                tempCenter = Instantiate(center, centerPos, Quaternion.identity) as GameObject;
                tempCenter.transform.parent = centerHolder.transform;
            }
        }
        CreateCells();
    }
    void CreateCells() {
        Debug.Log("CreateCells");
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        GameObject[] allWalls;
        GameObject[] allCenters;
        int centers = centerHolder.transform.childCount;
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        allCenters = new GameObject[centers];
        cells = new Cell[xSize * ySize];
        int atualCenter = 0;

        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;
        for (int i = 0; i < children; i++) {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < centers; i++) {
            allCenters[i] = centerHolder.transform.GetChild(i).gameObject;
        }

        for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++) {
            if (termCount == xSize) {
                eastWestProcess++;
                termCount = 0;
            }
            cells[cellProcess] = new Cell();
            cells[cellProcess].east = allWalls[eastWestProcess];
            cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];
            eastWestProcess++;
            
            termCount++;
            childProcess++;
            cells[cellProcess].west = allWalls[eastWestProcess];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize)+xSize-1];
            cells[cellProcess].full = false;
            if (atualCenter< centers) {
                cells[cellProcess].pos = allCenters[atualCenter];
                atualCenter++;
            }
            


        }
        CreateMaze();
    }

    void CreateMaze() {
        Debug.Log("CreateMaze");
        while (visitedCells < totalCells) {
            if (stardedBuilding) {
                GiveMeNeighbor();
                if(cells[currentNeighbour].visited==false && cells[currentCell].visited == true) {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0) {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }else {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                stardedBuilding = true;
            }
        }
    }

    void BreakWall() {
        switch (wallToBreak) {
            case 1:
                Destroy(cells[currentCell].north);
                break;
            case 2:
                Destroy(cells[currentCell].east);
                break;
            case 3:
                Destroy(cells[currentCell].west);
                break;
            case 4:
                Destroy(cells[currentCell].south);
                break;
        }
    }

    void GiveMeNeighbor() {
        
        int length = 0;
        int[] neighbors = new int[4];
        int[] conectedWalls = new int[4];
        int check = 0;
        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;
        //west
        if(currentCell+1<totalCells && (currentCell + 1) != check) {
            if(cells[currentCell + 1].visited == false) {
                neighbors[length] = currentCell + 1;
                conectedWalls[length] = 3;
                length++;
            }
        }
        //east
        if (currentCell - 1 >=0 && currentCell  != check) {
            if (cells[currentCell - 1].visited == false) {
                neighbors[length] = currentCell - 1;
                conectedWalls[length] = 2;
                length++;
            }
        }
        //north
        if (currentCell +xSize<totalCells) {
            if (cells[currentCell + xSize].visited == false) {
                neighbors[length] = currentCell + xSize;
                conectedWalls[length] = 1;
                length++;
            }
        }
        //south
        if (currentCell - xSize >= 0) {
            if (cells[currentCell - xSize].visited == false) {
                neighbors[length] = currentCell - xSize;
                conectedWalls[length] = 4;
                length++;
            }

        }

        if (length != 0) {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neighbors[theChosenOne];
            wallToBreak = conectedWalls[theChosenOne];
        }else {
            if (backingUp > 0) {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }


    }
    // Update is called once per frame
    void Update () {
	
	}
}
