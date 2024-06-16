using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelModoEdicion; // El panel para el modo de diseño
    public GameObject panelModoFuncionamiento; // El panel para el modo de funcionamiento
    public Button botonModoFuncionamiento; // Botón para cambiar al modo de funcionamiento
    public Button botonModoEdicion; // Botón para cambiar al modo de diseño

    public LayoutManager layoutManager; // Referencia al LayoutManager
    public Transform mesasContainer; // Contenedor para las mesas en el modo de previsualización

    public GameObject mesaPrefab; // Prefab de la mesa
    public GameObject sillaPrefab; // Prefab de la silla

    void Start()
    {
        // Asegúrate de que el modo de diseño esté activo al iniciar
        SetModoEdicion();

        // Agregar listeners a los botones
        botonModoFuncionamiento.onClick.AddListener(() => {
            GuardarDistribucion();
            SetModoFuncionamiento();
            CargarDistribucion();
        });
        botonModoEdicion.onClick.AddListener(SetModoEdicion);
    }

    void SetModoEdicion()
    {
        panelModoEdicion.SetActive(true);
        panelModoFuncionamiento.SetActive(false);
        mesasContainer.gameObject.SetActive(true); // Asegúrate de que MesasContainer esté activo
        Debug.Log("Modo de diseño activado. MesasContainer activo: " + mesasContainer.gameObject.activeSelf);
    }

    void SetModoFuncionamiento()
    {
        panelModoEdicion.SetActive(false);
        panelModoFuncionamiento.SetActive(true);
        mesasContainer.gameObject.SetActive(true); // Asegúrate de que MesasContainer esté activo
        Debug.Log("Modo de funcionamiento activado. MesasContainer activo: " + mesasContainer.gameObject.activeSelf);
    }



    public void GuardarDistribucion()
    {
        layoutManager.mesasConfig.Clear(); // Asegúrate de limpiar la lista antes de guardar nueva distribución

        foreach (Transform mesa in mesasContainer)
        {
            LayoutManager.MesaConfig mesaConfig = new LayoutManager.MesaConfig
            {
                posicion = mesa.localPosition,
                tamano = new Vector2(mesa.localScale.x, mesa.localScale.y),
                numeroSillas = mesa.childCount,
                sillas = new List<LayoutManager.SillaConfig>()
            };

            foreach (Transform silla in mesa)
            {
                LayoutManager.SillaConfig sillaConfig = new LayoutManager.SillaConfig
                {
                    posicion = silla.localPosition,
                    rotacion = silla.localRotation
                };
                mesaConfig.sillas.Add(sillaConfig);
            }

            layoutManager.mesasConfig.Add(mesaConfig);
        }

        layoutManager.GuardarDistribucion(); // Asegurarse de llamar a GuardarDistribucion() en LayoutManager
        Debug.Log("Distribución guardada. Número de mesas guardadas: " + layoutManager.mesasConfig.Count);
    }



    public void CargarDistribucion()
    {
        // Limpiar cualquier mesa existente en el modo de funcionamiento
        foreach (Transform child in mesasContainer)
        {
            Destroy(child.gameObject);
        }

        // Cargar la distribución guardada desde LayoutManager
        layoutManager.CargarDistribucion();
        Debug.Log("Distribución cargada. Número de mesas cargadas: " + layoutManager.mesasConfig.Count);

        // Instanciar las mesas y sillas guardadas
        foreach (var mesaConfig in layoutManager.mesasConfig)
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
            Debug.Log("Mesa instanciada en posición: " + mesa.transform.position);
        }
    }

}
