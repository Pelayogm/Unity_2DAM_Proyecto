using System.Collections;
using Funciones.Partida;
using TMPro;
using UnityEngine;

public class RachaDeTiros : MonoBehaviour
{
    public int rachaActual;
    public GameObject impactoTexto;
    float tiempoReincioRacha = 5f;
    
    public GameObject eliminacionTexto;
    public GameObject rachaTextoObjeto;
    public TextMeshProUGUI rachaTexto;
    
    //SINGLETON
    public static RachaDeTiros Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public void reiniciarRacha()
    {
        rachaActual = 0;
        tiempoReincioRacha = 0f;
    }

    private IEnumerator esperar(GameObject objeto)
    {
        yield return new WaitForSeconds(1);
        objeto.SetActive(false);
    }
    
    public void eliminacion()
    {
        rachaActual = rachaActual + 1;
        eliminacionTexto.SetActive(true);
        StartCoroutine(esperar(eliminacionTexto));
        tiempoReincioRacha += 15f;
        print(rachaActual);
    }

    //Arreglar lo de los puntos suman infinitamente puntos por el bucle al estar en Update()
    void Update()
    {
        if (tiempoReincioRacha >= 0)
        {
            rachaTextoObjeto.SetActive(true);
        }
        
        if (tiempoReincioRacha <= 0)
        {
            rachaTextoObjeto.SetActive(false);
            rachaActual = 0;
        }
        tiempoReincioRacha -= 0.01f;

        if (rachaActual == 0) {
            rachaTexto.SetText("");
        }

        if (rachaActual == 1)
        {
            rachaTexto.SetText("Enemigo Eliminado");
            
        }
        
        if (rachaActual == 2)
        {
            rachaTexto.SetText("Eliminación doble");
            PuntuacionManager.Instance.aumentarPuntuacion(125);
        }
        
        if (rachaActual == 3)
        {
            rachaTexto.SetText("Eliminación triple");
            PuntuacionManager.Instance.aumentarPuntuacion(250);
        }
        
        if (rachaActual == 4)
        {
            rachaTexto.SetText("Enemigos Arrasados");
            PuntuacionManager.Instance.aumentarPuntuacion(430);
        }
        
        if (rachaActual == 5)
        {
            rachaTexto.SetText("Dominación total");
            PuntuacionManager.Instance.aumentarPuntuacion(600);
        }
        
        if (rachaActual == 6)
        {
            rachaTexto.SetText("Eliminación maestra");
            PuntuacionManager.Instance.aumentarPuntuacion(800);
        }
    }
    
    
}
