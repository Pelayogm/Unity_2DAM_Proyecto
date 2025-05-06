using System;
using System.Collections.Generic;
using Armeria;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Funciones.Menu
{
    public class ControladorMenuCentral : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private List<AudioClip> musicaAudioClips;
        private AudioSource audioSource;
        private int indiceMusica = DataUsuario.posicionMusica;
        
        [Header("UI")]
        [SerializeField] private TMP_Text textoMonedas;
        [SerializeField] private Button desactivarOlas;
        [SerializeField] private TMP_Text textoBoton;
        [SerializeField] private Animator animacionOlas;
        [SerializeField] private GameObject animacionGameObject;

        [Header("Variables")] 
        private bool animacion = DataUsuario.animacionOlas;
        
        private void Awake()
        {
            //Cargamos el índice de música que teníamos guardado.
            DataUsuario.cargarDatos();
            indiceMusica = DataUsuario.posicionMusica;
            animacion = DataUsuario.animacionOlas;
            if (!animacion)
            {
                animacionGameObject.SetActive(false);
                textoBoton.text = "ACTIVAR";
            }
            //Cogemos el componente AudioSource.
            audioSource = GetComponent<AudioSource>();
            //Buscamos la primera canción y la ponemos.
            audioSource.clip = musicaAudioClips[indiceMusica];
            audioSource.Play();
        }
        
        //Avanzamos la lista y en caso de poder salirnos del bucle lo controlamos y volvemos a partir desde 0.
        public void cambiarTema()
        {
            if (indiceMusica + 1 >= musicaAudioClips.Count)
            {
                indiceMusica = 0;
            }
            else
            {
                indiceMusica++;
            }
            //Paramos la canción anterior.
            audioSource.Stop();
            //Buscamos la nueva canción.
            audioSource.clip = musicaAudioClips[indiceMusica];
            //Y ponemos la nueva canción.
            audioSource.Play();
            
            //Guardamos él índice actualizado.
            DataUsuario.posicionMusica = indiceMusica;
            DataUsuario.guardarDatos();
        }

        //Según el valor de la variable invertimos o revertimos el texto y la animación.
        public void animacionBoton()
        {
            if (!animacion)
            {
                animacion = true;
                animacionGameObject.SetActive(true);
                textoBoton.text = "DESACTIVAR";
                DataUsuario.animacionOlas = animacion;
                DataUsuario.guardarDatos();
                Update();
            }
            else
            {
                animacion = false;
                animacionGameObject.SetActive(false);
                textoBoton.text = "ACTIVAR";
                DataUsuario.animacionOlas = animacion;
                DataUsuario.guardarDatos();
                Update();
            }
        }
        
        

        void Start()
        {
            monedasUsuarioMenu();
        }

        //Método para cargar las monedas del menú y pasarlas a la interfaz.
        public void monedasUsuarioMenu()
        {
            DataUsuario.cargarDatos();
            int creditosActuales = DataUsuario.creditos;
            if (creditosActuales > 10000)
            {
                textoMonedas.text = "9999+";
            }
            else
            {
                textoMonedas.text = creditosActuales.ToString();
            }
        }
        
        //Cerrar juego
        public void cerrarJuego()
        {
            Application.Quit();
        }

        //Abrir el menú de arma
        public void abrirArmeria()
        {
            SceneManager.LoadScene("Armeria_definitiva");
        }

        //Empezar una partida nueva.
        public void empezarPartida()
        {
            abrirArmeria();
        }
        
        //Bucle gracias a Update() para mantener constantemente actualizadas las monedas del usuario.
        private void Update()
        {
            textoMonedas.text = DataUsuario.creditos.ToString();
        }
    }

}