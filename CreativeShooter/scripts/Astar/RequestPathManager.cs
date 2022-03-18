using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RequestPathManager : MonoBehaviour {

    //建立一個集合來放置FIFO的物件
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static RequestPathManager instance;
    PathFinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }

    //定義RequestPath函式,POP出路徑的這一個節點,POP In路徑的下一個節點
    public static void RequestPath(Vector3 pathStart , Vector3 pathEnd , Action<Vector3[] , bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        //如果不是正在處理路徑而且路徑的集合!=0
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            //目前的路徑是移除掉最前端的物件後的下一個物件
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart , currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path , bool success)
    {
        //處離完成後呼叫下一個集合物件繼續處理
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start , Vector3 _end , Action<Vector3[] , bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

}
