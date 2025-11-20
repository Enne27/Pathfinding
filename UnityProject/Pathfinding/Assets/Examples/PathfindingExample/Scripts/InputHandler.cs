using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
    [SerializeField] LayerMask pathfindingNodeLayerMask;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pathfindingNodeLayerMask)) {
                PathNode p = hit.collider.GetComponentInParent<PathNode>();
                if (p) {
                    switch (p.pathTerrainCost) {
                        case 1:
                            p.SetPathTerrainCost(2.0f);
                            break;
                        case 2:
                            p.SetPathTerrainCost(4.0f);
                            break;
                        case 4:
                            p.SetPathTerrainCost(8.0f);
                            break;
                        case 8:
                            p.SetPathTerrainCost(Mathf.Infinity);
                            break;
                        default:
                            p.SetPathTerrainCost(1.0f);
                            break;
                    }

                }
            }

        }
    }
}
