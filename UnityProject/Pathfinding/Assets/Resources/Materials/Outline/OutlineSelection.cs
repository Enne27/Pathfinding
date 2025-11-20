using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer))]
public class OutlineSelection : MonoBehaviour
{
    [Header("Material Settings")]
    [Tooltip("Material que se aplicará al último slot al hacer hover")]
    public Material hoverMaterial;

    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private int lastMaterialIndex;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // Verificar si el objeto tiene collider
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("El objeto necesita un Collider para detectar el hover del ratón!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverMaterial == null) return;

        // Obtener índice del último material
        lastMaterialIndex = meshRenderer.materials.Length - 1;

        // Guardar material original
        originalMaterial = meshRenderer.materials[lastMaterialIndex];

        // Crear nueva array de materiales
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[lastMaterialIndex] = hoverMaterial;
        meshRenderer.materials = newMaterials;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (originalMaterial == null) return;

        // Restaurar material original
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[lastMaterialIndex] = originalMaterial;
        meshRenderer.materials = newMaterials;
    }
}
