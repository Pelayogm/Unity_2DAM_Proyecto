using System;
using Armeria;
using Funciones.Partida;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorMenuArmas : MonoBehaviour
{
    //Array de los modelos de las armas disponibles.
    public GameObject [] armasDisponibles;
    
    //Menu Estadisticas (UI)
    public Slider potenciaSlider, cadenciaSlider, direccionSlider;
    public TMP_Text p_potencia, p_cadencia, p_direccion, p_arma, n_arma;
    public GameObject estadisticasBloqueadas, estadisticasDesbloqueadas;
    public TMP_Text descripcionArma;
    
    //Menu Barra de Abajo
    public GameObject[] candados;
    
    //Botones de la UI para comprar un arma o desactivarlo
    public GameObject botonComprar, botonEscoger;
    
    //Barra de Arriba
    public TMP_Text t_creditos;
    //PopUps
    public GameObject comprarArmaPopUP, faltanMonedasPopUP;
    
    public void Start()
    {
        //Barra de arriba
        DataUsuario.cargarDatos();
        t_creditos.text = DataUsuario.creditos.ToString();
        n_arma.text = DataUsuario.nombresArmas[0];
        descripcionArma.text = DataUsuario.descripcionesArmas[0];

        //Barra de abajo
        for (int i = 0; i < candados.Length; i++)
        {
            if (DataUsuario.armasDesbloqueadas[i] == 1) 
            {
                candados[i].SetActive(false);
            }
            else
            {
                candados[i].SetActive(true);
            }
        }
        
        botonEscoger.SetActive(true);
        botonComprar.SetActive(false);

        //Lista de armas
        //Se recorren todas las armas y se desactivan, menos la primera el cañon.
        foreach (GameObject arma in armasDisponibles)
        {
            arma.SetActive(false);
        }
        armasDisponibles[0].SetActive(true);
        
        //Estadísticas de las armas (sliders)
        potenciaSlider.value = DataUsuario.armasPotencia[0] / 10f;
        cadenciaSlider.value = DataUsuario.armasCadencia[0] / 10f;
        direccionSlider.value = DataUsuario.armasSuerte[0] / 10f;
        
        //Los textos de cuanto cuesta cada mejora.
        p_potencia.text = DataUsuario.costeMejoras[0, DataUsuario.armasPotencia[0]].ToString();
        p_cadencia.text = DataUsuario.costeMejoras[0, DataUsuario.armasCadencia[0]].ToString();
        p_direccion.text = DataUsuario.costeMejoras[0, DataUsuario.armasSuerte[0]].ToString();
    }

    public void escogerArma(int posicionArma)
    {
        DataUsuario.armaActual = posicionArma;
        foreach (GameObject arma in armasDisponibles)
        {
            arma.SetActive(false);
        }
        armasDisponibles[posicionArma].SetActive(true);
        
        //Si el arma esta bloqueada se pone el menú de compra.
        if (DataUsuario.armasDesbloqueadas[posicionArma] == 0)
        {
            botonComprar.SetActive(true);
            botonEscoger.SetActive(false);
            estadisticasBloqueadas.SetActive(true);
            estadisticasDesbloqueadas.SetActive(false);
            
            p_arma.text = DataUsuario.precioArmas[posicionArma].ToString();
        }
        else
        {
            botonComprar.SetActive(false);
            botonEscoger.SetActive(true);
            estadisticasBloqueadas.SetActive(false);
            estadisticasDesbloqueadas.SetActive(true);
            
            potenciaSlider.value = DataUsuario.armasPotencia[posicionArma] / 10f;
            cadenciaSlider.value = DataUsuario.armasCadencia[posicionArma] / 10f;
            direccionSlider.value = DataUsuario.armasSuerte[posicionArma] / 10f;
            
            print(DataUsuario.costeMejoras[posicionArma, DataUsuario.nivelesPotencia[posicionArma]].ToString());
            print(DataUsuario.costeMejoras[posicionArma, DataUsuario.nivelesCadencia[posicionArma]].ToString());
            print(DataUsuario.costeMejoras[posicionArma, DataUsuario.nivelesSuerte[posicionArma]].ToString());
            
            p_potencia.text = DataUsuario.costeMejoras[posicionArma, DataUsuario.nivelesPotencia[posicionArma]].ToString();
            p_cadencia.text = DataUsuario.costeMejoras[posicionArma, DataUsuario.nivelesCadencia[posicionArma]].ToString();
            p_direccion.text = DataUsuario.costeMejoras[posicionArma, DataUsuario.nivelesSuerte[posicionArma]].ToString();
        }
        n_arma.text = DataUsuario.nombresArmas[posicionArma];
        descripcionArma.text = DataUsuario.descripcionesArmas[posicionArma];
        PartidaManager.instance.armaSeleccionada = posicionArma;
    }

    public void comprarArma()
    {
        if (DataUsuario.precioArmas[DataUsuario.armaActual] <= DataUsuario.creditos)
        {
           comprarArmaPopUP.SetActive(true); 
        }
        else
        {
            faltanMonedasPopUP.SetActive(true);
        }
    }
    
    //REVISAR ESTOS METODOS POR EL BUCLE

    public void confirmarComprarArma()
    {
        DataUsuario.creditos -= DataUsuario.precioArmas[DataUsuario.armaActual];
        DataUsuario.armasDesbloqueadas[DataUsuario.armaActual] = 1;
        DataUsuario.guardarDatos();
        
        comprarArmaPopUP.SetActive(false);
        candados[DataUsuario.armaActual].SetActive(false);
        botonEscoger.SetActive(true);
        botonComprar.SetActive(false);
        estadisticasDesbloqueadas.SetActive(true);
        estadisticasBloqueadas.SetActive(false);
        
        t_creditos.text = DataUsuario.creditos.ToString();
        
        potenciaSlider.value = DataUsuario.armasPotencia[DataUsuario.armaActual] / 10f;
        cadenciaSlider.value = DataUsuario.armasCadencia[DataUsuario.armaActual] / 10f;
        direccionSlider.value = DataUsuario.armasSuerte[DataUsuario.armaActual] / 10f;
            
        p_potencia.text = DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.nivelesPotencia[DataUsuario.armaActual]].ToString();
        p_cadencia.text = DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.nivelesCadencia[DataUsuario.armaActual]].ToString();
        p_direccion.text = DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.nivelesSuerte[DataUsuario.armaActual]].ToString();
    }

    public void cancelarComprarArma()
    {
        faltanMonedasPopUP.SetActive(false);
        comprarArmaPopUP.SetActive(false);
    }

    public void confirmarArmaSeleccion()
    {
        PartidaManager.instance.armaSeleccionada = DataUsuario.armaActual;
        print(DataUsuario.armaActual);
        SceneManager.LoadScene("Scenes/SelectorEscenarios");
    }

    public void comprarMejoraPotencia()
    {
        int nivelActual = DataUsuario.nivelesPotencia[DataUsuario.armaActual];
        int maxNivel = DataUsuario.nivelesPotencia.Length;
        
        if (nivelActual < maxNivel)
        {
            if (DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.nivelesPotencia[DataUsuario.armaActual]] <= DataUsuario.creditos)
            {
            
                DataUsuario.creditos -= DataUsuario.costeMejoras[DataUsuario.armaActual,  DataUsuario.nivelesPotencia[DataUsuario.armaActual]];
                DataUsuario.nivelesPotencia[DataUsuario.armaActual]++;
                DataUsuario.armasPotencia[DataUsuario.armaActual]++;
                DataUsuario.guardarDatos();

                t_creditos.text = DataUsuario.creditos.ToString();
                potenciaSlider.value = DataUsuario.nivelesPotencia[DataUsuario.armaActual] / 10f;
                p_potencia.text = DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.nivelesPotencia[DataUsuario.armaActual]].ToString();
            }
            else
            {
                faltanMonedasPopUP.SetActive(true);
            }
        }
        else
        {
            p_potencia.text = "MAX";
        }
    }
    
    public void comprarMejoraCadencia()
    {
        
        int nivelActual = DataUsuario.armasCadencia[DataUsuario.armaActual];
        int maxNivel = DataUsuario.costeMejoras.GetLength(1);

        if (nivelActual + 1 < maxNivel)
        {
            int posicionArma = DataUsuario.armaActual;
            print(DataUsuario.armasCadencia[posicionArma]);
            if (DataUsuario.costeMejoras[posicionArma, DataUsuario.armasCadencia[posicionArma]] <= DataUsuario.creditos)
            {
                DataUsuario.creditos -= DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.armasCadencia[DataUsuario.armaActual]];
                DataUsuario.armasCadencia[DataUsuario.armaActual]++;
                DataUsuario.guardarDatos();

                t_creditos.text = DataUsuario.creditos.ToString();
                cadenciaSlider.value = DataUsuario.nivelesCadencia[DataUsuario.armaActual] / 10f;
                p_cadencia.text = DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.armasCadencia[DataUsuario.armaActual]].ToString();
            }
            else
            {
                faltanMonedasPopUP.SetActive(true);
            }
        }
        else
        {
            p_cadencia.text = "MAX";
        }
    }
    
    public void comprarMejoraSuerte()
    {
        int nivelActual = DataUsuario.armasSuerte[DataUsuario.armaActual];
        int maxNivel = DataUsuario.costeMejoras.GetLength(1);

        if (nivelActual + 1 < maxNivel)
        {
            if (DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.armasSuerte[DataUsuario.armaActual]] <= DataUsuario.creditos)
            {
                DataUsuario.creditos -= DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.armasSuerte[DataUsuario.armaActual]];
                DataUsuario.armasSuerte[DataUsuario.armaActual]++;
                DataUsuario.guardarDatos();

                t_creditos.text = DataUsuario.creditos.ToString();
                direccionSlider.value = DataUsuario.nivelesSuerte[DataUsuario.armaActual] / 10f;
                p_direccion.text = DataUsuario.costeMejoras[DataUsuario.armaActual, DataUsuario.armasSuerte[DataUsuario.armaActual]].ToString();
            }
            else
            {
                faltanMonedasPopUP.SetActive(true);
            }
        }
        else
        {
            p_direccion.text = "MAX";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
