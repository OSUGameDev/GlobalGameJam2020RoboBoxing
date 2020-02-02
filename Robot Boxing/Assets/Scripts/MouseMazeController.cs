using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class MouseMazeController : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public int cellWidth = 16;
    public int WallWidth = 2;
    public Color WallColor = Color.black;
    public Color PathColor = Color.white;
    public Color TracedPathColor = Color.blue;
    public Color StartColor = Color.red;
    public bool IsHead = false;

    enum WallState
    {
        NONE = 0,
        TOP = 1,
        RIGHT = 2,
        BOTTOM = 4,
        LEFT = 8,
        ALL = 15,
        

    }
    class Cell
    {

        public WallState walls;
        public bool IsPath;
        public Vector2Int position;
        public bool isTrace;
        public bool isSpecial;
        public Cell(int x, int y)
        {
            position = new Vector2Int(x, y);
            walls = WallState.ALL;
            IsPath = false;
            this.isTrace = false;
            this.isSpecial = false;
        }
    }

    int stepCount = 0;
    private Cell[,] cells;
    Stack<Cell> currentPath;
    Texture2D theText;
    Vector3 Dimeniosn;
    // Start is called before the first frame update
    void Start()
    {
        int startX = Random.Range(0, width);
        int startY = Random.Range(0, height);
        cells = new Cell[width, height];

        for(int ix = 0; ix < width; ix++)
        {
            for(int iy = 0; iy < height; iy++)
            {
                cells[ix, iy] = new Cell(ix,iy);
            }
        }
        currentPath = new Stack<Cell>();
        cells[startX, startY].IsPath = true;
        currentPath.Push(cells[startX, startY]);
        while (currentPath.Count > 0)
        {
            Cell top = currentPath.Peek();
            int direction = Random.Range(0, 4);
            // up right left down
            if(direction == 0 && CheckDirection(new Vector2Int(0, 1)))
            {
                Cell toAlter = cells[top.position.x, top.position.y + 1];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.BOTTOM);
                top.walls = top.walls & (WallState.ALL ^ WallState.TOP);
                currentPath.Push(toAlter);
                continue;
            }
            if (direction <= 1 && CheckDirection(new Vector2Int(1, 0)))
            {
                Cell toAlter = cells[top.position.x + 1, top.position.y];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.LEFT);
                top.walls = top.walls & (WallState.ALL ^ WallState.RIGHT);
                currentPath.Push(toAlter);
                continue;
            }
            if (direction <= 2 && CheckDirection(new Vector2Int(0, -1)))
            {
                Cell toAlter = cells[top.position.x, top.position.y - 1];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.TOP);
                top.walls = top.walls & (WallState.ALL ^ WallState.BOTTOM);
                currentPath.Push(toAlter);
                continue;
            }
            if (direction <= 3 && CheckDirection(new Vector2Int(-1, 0)))
            {
                Cell toAlter = cells[top.position.x - 1, top.position.y];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.RIGHT);
                top.walls = top.walls & (WallState.ALL ^ WallState.LEFT);
                currentPath.Push(toAlter);
                continue;
            }
            if (CheckDirection(new Vector2Int(0, 1)))
            {
                Cell toAlter = cells[top.position.x, top.position.y + 1];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.BOTTOM);
                top.walls = top.walls & (WallState.ALL ^ WallState.TOP);
                currentPath.Push(toAlter);
                continue;
            }
            if (CheckDirection(new Vector2Int(1, 0)))
            {
                Cell toAlter = cells[top.position.x + 1, top.position.y];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.LEFT);
                top.walls = top.walls & (WallState.ALL ^ WallState.RIGHT);
                currentPath.Push(toAlter);
                continue;
            }
            if (CheckDirection(new Vector2Int(0, -1)))
            {
                Cell toAlter = cells[top.position.x, top.position.y - 1];
                toAlter.IsPath = true;
                toAlter.walls = toAlter.walls & (WallState.ALL ^ WallState.TOP);
                top.walls = top.walls & (WallState.ALL ^ WallState.BOTTOM);
                currentPath.Push(toAlter);
                continue;
            }
            currentPath.Pop();
        }
        theText = new Texture2D(width * cellWidth, height * cellWidth);

        Cell startCell = cells[0, height - 1];
        startCell.isSpecial = true;
        startCell.walls = startCell.walls & (WallState.ALL ^ WallState.BOTTOM);
        start = startCell.position;
        previousTile = startCell.position;

        Cell endCell = cells[width - 1, 0];
        endCell.isSpecial = true;
        endCell.walls = endCell.walls & (WallState.ALL ^ WallState.TOP);
        finish = endCell.position;
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                WriteCell(cells[ix, iy]);
            }
        }
        theText.Apply();
        this.GetComponent<Image>().sprite = Sprite.Create(theText,new Rect(new Vector2(0,0),new Vector2(width * cellWidth,height * cellWidth)),new Vector2(width * cellWidth / 2, height * cellWidth / 2));

        Dimeniosn = new Vector3(this.GetComponent<RectTransform>().rect.width, this.GetComponent<RectTransform>().rect.height, 0);
    }



    bool CheckDirection(Vector2Int dir)
    {
        Cell cur = currentPath.Peek();
        Vector2Int searchPos = (cur.position + dir);
        if (searchPos.x < 0 || searchPos.x >= cells.GetLength(0) ||
            searchPos.y < 0 || searchPos.y >= cells.GetLength(1))
            return false;
        return !cells[searchPos.x, searchPos.y].IsPath;
    }

    void WriteCell(Cell value)
    {
        for(int ix = 0; ix < cellWidth; ix++)
        {
            for (int iy = 0; iy < cellWidth; iy++)
            {
                theText.SetPixel(value.position.x * cellWidth + ix, value.position.y * cellWidth + iy
                    , value.isSpecial ? StartColor :  (value.isTrace ? TracedPathColor : PathColor));
            }
        }

        for (int iy = 0; iy < height; iy++)
        {
            for (int jy = 0; jy < WallWidth; jy++)
            {
                for (int jx = 0; jx < cellWidth; jx++)
                {
                    if ((cells[value.position.x, value.position.y].walls & WallState.TOP) != 0)
                    {
                        theText.SetPixel(value.position.x * cellWidth + jx, (value.position.y + 1) * cellWidth - 1 - jy, WallColor);
                    }
                    if ((cells[value.position.x, value.position.y].walls & WallState.BOTTOM) != 0)
                    {
                        theText.SetPixel(value.position.x * cellWidth + jx, value.position.y * cellWidth + jy, WallColor);
                    }
                    if ((cells[value.position.x, value.position.y].walls & WallState.RIGHT) != 0)
                    {
                        theText.SetPixel((value.position.x + 1) * cellWidth - 1 - jy, value.position.y * cellWidth + jx, WallColor);
                    }
                    if ((cells[value.position.x, value.position.y].walls & WallState.LEFT) != 0)
                    {
                        theText.SetPixel(value.position.x * cellWidth + jy, value.position.y * cellWidth + jx, WallColor);
                    }
                }
            }
        }

    }

    Vector2Int start;
    Vector2Int finish;


    bool checkIsValidMove(Vector2Int lastCoords, Vector2Int newCoords)
    {
        Vector2Int diff = newCoords - lastCoords;
        if (diff.sqrMagnitude > 1)
            return false;
        if(diff == new Vector2Int(1,0))
        {
            return (cells[newCoords.x, newCoords.y].walls & WallState.LEFT) == 0;
        }
        else if (diff == new Vector2Int(-1, 0))
        {
            return (cells[newCoords.x, newCoords.y].walls & WallState.RIGHT) == 0;
        }
        else if (diff == new Vector2Int(0, 1))
        {
            return (cells[newCoords.x, newCoords.y].walls & WallState.BOTTOM) == 0;
        }
        else if (diff == new Vector2Int(0, -1))
        {
            return (cells[newCoords.x, newCoords.y].walls & WallState.TOP) == 0;
        }
        return false;
    }

    Stack<Vector2Int> Trail = new Stack<Vector2Int>();
    Vector2Int previousTile;
    // Update is called once per frame
    void Update()
    {

        Vector3 position = Input.mousePosition - (this.transform.position);
        position = new Vector3(position.x / transform.lossyScale.x, position.y / transform.lossyScale.y, position.z / transform.lossyScale.z);


        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("");
        }
        if(position.x > 0 && position.y > 0 && position.x < Dimeniosn.x && position.y < Dimeniosn.y)
        {
            Vector2Int coords = new Vector2Int((int)(position.x / Dimeniosn.x * width), (int)(position.y / Dimeniosn.y * height));
            if ((previousTile != coords) && checkIsValidMove(previousTile, coords) && (Trail.Count == 0 || Trail.Peek() != coords))
            {
                stepCount++;
                Trail.Push(previousTile);
                previousTile = coords;
                cells[coords.x, coords.y].isTrace = true;
                WriteCell(cells[coords.x, coords.y]);
                theText.Apply();
                this.GetComponent<Image>().sprite = Sprite.Create(theText, new Rect(new Vector2(0, 0), new Vector2(width * cellWidth, height * cellWidth)), new Vector2(width * cellWidth / 2, height * cellWidth / 2));

                if(coords == finish)
                {
                    int optimalCount = Trail.Count;
                    if(IsHead)
                    {
                        GameController.Instance.player.ModifyCircuitBoard(100 * (stepCount / optimalCount));
                    }
                    else
                    {
                        GameController.Instance.player.ModifyCableBox(100 * (stepCount / optimalCount));
                    }
                    
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Repair Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
                }
            }
            else if(Trail.Count > 0 && Trail.Peek() == coords)
            {
                cells[previousTile.x, previousTile.y].isTrace = false;
                WriteCell(cells[previousTile.x, previousTile.y]);
                theText.Apply();
                this.GetComponent<Image>().sprite = Sprite.Create(theText, new Rect(new Vector2(0, 0), new Vector2(width * cellWidth , height * cellWidth )), new Vector2(width * cellWidth / 2, height * cellWidth / 2));
                
                previousTile = Trail.Pop();
            }
        }
    }
}
