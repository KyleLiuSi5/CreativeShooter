using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFinding : MonoBehaviour {

    RequestPathManager requestManager;
    //獲取格子
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<RequestPathManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos , Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if(startNode.walkable && targetNode.walkable)
        {
            //製作兩個List用來存放路線
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            //放入最開始的節點
            openSet.Add(startNode);

            while (openSet.Count > 0)//如果還有節點,繼續搜尋
            {
                //走過後將節點移除並放入closedSet陣列中
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)//如果到達最終目標節點,完成路線
                {
                    pathSuccess = true;
                    break;
                }
                //搜尋鄰近點
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))//如果鄰近節點不能行走或是鄰近節點在走過的陣列中
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }        
        yield return null;
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode , Node endNode)
    {
        //從最終節點往前搜尋節點放入陣列中,最後再反轉陣列然後return wayPoints
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    //獲取節點距離
    int GetDistance(Node nodeA , Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
