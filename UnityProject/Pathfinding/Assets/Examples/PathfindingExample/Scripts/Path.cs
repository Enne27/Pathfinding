using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path {
    public PathNode[] pathNodes;
    public Path(List<PathNode> pathNodeList) {
        pathNodes = pathNodeList.ToArray();
    }

    public Path(PathNode[] pathNodeList) {
        pathNodes = pathNodeList;
    }

    public Path() {
        pathNodes = new PathNode[0];
    }

}

[System.Serializable]
public class PathDebugInfo {
    public List<PathNode> orderedFrontier;
    public PathDebugInfo() {
        orderedFrontier = new List<PathNode>();
    }

}