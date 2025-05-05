using System;
using Armeria;
using UnityEngine;
using UnityEngine.UI;

namespace Armas
{
    public class GestorCanon : MonoBehaviour
    {
        [Header("Elementos relacionados con la munición")]
        public GameObject bolaCanon;
        public Transform posicionDisparar;
        
        [Header("Valores de la munición")]
        public float potencia;
        public float tiempoEnfriamiento;
        private float ultimoEnfriamiento;
        
        [Header("IU")]
        public Slider municionSlider;
        
        [Header("Posición de la camara para calcular la dirección de la bala")]
        public Camera playerCamera;
        
        [Header("Acción de disparar")]
        public static Action disparoUsuario;

        [Header("Sonidos")] 
        [SerializeField] private AudioClip sonidoDisparo;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            //Buscamos el nivel de cadencia del arma actual.
            var arma = DataUsuario.armaActual;
            var nivelCadencia = DataUsuario.nivelesCadencia[arma];
            
            //Calculamos el tiempo de recarga.
            var factorCadencia = 1f + nivelCadencia * 0.1f;
            
            //Y al dividirlo por el tiempo de enfriamiento, al ser él dividiendo cada vez más grande, el cociente es
            //cada vez más pequeño, por tanto, el tiempo se reduce.
            var tiempoEnfriamientoAjustado = tiempoEnfriamiento / factorCadencia;
            //"Clampamos" el dato, es decir le ponemos un mínimo para que no se salga de los límites.
            tiempoEnfriamientoAjustado = Mathf.Max(0.1f, tiempoEnfriamientoAjustado);

            //Actualizamos el slider con el valor calculado
            var tiempoTranscurrido = Time.time - ultimoEnfriamiento;
            //Y el valor del slider se actualiza.
            municionSlider.value = Mathf.Clamp01(tiempoTranscurrido / tiempoEnfriamientoAjustado);
            
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > ultimoEnfriamiento + tiempoEnfriamientoAjustado)
            {
                Disparar();
                ultimoEnfriamiento = Time.time;
            }
        }


        void Disparar()
        {
            //Se crea una copia de la bala del cañon (GameObject - Sphere) que está dentro del cañon partiendo desde la zona de
            //instanciación.
            GameObject balaCanon = Instantiate(bolaCanon, posicionDisparar.position, posicionDisparar.rotation);
            //Se le pone colisión a la munición.
            Rigidbody rb = balaCanon.GetComponent<Rigidbody>();
            //Se le activa la gravedad para que imapcte.
            rb.useGravity = true;
            //Le ponemos a AudioSource el sonido
            audioSource.clip = sonidoDisparo;
            //Ejecutamos el sonido
            audioSource.Play();
            //El disparo depende de la posición que mira la cámara del jugador HACIA DELANTE.
            Vector3 disparo = playerCamera.transform.forward;
            //La velocidad con la que la copia de la bala sale disparada.
            rb.linearVelocity = disparo * potencia;
            
            disparoUsuario?.Invoke();
        }
    }
}
