using System;
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
            float tiempoTranscurrido = Time.time - ultimoEnfriamiento;
            recargaSlider.value = Mathf.Clamp01(tiempoTranscurrido / tiempoEnfriamiento);
            
            //Si se pulsa el espacio y el tiempo desde el último disparo supera el tiempo de enfriamiento.
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > ultimoEnfriamiento + tiempoEnfriamiento)
            {
                //Se llama a disparar
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
