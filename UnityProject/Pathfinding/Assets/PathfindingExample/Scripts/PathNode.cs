using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {
    public PathNodeMap map;
    public PathNode[] surroundingNodes;
    public Vector2 coordenates;
    public float pathTerrainCost;
    [SerializeField] TMPro.TextMeshProUGUI costText;

    private void Start() {
        SetPathTerrainCost(pathTerrainCost);
    }

    public void CheckModifiersForCost() {
        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, 100, map.nodeModifierMask, QueryTriggerInteraction.Collide)) {
            PathNodeModifier modifier = hit.collider.GetComponentInParent<PathNodeModifier>();
            SetPathTerrainCost(modifier.modifier);
        }
    }

    public void SetPathTerrainCost(float newCost) {
        pathTerrainCost = newCost;
        if (costText) {
            costText.text = newCost.ToString();
        }
    }


}
