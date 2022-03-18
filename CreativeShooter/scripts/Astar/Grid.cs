using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public bool displayGridGizmos;
    //不能行走的layer類別
    public LayerMask unwalkableMask;
    //A*壟罩範圍
    public Vector2 gridWorldSize;
    //每一格的半徑
    public float nodeRadius;

    public PlaceType[] walkableRegions;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    //宣告格子節點
    Node[,] grid;
    
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);//x軸方向有幾個格子
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//y軸方向有幾個格子

        foreach(PlaceType region in walkableRegions)
        {
            walkableMask.value += region.PlaceMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.PlaceMask.value , 2),region.PlacePenalty);
        }

        CreatGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreatGrid()
    {
        
        grid = new Node[gridSizeX, gridSizeY];

        //從範圍內的左下方開始繪製格子
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for(int x = 0;x<gridSizeX;x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius , unwalkableMask));

                int movementPenalty = 0;

                //射線
                if(walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if(Physics.Raycast(ray,out hit , 100 , walkableMask))
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                grid[x, y] = new Node(walkable, worldPoint , x , y , movementPenalty);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //鄰近節點為自己的點X軸+-1,y軸+-1
        for(int x = -1 ; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    //尋找特定的點
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x + gridWorldSize.x / 2 - nodeRadius) / nodeDiameter);
        int y = Mathf.RoundToInt((worldPosition.y + gridWorldSize.y / 2 - nodeRadius) / nodeDiameter);

        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        //畫出格子
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));              
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    //可以走的顯示白色,不能走的顯示紅色
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        
        
    }

    [System.Serializable]
    public class PlaceType
    {
        public LayerMask PlaceMask;
        public int PlacePenalty;
    }

	
}
