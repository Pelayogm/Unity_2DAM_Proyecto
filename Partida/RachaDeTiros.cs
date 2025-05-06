using System.Collections;
using Funciones.Partida;
using TMPro;
using UnityEngine;

public class RachaDeTiros : MonoBehaviour
{
    [Header("Variables de la racha de tiros.")]
    public int rachaActual;
    private int ultimaRachaProcesada = 0;
    float tiempoReincioRacha = 5f;

    [Header("UI del juego.")]
    public GameObject impactoTexto;
    public GameObject eliminacionTexto;
    public GameObject rachaTextoObjeto;
    public TextMeshProUGUI rachaTexto;

    [Header("Singleton de la racha de tiros.")]
    public static RachaDeTiros Instance { get; private set; }

    //Inicializamos la instancia de la racha de tiros.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Método para un impacto en un enemigo.
    public void impactoEnEnemigo()
    {
        //Activamos el GameObject para poner texto
        impactoTexto.SetActive(true);
        //Se llama a la transición de esperar().
        //Es el tiempo en el que estará por pantalla con el texto de impacto.
        StartCoroutine(esperar(impactoTexto));
        //Se aumenta el tiempo de reinicio de la racha por el impacto.
        tiempoReincioRacha += 4f;
    }

    //Método para la eliminación de un enemigo.
    public void eliminacion()
    {
        //Aumentamos el contador de eliminaciones de la racha.
        rachaActual++;
        //Activamos el GameObject para poner texto
        eliminacionTexto.SetActive(true);
        //Se llama a la transición de esperar().
        //Es el tiempo en el que estará por pantalla con el texto de eliminación.
        StartCoroutine(esperar(eliminacionTexto));
        //Se aumenta el tiempo de reinicio de la racha por la eliminación.
        tiempoReincioRacha += 10f;
        //Se comienza con la racha.
        ProcesarRacha();
    }

    //Método para reiniciar la racha para volver a empezar.
    public void reiniciarRacha()
    {
        rachaActual = 0;
        ultimaRachaProcesada = 0;
        tiempoReincioRacha = 0f;
    }

    //Transición que mantiene el texto en pantalla 1f.
    private IEnumerator esperar(GameObject objeto)
    {
        yield return new WaitForSeconds(1f);
        objeto.SetActive(false);
    }

    void Update()
    {
        // Decrementamos con independencia de fps
        tiempoReincioRacha -= Time.deltaTime;

        //Con el Update() gracias a su constante actualización controlamos que la racha no 
        //dure más de lo que deba.
        if (tiempoReincioRacha > 0f)
        {
            rachaTextoObjeto.SetActive(true);
        }
        else
        {
            ResetRacha();
        }
    }

    private void ProcesarRacha()
    {
        // Solo procesamos cada nivel de racha una vez
        if (rachaActual == ultimaRachaProcesada) return;
        ultimaRachaProcesada = rachaActual;
        //Activamos el texto dónde se contabilizará la racha.
        rachaTextoObjeto.SetActive(true);

        //Switch con las casuísticas.
        //Según el número de la racha, se irá actualizando el texto y sumando puntuación extra
        //por los tiros encadenados.
        switch (rachaActual)
        {
            case 0:
                rachaTexto.SetText("");
                break;
            case 1:
                rachaTexto.SetText("Enemigo Eliminado");
                break;
            case 2:
                rachaTexto.SetText("Eliminación doble");
                PuntuacionManager.Instance.aumentarPuntuacion(125);
                break;
            case 3:
                rachaTexto.SetText("Eliminación triple");
                PuntuacionManager.Instance.aumentarPuntuacion(250);
                break;
            case 4:
                rachaTexto.SetText("Enemigos Arrasados");
                PuntuacionManager.Instance.aumentarPuntuacion(430);
                break;
            case 5:
                rachaTexto.SetText("Dominación total");
                PuntuacionManager.Instance.aumentarPuntuacion(600);
                break;
            case 6:
                rachaTexto.SetText("Eliminación maestra");
                PuntuacionManager.Instance.aumentarPuntuacion(800);
                break;
        }
    }

    //Reinicio completo de la instancia para cuando se acaba una racha por tiempo.
    private void ResetRacha()
    {
        rachaActual = 0;
        ultimaRachaProcesada = 0;
        tiempoReincioRacha = 0f;
        rachaTextoObjeto.SetActive(false);
        rachaTexto.SetText("");
    }
}
