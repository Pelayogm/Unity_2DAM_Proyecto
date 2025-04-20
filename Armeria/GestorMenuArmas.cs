using System;
using System.Linq;
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
    int totalArmas = armasDisponibles.Length;
    if (posicionArma < 0 || posicionArma >= totalArmas)
    {
        Debug.LogError($"Índice de arma inválido: {posicionArma} (debe estar entre 0 y {totalArmas - 1}).");
        return;
    }
    
    DataUsuario.armaActual = posicionArma;
    
    for (int i = 0; i < totalArmas; i++)
        armasDisponibles[i].SetActive(i == posicionArma);
    
    bool desbloqueada = posicionArma < DataUsuario.armasDesbloqueadas.Length
                        && DataUsuario.armasDesbloqueadas[posicionArma] == 1;

    if (!desbloqueada)
    {
        botonComprar.SetActive(true);
        botonEscoger.SetActive(false);
        estadisticasBloqueadas.SetActive(true);
        estadisticasDesbloqueadas.SetActive(false);
        
        if (posicionArma < DataUsuario.precioArmas.Length)
            p_arma.text = DataUsuario.precioArmas[posicionArma].ToString();
        else
            p_arma.text = "N/A";
    }
    else
    {
        botonComprar.SetActive(false);
        botonEscoger.SetActive(true);
        estadisticasBloqueadas.SetActive(false);
        estadisticasDesbloqueadas.SetActive(true);
        
        int nivelPot  = Mathf.Clamp(
                            (posicionArma < DataUsuario.nivelesPotencia.Length)
                            ? DataUsuario.nivelesPotencia[posicionArma]
                            : 0,
                            0,
                            DataUsuario.costeMejoras.GetLength(1) - 1);
        int nivelCad  = Mathf.Clamp(
                            (posicionArma < DataUsuario.nivelesCadencia.Length)
                            ? DataUsuario.nivelesCadencia[posicionArma]
                            : 0,
                            0,
                            DataUsuario.costeMejoras.GetLength(1) - 1);
        int nivelSue  = Mathf.Clamp(
                            (posicionArma < DataUsuario.nivelesSuerte.Length)
                            ? DataUsuario.nivelesSuerte[posicionArma]
                            : 0,
                            0,
                            DataUsuario.costeMejoras.GetLength(1) - 1);
        
        potenciaSlider.value = (posicionArma < DataUsuario.armasPotencia.Length)
                                ? DataUsuario.armasPotencia[posicionArma] / 10f
                                : nivelPot / 10f;
        cadenciaSlider.value = (posicionArma < DataUsuario.armasCadencia.Length)
                                ? DataUsuario.armasCadencia[posicionArma] / 10f
                                : nivelCad / 10f;
        direccionSlider.value = (posicionArma < DataUsuario.armasSuerte.Length)
                                ? DataUsuario.armasSuerte[posicionArma] / 10f
                                : nivelSue / 10f;

        int maxNivel   = DataUsuario.costeMejoras.GetLength(1) - 1;
        int costePot   = DataUsuario.costeMejoras[posicionArma, nivelPot];
        int costeCad   = DataUsuario.costeMejoras[posicionArma, nivelCad];
        int costeDirec = DataUsuario.costeMejoras[posicionArma, nivelSue];
        
        //print($"[EscogerArma] Arma {posicionArma}: Niveles P/C/S = {nivelPot}/{nivelCad}/{nivelSue}");
        //print($"           Costes P/C/S = {costePot}/{costeCad}/{costeDirec}");
        p_potencia .text = (nivelPot  < maxNivel) ? costePot.ToString()   : "MAX";
        p_cadencia .text = (nivelCad  < maxNivel) ? costeCad.ToString()   : "MAX";
        p_direccion.text = (nivelSue  < maxNivel) ? costeDirec.ToString() : "MAX";
    }
    
    n_arma.text = DataUsuario.nombresArmas.ElementAtOrDefault(posicionArma)   ?? "Desconocido";
    descripcionArma.text = DataUsuario.descripcionesArmas.ElementAtOrDefault(posicionArma) ?? "";
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
        int arma = DataUsuario.armaActual;
        int nivelActual = DataUsuario.nivelesPotencia[arma];
        int maxNivel = DataUsuario.costeMejoras.GetLength(1) - 1; 
        
        if (nivelActual < 0 || nivelActual > maxNivel)
        {
            p_potencia.text = "MAX"; 
            return;
        }

        int costeMejora = DataUsuario.costeMejoras[arma, nivelActual];
        if (costeMejora > DataUsuario.creditos)
        {
            faltanMonedasPopUP.SetActive(true);
            return;
        }
        
        DataUsuario.creditos -= costeMejora;
        DataUsuario.nivelesPotencia[arma]++;
        DataUsuario.armasPotencia[arma]++;
        DataUsuario.guardarDatos();      
        
        t_creditos.text  = DataUsuario.creditos.ToString();
        potenciaSlider.value = DataUsuario.nivelesPotencia[arma] / 10f;
        p_potencia.text  = (nivelActual + 1 <= maxNivel) ? DataUsuario.costeMejoras[arma, nivelActual + 1].ToString() : "MAX";
    }
    
    public void comprarMejoraCadencia()
    {
        int arma = DataUsuario.armaActual;
        int nivelActual = DataUsuario.nivelesCadencia[arma];
        int maxNivel = DataUsuario.costeMejoras.GetLength(1) - 1; 
        
        if (nivelActual < 0 || nivelActual > maxNivel)
        {
            p_cadencia.text = "MAX"; 
            return;
        }

        int costeMejora = DataUsuario.costeMejoras[arma, nivelActual];
        if (costeMejora > DataUsuario.creditos)
        {
            faltanMonedasPopUP.SetActive(true);
            return;
        }
        
        DataUsuario.creditos -= costeMejora;
        DataUsuario.nivelesCadencia[arma]++;
        DataUsuario.armasCadencia[arma]++;
        DataUsuario.guardarDatos();      
        
        t_creditos.text  = DataUsuario.creditos.ToString();
        cadenciaSlider.value = DataUsuario.nivelesCadencia[arma] / 10f;
        p_cadencia.text  = (nivelActual + 1 <= maxNivel) ? DataUsuario.costeMejoras[arma, nivelActual + 1].ToString() : "MAX";
    }

    
    public void comprarMejoraSuerte()
    {
        int arma = DataUsuario.armaActual;
        int nivelActual = DataUsuario.nivelesSuerte[arma];
        int maxNivel = DataUsuario.costeMejoras.GetLength(1) - 1; 
        
        if (nivelActual < 0 || nivelActual > maxNivel)
        {
            p_direccion.text = "MAX"; 
            return;
        }

        int costeMejora = DataUsuario.costeMejoras[arma, nivelActual];
        if (costeMejora > DataUsuario.creditos)
        {
            faltanMonedasPopUP.SetActive(true);
            return;
        }
        
        DataUsuario.creditos -= costeMejora;
        DataUsuario.nivelesSuerte[arma]++;
        DataUsuario.armasSuerte[arma]++;
        DataUsuario.guardarDatos();      
        
        t_creditos.text  = DataUsuario.creditos.ToString();
        direccionSlider.value = DataUsuario.nivelesSuerte[arma] / 10f;
        p_direccion.text  = (nivelActual + 1 <= maxNivel) ? DataUsuario.costeMejoras[arma, nivelActual + 1].ToString() : "MAX";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
