using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindAgent : MonoBehaviour {
    public enum PATHFIND_ALGORITHM { BREADTH_FIRST_SEARCH, DIJKSTRA, GREEDY_FIRST_SEARCH, A_STAR }
    [SerializeField] PATHFIND_ALGORITHM pathfindAlgorithm;
    [SerializeField] PathNodeMap nodeMap;
    [SerializeField] GameObject from;
    [SerializeField] GameObject to;
    [SerializeField] GameObject pathRepresentationPrefabObject;
    [SerializeField] GameObject incompletePathRepresentationPrefabObject;
    [SerializeField] GameObject pathRepresentationParentObject;
    [SerializeField] GameObject frontierRepresentationParentObject;
    [SerializeField] GameObject frontierRepresentationPrefabObject;
    List<GameObject> pathRepresentationObjects;
    List<GameObject> frontierRepresentationObjects;
    [SerializeField] float heuristicFactor = 2.0f;
    [SerializeField] bool searchOnChange = false;
    [SerializeField] bool earlyExit = false;
    [SerializeField] bool generateFrontierObject = false;

    PATHFIND_ALGORITHM prevPathfindAlgorithm;
    Vector3 prevDestinationPos;
    Vector3 prevOriginPos;
    bool prevSearchOnChange = false;
    bool prevEarlyExit = false;


    private void Update() {
        if (searchOnChange) {
            if (prevDestinationPos != to.transform.position
                || prevOriginPos != from.transform.position
                || prevPathfindAlgorithm != pathfindAlgorithm
                || prevSearchOnChange != searchOnChange
                || prevEarlyExit != earlyExit) {
                UpdateBasedOnSelectPathfindAlgorithm(pathfindAlgorithm);
                prevDestinationPos = to.transform.position;
                prevOriginPos = from.transform.position;
                prevPathfindAlgorithm = pathfindAlgorithm;
            }
        }
    }

    void UpdateBasedOnSelectPathfindAlgorithm(PATHFIND_ALGORITHM pathfindAlgorithm) {
        switch (pathfindAlgorithm) {
            case PATHFIND_ALGORITHM.BREADTH_FIRST_SEARCH:
                DoSearchForPathBreadthFirstSearch();
                break;
            case PATHFIND_ALGORITHM.DIJKSTRA:
                DoSearchForPathDijkstra();
                break;
            case PATHFIND_ALGORITHM.GREEDY_FIRST_SEARCH:
                DoSearchGreedyBestFirstSearch();
                break;
            case PATHFIND_ALGORITHM.A_STAR:
                DoSearchForPathAStar();
                break;
        }
    }

    [ContextMenu("SearchBreadthFirstSearch")]
    public void DoSearchForPathBreadthFirstSearch() {
        //Out path es el camino (Array de nodos) que podemos usar para hacer Path Follow
        if (Pathfinder.GeneratePathBreadthFirstSearch(nodeMap.FindClosestNodeTo(from.transform.position), nodeMap.FindClosestNodeTo(to.transform.position), earlyExit, out Path path, out PathDebugInfo pathDebugInfo)) {
            RepresentPath(path, pathDebugInfo);
        } else {
            ClearPathRepresentationObject();
            ClearFrontierRepresentationObject();
        }
    }

    [ContextMenu("SearchGreedyBestFirstSearch")]
    public void DoSearchGreedyBestFirstSearch() {
        //Out path es el camino (Array de nodos) que podemos usar para hacer Path Follow
        if (Pathfinder.GeneratePathHeuristic(nodeMap.FindClosestNodeTo(from.transform.position), nodeMap.FindClosestNodeTo(to.transform.position), earlyExit, out Path path, out PathDebugInfo pathDebugInfo)) {
            RepresentPath(path, pathDebugInfo);
        }
    }



    [ContextMenu("SearchAStar")]
    public void DoSearchForPathAStar() {
        //Out path es el camino (Array de nodos) que podemos usar para hacer Path Follow
        if (Pathfinder.GeneratePathAStar(nodeMap.FindClosestNodeTo(from.transform.position), nodeMap.FindClosestNodeTo(to.transform.position), heuristicFactor, earlyExit, out Path path, out PathDebugInfo pathDebugInfo)) {
            RepresentPath(path, pathDebugInfo);
        } else {
            RepresentIncompletePath(path, pathDebugInfo);
        }
    }

    [ContextMenu("SearchDijkstra")]
    public void DoSearchForPathDijkstra() {
        //Out path es el camino (Array de nodos) que podemos usar para hacer Path Follow
        if (Pathfinder.GeneratePathDijkstra(nodeMap.FindClosestNodeTo(from.transform.position), nodeMap.FindClosestNodeTo(to.transform.position), earlyExit, out Path path, out PathDebugInfo pathDebugInfo)) {
            RepresentPath(path, pathDebugInfo);
        } else {
            RepresentIncompletePath(path, pathDebugInfo);
        }
    }

    private void RepresentIncompletePath(Path path, PathDebugInfo pathDebugInfo) {
        RepresentPathInternal(path, pathDebugInfo, incompletePathRepresentationPrefabObject);
    }

    void RepresentPath(Path path, PathDebugInfo pathDebugInfo) {
        RepresentPathInternal(path, generateFrontierObject ? pathDebugInfo : null, pathRepresentationPrefabObject);
    }

    void RepresentPathInternal(Path path, PathDebugInfo pathDebugInfo, GameObject prefabsForPathPoints) {
        ClearFrontierRepresentationObject();
        ClearPathRepresentationObject();
        foreach (PathNode p in path.pathNodes) {
            GameObject frontierRepresentationObject = Instantiate(prefabsForPathPoints, pathRepresentationParentObject.transform);
            frontierRepresentationObject.transform.position = p.transform.position;
            pathRepresentationObjects.Add(frontierRepresentationObject);
        }

        if (pathDebugInfo != null) {
            foreach (PathNode p in pathDebugInfo.orderedFrontier) {
                GameObject frontierRepresentationObject = Instantiate(frontierRepresentationPrefabObject, frontierRepresentationParentObject.transform);
                frontierRepresentationObject.transform.position = p.transform.position;
                frontierRepresentationObjects.Add(frontierRepresentationObject);
            }
        }

    }

    private void ClearFrontierRepresentationObject() {
        if (frontierRepresentationObjects != null) {
            for (int i = frontierRepresentationObjects.Count - 1; i >= 0; i--) {
                Destroy(frontierRepresentationObjects[i]);
            }
        }
        frontierRepresentationObjects = new List<GameObject>();
    }

    private void ClearPathRepresentationObject() {
        if (pathRepresentationObjects != null) {
            for (int i = pathRepresentationObjects.Count - 1; i >= 0; i--) {
                Destroy(pathRepresentationObjects[i]);
            }
        }
        pathRepresentationObjects = new List<GameObject>();

    }
}
