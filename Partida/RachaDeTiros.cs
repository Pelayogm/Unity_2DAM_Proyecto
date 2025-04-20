using System.Collections;
using Funciones.Partida;
using TMPro;
using UnityEngine;

public class RachaDeTiros : MonoBehaviour
{
    public int rachaActual;
    private int ultimaRachaProcesada = 0;
    float tiempoReincioRacha = 5f;

    //UI
    public GameObject impactoTexto;
    public GameObject eliminacionTexto;
    public GameObject rachaTextoObjeto;
    public TextMeshProUGUI rachaTexto;

    // SINGLETON
    public static RachaDeTiros Instance { get; private set; }

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

    public void impactoEnEnemigo()
    {
        impactoTexto.SetActive(true);
        StartCoroutine(esperar(impactoTexto));
        tiempoReincioRacha += 4f;
    }

    public void eliminacion()
    {
        rachaActual++;
        eliminacionTexto.SetActive(true);
        StartCoroutine(esperar(eliminacionTexto));
        tiempoReincioRacha += 15f;
        ProcesarRacha();
    }

    public void reiniciarRacha()
    {
        rachaActual = 0;
        ultimaRachaProcesada = 0;
        tiempoReincioRacha = 0f;
    }

    private IEnumerator esperar(GameObject objeto)
    {
        yield return new WaitForSeconds(1f);
        objeto.SetActive(false);
    }

    void Update()
    {
        // Decrementamos con independencia de fps
        tiempoReincioRacha -= Time.deltaTime;

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
        rachaTextoObjeto.SetActive(true);

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

    private void ResetRacha()
    {
        rachaActual = 0;
        ultimaRachaProcesada = 0;
        tiempoReincioRacha = 0f;
        rachaTextoObjeto.SetActive(false);
        rachaTexto.SetText("");
    }
}
