using System;
using Armeria;
using UnityEngine;
using UnityEngine.UI;

namespace Armas
{
    public class GestorCanon : MonoBehaviour
    {
        public GameObject bolaCanon;
        public Transform posicionDisparar;
        public float potencia;
        public float tiempoEnfriamiento;
        private float ultimoEnfriamiento;
        public Slider recargaSlider;
        public Camera playerCamera;
        public static Action disparoUsuario;

        void Update()
        {
            //Busamos el nivel de cadencia del arma actual
            int arma = DataUsuario.armaActual;
            int nivelCadencia = DataUsuario.nivelesCadencia[arma];
            
            float factorCadencia = 1f + nivelCadencia * 0.1f;
            
            float tiempoEnfriamientoAjustado = tiempoEnfriamiento / factorCadencia;
            tiempoEnfriamientoAjustado = Mathf.Max(0.1f, tiempoEnfriamientoAjustado);

            //Actualizamos el slider
            float tiempoTranscurrido = Time.time - ultimoEnfriamiento;
            recargaSlider.value = Mathf.Clamp01(tiempoTranscurrido / tiempoEnfriamientoAjustado);
            
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > ultimoEnfriamiento + tiempoEnfriamientoAjustado)
            {
                Disparar();
                ultimoEnfriamiento = Time.time;
            }
        }


        void Disparar()
        {
            //Se crea una copia de la bala del cañon (GameObject - Sphere) que está dentro del cañon
            GameObject balaCanon = Instantiate(bolaCanon, posicionDisparar.position, posicionDisparar.rotation);
            //Se le pone colisión
            Rigidbody rb = balaCanon.GetComponent<Rigidbody>();
            //Se le activa la gravedad
            rb.useGravity = true;
            //El disparo depende de la posición que mira la cámara del jugador HACIA DELANTE
            Vector3 disparo = playerCamera.transform.forward;
            //La velocidad con la que la copia de la bala sale disparada
            rb.linearVelocity = disparo * potencia;
            disparoUsuario?.Invoke();
        }
    }
}
