using UnityEngine;

namespace Armeria
{
    public class DataUsuario : MonoBehaviour
    {
        public static string [] nombresArmas = {"El Patriota", "Carabina R5C", "Fusil K5", "Commando 5", "Escopeta de Asalto"};

        public static string [] clasesArmas = { "Potencia", "Respuesta rápida", "Respuesta rápida", "Larga distancia", "Asalto" };

        public static string[] descripcionesArmas =
        {
            "Desde Bosnia con amor… y mucha artillería. Forjado en la guerra, hecho para arrasar, su legado sigue reduciendo todo a polvo.",
            "Arma de última factura. Se comenta que el FBI la esta buscando porque la han perdido...",
            "Primo de la M4A1, con la misma precisión y velocidad, pero con una inyeccion extra de potencia por HK. Ágil, letal y siempre listo para arrasar en cualquier batalla.",
            "El rifle preferido de la marina alemana. Compacto, preciso y letal, diseñado para misiones rápidas y combate cercano.",
            "La candidata ideal para el caos. Potente y letal, perfecta para enfrentar el peligro cara a cara."
        };
        
        // O = Arma Bloqueada | 1 = Arma Desbloqueada
        public static int [] armasDesbloqueadas = { 1, 0, 0, 0, 0 };
        
        //Niveles de las armas
        public static int [] armasCadencia = { 1, 2, 2, 2, 1 };
        public static int [] armasPotencia = { 2, 1, 1, 1, 3 };
        public static int [] armasSuerte = { 3, 1, 3, 1, 5 };
        
        //Precios para comprar Armas
        public static int [] precioArmas = { 0, 100, 300, 250, 700};
        
        //Precios de las mejoras de las armas
        public static int [,] costeMejoras = {
        { 0, 25, 50, 150, 400 },
        { 0, 25, 70, 100, 200 },
        { 0, 15, 30, 100, 200 },
        { 0, 25, 50, 110, 200 },
        { 0, 80, 130, 180, 300 }};
        
        //Array para saber las mejoras de las armas
        public static int [] nivelesCadencia = { 1, 1, 1, 1, 1 };
        public static int [] nivelesPotencia = { 1, 1, 1, 1, 1 };
        public static int [] nivelesSuerte = { 1, 1, 1, 1, 1 };

        //Creditos (Moneda interna del juego)
        public static int creditos;
        
        //Posicion del arma que tiene el jugador selccionada
        public static int armaActual;
        
        //FPS
        public static int posicionSelector = 1;
        public static int fps = 60;
        
        //Config extra
        public static int posicionFondos = 0;
        public static int posicionMusica = 0;
        public static bool animacionOlas = true;

        //Guardar los datos en el PlayerPrefs
        public static void guardarDatos()
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("armasDesbloqueadas" + i.ToString(), armasDesbloqueadas[i]);
                
                PlayerPrefs.SetInt("armasCadencia" + i.ToString(), armasCadencia[i]);
                PlayerPrefs.SetInt("armasPotencia" + i.ToString(), armasPotencia[i]);
                PlayerPrefs.SetInt("armasSuerte" + i.ToString(), armasSuerte[i]);
                
                PlayerPrefs.SetInt("nivelesCadencia" + i.ToString(), nivelesCadencia[i]);
                PlayerPrefs.SetInt("nivelesPotencia" + i.ToString(), nivelesPotencia[i]);
                PlayerPrefs.SetInt("nivelesSuerte" + i.ToString(), nivelesSuerte[i]);
            }
            PlayerPrefs.SetInt("ArmasCompradas" + 0, 1);
            PlayerPrefs.SetInt("Creditos", creditos);
            PlayerPrefs.SetInt("PosicionFPS", posicionSelector);
            PlayerPrefs.SetInt("FPS", fps);
            PlayerPrefs.SetInt("IndiceFondo", posicionFondos);
            PlayerPrefs.SetInt("IndiceMusica", posicionMusica);
            PlayerPrefs.SetInt("Animacion", (animacionOlas ? 1 : 0));
        }
        
        
        //Sacar los datos que se guardan con el metodo guardarDatos()
        public static void cargarDatos()
        {
            for (int i = 0; i < 5; i++)
            {
                armasDesbloqueadas[i] = PlayerPrefs.GetInt("armasDesbloqueadas" + i.ToString(), 0);
                
                armasCadencia[i] = PlayerPrefs.GetInt("armasCadencia" + i.ToString(), 1);
                armasPotencia[i] = PlayerPrefs.GetInt("armasPotencia" + i.ToString(), 1);
                armasSuerte[i] = PlayerPrefs.GetInt("armasSuerte" + i.ToString(), 1);
                
                nivelesCadencia[i] = PlayerPrefs.GetInt("nivelesCadencia" + i.ToString(), 1);
                nivelesPotencia[i] = PlayerPrefs.GetInt("nivelesPotencia" + i.ToString(), 1);
                nivelesSuerte[i] = PlayerPrefs.GetInt("nivelesSuerte" + i.ToString(), 1);
            }

            armasDesbloqueadas[0] = 1;
            creditos = PlayerPrefs.GetInt("Creditos", creditos);
            posicionSelector = PlayerPrefs.GetInt("PosicionFPS");
            fps = PlayerPrefs.GetInt("FPS");
            posicionFondos = PlayerPrefs.GetInt("IndiceFondo");
            posicionMusica = PlayerPrefs.GetInt("IndiceMusica");
            animacionOlas = (PlayerPrefs.GetInt("Animacion") != 0);

        }
        
        public static void ReiniciarDatos()
        {
            creditos = 0;
            armaActual = 0;
            
            armasDesbloqueadas = new [] { 1, 0, 0, 0, 0 };
            
            armasCadencia = new [] { 1, 2, 2, 2, 0 };
            armasPotencia = new [] { 2, 1, 1, 1, 3 };
            armasSuerte = new [] { 3, 1, 3, 1, 5 };
            
            nivelesCadencia = new [] { 1, 1, 1, 1, 1 };
            nivelesPotencia = new [] { 1, 1, 1, 1, 1 };
            nivelesSuerte = new [] { 1, 1, 1, 1, 1 };
            posicionSelector = 1;
            fps = 60;
            posicionFondos = 0;
            posicionMusica = 0;
            animacionOlas = true;
            
            guardarDatos();
        }
    }
    
}

    
