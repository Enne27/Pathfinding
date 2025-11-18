using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {
    public static bool GeneratePathAStar(PathNode origin, PathNode destination, float heuristicFactor, bool earlyExit, out Path path, out PathDebugInfo pathDebugInfo) {
        pathDebugInfo = new PathDebugInfo();
        path = null;
        bool foundPath = false;
        PriorityQueue<PathNode> frontier = new PriorityQueue<PathNode>();
        frontier.Enqueue(origin, 0);
        pathDebugInfo.orderedFrontier.Add(origin);

        Dictionary<PathNode, PathNode> came_from = new Dictionary<PathNode, PathNode>();
        Dictionary<PathNode, float> cost_so_far = new Dictionary<PathNode, float>();

        came_from[origin] = null;
        cost_so_far[origin] = 0;

        while (frontier.Count > 0 && !(foundPath && earlyExit)) {
            PathNode current = frontier.Dequeue();

            if (current == destination) {
                foundPath = true;
            }

            for (int i = 0; i < current.surroundingNodes.Length; i++) {
                PathNode next = current.surroundingNodes[i];
                float newCost = cost_so_far[current] + current.map.CalculateCostBetweenNodes(current, next);
                if (newCost < Mathf.Infinity) {
                    if (!cost_so_far.ContainsKey(next) || newCost < cost_so_far[next]) {
                        float priority = newCost + ManhattanHeuristic(next, destination) * heuristicFactor;
                        cost_so_far[next] = newCost;
                        frontier.Enqueue(next, priority);
                        pathDebugInfo.orderedFrontier.Add(next);
                        came_from[next] = current;
                    }
                }
            }

        }

        path = GeneratePathWithCameFrom(origin, destination, came_from);

        return foundPath;
    }

    public static bool GeneratePathDijkstra(PathNode origin, PathNode destination, bool earlyExit, out Path path, out PathDebugInfo pathDebugInfo) {
        pathDebugInfo = new PathDebugInfo();
        path = null;
        bool foundPath = false;
        PriorityQueue<PathNode> frontier = new PriorityQueue<PathNode>();
        frontier.Enqueue(origin, 0);
        pathDebugInfo.orderedFrontier.Add(origin);

        Dictionary<PathNode, PathNode> came_from = new Dictionary<PathNode, PathNode>();
        Dictionary<PathNode, float> cost_so_far = new Dictionary<PathNode, float>();

        came_from[origin] = null;
        cost_so_far[origin] = 0;

        while (frontier.Count > 0 && !(foundPath && earlyExit)) {
            PathNode current = frontier.Dequeue();

            if (current == destination) {
                foundPath = true;
            }

            for (int i = 0; i < current.surroundingNodes.Length; i++) {
                PathNode next = current.surroundingNodes[i];
                float newCost = cost_so_far[current] + current.map.CalculateCostBetweenNodes(current, next);
                if (newCost < Mathf.Infinity) {
                    if ((!cost_so_far.ContainsKey(next)) || newCost < cost_so_far[next]) {
                        float priority = newCost;
                        cost_so_far[next] = newCost;
                        frontier.Enqueue(next, priority);
                        pathDebugInfo.orderedFrontier.Add(next);
                        came_from[next] = current;
                    }
                }
            }

        }

        path = GeneratePathWithCameFrom(origin, destination, came_from);

        return foundPath;
    }

    public static bool GeneratePathHeuristic(PathNode origin, PathNode destination, bool earlyExit, out Path path, out PathDebugInfo pathDebugInfo) {
        pathDebugInfo = new PathDebugInfo();
        path = null;
        bool foundPath = false;
        PriorityQueue<PathNode> frontier = new PriorityQueue<PathNode>();
        frontier.Enqueue(origin, 0);
        pathDebugInfo.orderedFrontier.Add(origin);

        Dictionary<PathNode, PathNode> came_from = new Dictionary<PathNode, PathNode>();

        came_from[origin] = null;

        while (frontier.Count > 0 && !(foundPath && earlyExit)) {
            PathNode current = frontier.Dequeue();

            if (current == destination) {
                foundPath = true;
            }


            for (int i = 0; i < current.surroundingNodes.Length; i++) {
                PathNode next = current.surroundingNodes[i];
                if (!came_from.ContainsKey(next)) {
                    float priority = ManhattanHeuristic(next, destination);
                    frontier.Enqueue(next, priority);
                    pathDebugInfo.orderedFrontier.Add(next);
                    came_from[next] = current;
                }
            }

        }

        path = GeneratePathWithCameFrom(origin, destination, came_from);

        return foundPath;
    }

    public static bool GeneratePathBreadthFirstSearch(PathNode origin, PathNode destination, bool earlyExit, out Path path, out PathDebugInfo pathDebugInfo) {
        pathDebugInfo = new PathDebugInfo();
        path = null;
        bool foundPath = false;
        Queue<PathNode> frontier = new Queue<PathNode>();
        frontier.Enqueue(origin);
        pathDebugInfo.orderedFrontier.Add(origin);

        Dictionary<PathNode, PathNode> came_from = new Dictionary<PathNode, PathNode>();

        came_from[origin] = null;

        while (frontier.Count > 0 && !(foundPath && earlyExit)) {
            PathNode current = frontier.Dequeue();

            if (current == destination) {
                foundPath = true;
            }

            for (int i = 0; i < current.surroundingNodes.Length; i++) {
                PathNode next = current.surroundingNodes[i];
                if (!came_from.ContainsKey(next)) {
                    frontier.Enqueue(next);
                    pathDebugInfo.orderedFrontier.Add(next);
                    came_from[next] = current;
                }
            }

        }

        path = GeneratePathWithCameFrom(origin, destination, came_from);

        return foundPath;
    }

    static Path GeneratePathWithCameFrom(PathNode origin, PathNode destination, Dictionary<PathNode, PathNode> came_from) {

        List<PathNode> pathNodeList = new List<PathNode>();
        PathNode current = destination;

        while (current != origin && came_from.ContainsKey(current)) {
            pathNodeList.Add(current);
            current = came_from[current];

        }
        pathNodeList.Add(origin);
        pathNodeList.Reverse();


        Path path = new Path(pathNodeList);

        return path;
    }

    public static float ManhattanHeuristic(PathNode from, PathNode to) {
        return Vector3.Distance(from.transform.position, to.transform.position);
    }

}