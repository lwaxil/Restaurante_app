using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    public string filePath = "mesas.json"; // Nombre del archivo donde se guardará la configuración
    public List<MesaConfig> mesasConfig = new List<MesaConfig>();

    public Transform mesasContainer; // Contenedor para las mesas en el panel de diseño
    public Transform mesasContainerFuncionamiento; // Contenedor para las mesas en el panel de funcionamiento
    public GameObject mesaPrefab; // Prefab de la mesa
    public GameObject sillaPrefab; // Prefab de la silla

    // Método para guardar la distribución de las mesas y sillas
    public void GuardarDistribucion()
    {
        string json = JsonUtility.ToJson(new MesasWrapper { mesas = mesasConfig });
        File.WriteAllText(filePath, json);
        Debug.Log("Archivo guardado en: " + filePath);
    }

    // Método para cargar la distribución de las mesas y sillas
    public void CargarDistribucion()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            mesasConfig = JsonUtility.FromJson<MesasWrapper>(json).mesas;
            InstanciarMesas(); // Instanciar las mesas después de cargar la distribución
        }
    }

    // Método para instanciar las mesas y sillas según la configuración guardada
    private void InstanciarMesas()
    {
        LimpiarMesasContainer(); // Limpiar cualquier mesa existente

        foreach (var mesaConfig in mesasConfig)
        {
            GameObject mesa = Instantiate(mesaPrefab, mesasContainer);
            mesa.transform.localPosition = mesaConfig.posicion;
            mesa.transform.localScale = new Vector3(mesaConfig.tamano.x, mesaConfig.tamano.y, 1);

            foreach (var sillaConfig in mesaConfig.sillas)
            {
                GameObject silla = Instantiate(sillaPrefab, mesa.transform);
                silla.transform.localPosition = sillaConfig.posicion;
                silla.transform.localRotation = sillaConfig.rotacion;
            }
        }
    }

    // Método para limpiar las mesas dentro del contenedor de diseño
    public void LimpiarMesasContainer()
    {
        foreach (Transform child in mesasContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // Clase wrapper para permitir la serialización/deserialización de una lista de MesaConfig
    [System.Serializable]
    public class MesasWrapper
    {
        public List<MesaConfig> mesas;
    }

    // Clase para representar la configuración de una mesa
    [System.Serializable]
    public class MesaConfig
    {
        public Vector2 posicion;
        public Vector2 tamano;
        public int numeroSillas;
        public List<SillaConfig> sillas;
    }

    // Clase para representar la configuración de una silla
    [System.Serializable]
    public class SillaConfig
    {
        public Vector2 posicion;
        public Quaternion rotacion;
    }
}
