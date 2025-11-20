using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PathfindAgent;

public class PathUser : MonoBehaviour {

    [SerializeField] PathNodeMap nodeMap;
    [SerializeField] GameObject destination;
    

    public Path GetPathWithAlgorithm(PathfindAgent.PATHFIND_ALGORITHM pathfindAlgorithm) {
        switch (pathfindAlgorithm) {
            case PATHFIND_ALGORITHM.BREADTH_FIRST_SEARCH:
                return DoSearchForPathBreadthFirstSearch();
            case PATHFIND_ALGORITHM.DIJKSTRA:
                return DoSearchForPathDijkstra();
            case PATHFIND_ALGORITHM.GREEDY_FIRST_SEARCH:
                return DoSearchGreedyBestFirstSearch();
            case PATHFIND_ALGORITHM.A_STAR:
                return DoSearchForPathAStar();
        }
        return null;
    }

    private Path DoSearchForPathAStar() {
        if (Pathfinder.GeneratePathAStar(nodeMap.FindClosestNodeTo(transform.position),
            nodeMap.FindClosestNodeTo(destination.transform.position),
            1.0f, true, out Path path, out PathDebugInfo pathDebugInfo)) {
            return path;
        }
        return null;
    }

    private Path DoSearchGreedyBestFirstSearch() {
        if (Pathfinder.GeneratePathHeuristic(
            nodeMap.FindClosestNodeTo(transform.position),
            nodeMap.FindClosestNodeTo(destination.transform.position),
             true, out Path path, out PathDebugInfo pathDebugInfo)) {
            return path;
        }
        return null;
    }

    private Path DoSearchForPathDijkstra() {
        if (Pathfinder.GeneratePathDijkstra(
            nodeMap.FindClosestNodeTo(transform.position),
            nodeMap.FindClosestNodeTo(destination.transform.position),
             true, out Path path, out PathDebugInfo pathDebugInfo)) {
            return path;
        }
        return null;
    }

    private Path DoSearchForPathBreadthFirstSearch() {
        if (Pathfinder.GeneratePathBreadthFirstSearch(
            nodeMap.FindClosestNodeTo(transform.position),
            nodeMap.FindClosestNodeTo(destination.transform.position),
             true, out Path path, out PathDebugInfo pathDebugInfo)) {
            return path;
        }
        return null;
    }
}
