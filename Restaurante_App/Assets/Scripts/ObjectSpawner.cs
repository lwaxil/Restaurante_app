using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject tablePrefab;
    public GameObject chairPrefab;
    public Transform parentTransform;

    public PanelSwitcher panelSwitcher; // Asegúrate de asignar esto desde el inspector

    public void SpawnTable()
    {
        GameObject mesa = Instantiate(tablePrefab, parentTransform);
        mesa.transform.SetAsLastSibling();

        Debug.Log("Mesa generada.");
        panelSwitcher.GuardarDistribucion(); // Asegurarse de llamar a GuardarDistribucion() después de cada generación
    }

    public void SpawnChair()
    {
        GameObject silla = Instantiate(chairPrefab, parentTransform);
        silla.transform.SetAsFirstSibling();

        Debug.Log("Silla generada.");
        panelSwitcher.GuardarDistribucion(); // Asegurarse de llamar a GuardarDistribucion() después de cada generación
    }

}
