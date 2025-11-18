using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathNodeMap : MonoBehaviour {
    [SerializeField] Transform nodesParent;
    [SerializeField] Transform modifiersParent;
    [SerializeField] GameObject pathNodePrefab;
    [SerializeField] int sizeX;
    [SerializeField] int sizeY;
    [SerializeField] PathNode[] pathNodes;
    PathNodeModifier[] modifiers;
    [SerializeField] float maxTerrainCost = 8;
    public LayerMask nodeModifierMask;


    private void Start() {
        CheckModifiersAndSetColors();
    }

    [ContextMenu("CheckModifiersAndSetColors")]
    void CheckModifiersAndSetColors() {
        SetModifierColorsBasedOnModification();
        MakeNodesUseModifiers();
        SetNodeColorsBasedOnWeight();
    }

    private void MakeNodesUseModifiers() {
        foreach (PathNode p in pathNodes) {
            p.CheckModifiersForCost();
        }
    }
    public void OnPathNodeValueChanged(PathNode n) {
        SetPathNodeColorBasedOnWeight(n);
    }


    private void SetModifierColorsBasedOnModification(bool hideVisuals = true) {
        PathNodeModifier[] modifiers = modifiersParent.GetComponentsInChildren<PathNodeModifier>();

        foreach (PathNodeModifier p in modifiers) {
            MeshRenderer renderer = p.GetComponentInChildren<MeshRenderer>();
            float colorValue = GetColorValueBasedOnCost(p.modifier, maxTerrainCost);

            if (Application.isPlaying) {
                p.HideVisuals();
            }

#if UNITY_EDITOR
            if (!Application.isPlaying) {
                Undo.RegisterCompleteObjectUndo(renderer, "SetModifierColorsBasedOnModification");
            }
#endif
            if (p.modifier < Mathf.Infinity) {
                renderer.material.color = new Color(colorValue, colorValue, colorValue);
            } else {
                renderer.material.color = Color.red;
            }
        }
    }

    private void SetNodeColorsBasedOnWeight() {
        foreach (PathNode p in pathNodes) {
            SetPathNodeColorBasedOnWeight(p);
        }
    }

    void SetPathNodeColorBasedOnWeight(PathNode p) {

        MeshRenderer renderer = p.GetComponentInChildren<MeshRenderer>();
        float colorValue = GetColorValueBasedOnCost(p.pathTerrainCost, maxTerrainCost);

#if UNITY_EDITOR
        if (!Application.isPlaying) {
            Undo.RegisterCompleteObjectUndo(renderer, "SetNodeColorsBasedOnWeight");
        }
#endif

        if (p.pathTerrainCost < Mathf.Infinity) {
            renderer.material.color = new Color(colorValue, colorValue, colorValue);
        } else {
            renderer.material.color = Color.red;
        }
    }


    [ContextMenu("GenerateMap")]
    public void GenerateMap() {
        ClearNodes();


#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(this, "GenerateMap");
#endif 
        pathNodes = new PathNode[sizeX * sizeY];


        for (int i = 0; i < sizeY; i++) {
            for (int j = 0; j < sizeX; j++) {

                GameObject nodeObj = null;


#if UNITY_EDITOR
                nodeObj = (GameObject)PrefabUtility.InstantiatePrefab(pathNodePrefab, nodesParent);
#else
                nodeObj = Instantiate(pathNodePrefab, nodesParent);
#endif
                nodeObj.transform.position = transform.position + new Vector3(i, 0, j);
                nodeObj.name += j.ToString() + " - " + i.ToString();
                PathNode node = nodeObj.GetComponent<PathNode>();
                node.map = this;
                pathNodes[j * sizeX + i] = node;

#if UNITY_EDITOR
                if (!Application.isPlaying) {
                    Undo.RegisterCreatedObjectUndo(nodeObj, "GenerateMap");
                }
#endif



            }
        }
        InterConnectNodes();
    }

    private void ClearNodes() {
        if (pathNodes != null) {
            foreach (PathNode p in pathNodes) {
#if UNITY_EDITOR
                if (Application.isPlaying) {
                    Destroy(p.gameObject);
                } else {
                    Undo.DestroyObjectImmediate(p.gameObject);
                }
#else
                Destroy(p.gameObject);
#endif
            }
        }
    }

    public float CalculateCostBetweenNodes(PathNode a, PathNode b) {
        return a.pathTerrainCost + b.pathTerrainCost;
    }

    void InterConnectNodes() {
        for (int i = 0; i < sizeY; i++) {
            for (int j = 0; j < sizeX; j++) {
                pathNodes[j * sizeX + i].coordenates = new Vector2(j, i);

                List<PathNode> surroundingPathNodesList = new List<PathNode>();

                if (j < sizeX - 1) {
                    surroundingPathNodesList.Add(pathNodes[(j + 1) * sizeX + i]);
                }

                if (i > 0) {
                    surroundingPathNodesList.Add(pathNodes[j * sizeX + i - 1]);
                }


                if (j > 0) {
                    surroundingPathNodesList.Add(pathNodes[(j - 1) * sizeX + i]);
                }

                if (i < sizeY - 1) {
                    surroundingPathNodesList.Add(pathNodes[j * sizeX + i + 1]);
                }



                pathNodes[j * sizeX + i].surroundingNodes = surroundingPathNodesList.ToArray();
            }
        }
    }

    public PathNode FindClosestNodeTo(Vector3 to) {
        PathNode node = null;
        float closestDistance = Mathf.Infinity;
        foreach (PathNode n in pathNodes) {
            float currentSqrDist = Vector3.SqrMagnitude(to - n.transform.position);
            if (currentSqrDist < closestDistance) {
                closestDistance = currentSqrDist;
                node = n;
            }
        }

        return node;
    }
    public static float GetColorValueBasedOnCost(float pathTerrainCost, float maxTerrainCost) {
        return Mathf.Lerp(1, 0, pathTerrainCost / maxTerrainCost);
    }

}

