using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Funciones.Partida
{
    public class EnemigoDefecto : MonoBehaviour
    {
        
        [Header("Objetos de la partida")]
        [SerializeField] public BarraVida barraVida;
        [SerializeField] public GameObject enemigo;
        [SerializeField] public Temporizador temporizador;
        
        [Header("Propiedades de vida del enemigo")]
        [SerializeField] private int saludInicial;
        public int salud = 100;
        
        [FormerlySerializedAs("_probabilidadRegeneracion")]
        [Header("Comportamiento variado: Regeneración de vida")]
        [Range(0f, 1f)]
        public float probabilidadRegeneracion = 0.6f;
        private float _maxPorcentajeCuracion = 0.4f;
        private float _duracionCuracion = 5f;
        private float _intervalo = 0.5f;
        private bool _regenera;
        
        [Header("Comportamiento variado: Animaciones de movimiento")]
        [Range(0f, 1f)]
        public float probabilidadAnimacion = 0.5f;
        private float minVelocidadAnimacion = 0.5f;
        private float maxVelocidadAnimacion = 1.5f;

        private bool enemigoTieneAnimacionAsignada;
        private bool originalenemigoTeniaAnimacion;
        private Animator _animator;
        
        [Header("Booleano que controla si varia su comportamiento o no")]
        private bool comportamientoVariado = false;

        void Awake()
        {
            //Vida
            saludInicial = salud;
            //Comportamiento variado: Regeneración de vida probabilidades
            _regenera = UnityEngine.Random.value < probabilidadRegeneracion;
            //Comportamiento variado: Animaciones de movimiento probabilidades
            enemigoTieneAnimacionAsignada = UnityEngine.Random.value < probabilidadAnimacion;
            originalenemigoTeniaAnimacion = enemigoTieneAnimacionAsignada;
            
            //Un enemigo puede no tener animación asignada, por lo tanto, lo comprobamos.
            if (TryGetComponent(out Animator animator))
            {
                //Si la tiene asignamos su Animator para controlarlo
                _animator = animator;
                animator.enabled = enemigoTieneAnimacionAsignada;
            }

        }

        void Update()
        {
            //Como Update() actualiza cada segundo, actualizamos nuestra variable del tiempo siempre para que estén sincronizadas.
            //Debido a que es poca carga en el sistema.
            var tiempoActual = temporizador.getTiempoRestante();

            //Cuando el tiempo es menos de 40s y su booleano para que se comporte aleatorio sea falso, para evitar bucles
            //Empezamos a variar el comportamiento.
            if (tiempoActual < 40 && comportamientoVariado == false)
            {
                //Establecemos el booleano a true para evitar que vuelvan a entrar en el siguiente frame.
                comportamientoVariado = true;
                
                if (_regenera)
                {
                    StartCoroutine(CurarProgresivo());
                }

                //Si el enemigo tiene animación entramos en el bucle.
                if (_animator is not null)
                {
                    //Activamos su Animator 
                    _animator.enabled = true;
                    
                    //Si ya tenía la animación activada antes, la modificaremos.
                    if (originalenemigoTeniaAnimacion)
                    {
                        //Cogemos aleatoriamente entre las variables de mínimo y de máximo para obtener un valor que nos servirá para la propiedad
                        //animator.speed
                        var multiplicadorDeMovimiento = UnityEngine.Random.Range(minVelocidadAnimacion, maxVelocidadAnimacion);
                        //Ahora multiplicamos el valor de la velocidad base del animator con nuestra variable, el resultado
                        //lo asignaremos a nuestro animator.
                        _animator.speed *= multiplicadorDeMovimiento;
                    }
                }
            }
        }
        
        private IEnumerator CurarProgresivo()
        {
            var porcentaje = UnityEngine.Random.Range(0f, _maxPorcentajeCuracion);
            var cantidadTotal = Mathf.CeilToInt(salud * porcentaje);

            var pasos = Mathf.CeilToInt(_duracionCuracion / _intervalo);
            var porPaso = Mathf.CeilToInt((float)cantidadTotal / pasos);

            var curadoAcumulado = 0;

            for (int i = 0; i < pasos && curadoAcumulado < cantidadTotal; i++)
            {
                yield return new WaitForSeconds(_intervalo);
                var restante = cantidadTotal - curadoAcumulado;
                var curacionTick = Mathf.Min(porPaso, restante);
                salud = Mathf.Min(salud + curacionTick, saludInicial);
                curadoAcumulado += curacionTick;
                barraVida.Curar(curacionTick);
            }
        }

        public void recibirImpacto(int cantidad)
        {
            print(salud);
            print(cantidad);
            if (salud > 0)
            {
                salud -= cantidad;
                
                if (barraVida != null)
                {
                    barraVida.Impactar(cantidad);
                }

                RachaDeTiros.Instance.impactoEnEnemigo();
                PuntuacionManager.Instance.aumentarPuntuacion(25);
                
            }
            
            if (salud <= 0)
            {
                desactivar();
                RachaDeTiros.Instance.eliminacion();
                PuntuacionManager.Instance.aumentarPuntuacion(100);
            }
            
        }

        private void desactivar()
        {
            Destroy(enemigo);
        }
    }
}