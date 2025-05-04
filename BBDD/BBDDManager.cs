using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

namespace Funciones.BBDD
{
    public class BBDDManager : MonoBehaviour
    {
        
        //Singleton para la BBDD
        public static BBDDManager Instance { get; private set; }
        
        //URL de la BBDD, se guarda en "appdata/LocalRow/Pelayogm"
        private string _connectionString;
        //Lista de Puntuaciones (Registro de la BBDD en el código).
        private List<Puntuacion> puntuaciones = new List<Puntuacion>();

        //Entrada de un registro en la tabla de Clasificación.
        public GameObject registroPuntuacionGameObject;
        
        //Tabla de Claisificación dónde van los registros.
        [SerializeField] public Transform tablaPadre;

        //El Awake() sólo se inicializa una vez, por lo tanto, nos aseguramos de una única instancia de un Objeto BBDDManager.
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
        //El Start() es solo la primera vez que accedemos al objeto en la ejecución.
        private void Start()
        {
            //Por tanto, es cuándo accedemos a la BBDD.
            string dbName = "Clasificacion.db";
            string sourcePath = Path.Combine(Application.streamingAssetsPath, dbName);
            string targetPath = Path.Combine(Application.persistentDataPath, dbName);
            
            if (!File.Exists(targetPath))
            {
                File.Copy(sourcePath, targetPath);
            }
            
            _connectionString = "URI=file:" + targetPath;

            //insertarPuntuacion(100, "Rober", "GANCHIILLO");
            GetPuntuaciones();
            mostrarPuntuaciones();
        }

        //GET de las puntuaciones a la BBDD,
        public void GetPuntuaciones()
        {
            
            //Eliminamos los registros de la tabla, por si acaso ya hemos mirado con anterioridad la Clasificación.
            foreach (Transform t in tablaPadre) Destroy(t.gameObject);
            
            //Limpiamos nuestra lista de puntuaciones.
            puntuaciones.Clear();
            
            //Nos conectamos a la BBDD
            using (IDbConnection dbConexion = new SqliteConnection(_connectionString))
            {
                //Abrimos la conexión.
                dbConexion.Open();
                
                //Preparamos para la query.
                using (IDbCommand comando = dbConexion.CreateCommand())
                {
                    //Creamos la query
                    comando.CommandText = "SELECT * FROM CLASIFICACION CLA ORDER BY CLA.PUNTUACION DESC;";
                    
                    //Volcamos los datos de la query en el variable "lector".
                    using (IDataReader lector = comando.ExecuteReader())
                    {
                        //Bucle para ir creando objetos "Puntuacion" y añadirlos en nuestra lista mientras haya registros.
                        while (lector.Read())
                        {
                            Puntuacion puntuacionActual = new Puntuacion(lector.GetInt32(0),
                                lector.GetInt32(1), lector.GetString(2), lector.GetString(3));
                            puntuaciones.Add(puntuacionActual);
                        }
                        
                        //Cerramos las conexiones.
                        lector.Close();
                        dbConexion.Close();
                    }
                }
            }
        }

        //PUT de una "Puntuacion" en la BBDD.
        public void insertarPuntuacion(int puntuacion, string nombreJugador, string nombreArma)
        {
                        
            //Nos conectamos a la BBDD
            using (IDbConnection dbConexion = new SqliteConnection(_connectionString))
            {
                
                //Preparamos para la query.
                dbConexion.Open();
                
                //Creamos la query con sus @param que son las variables de la partida (PUNTOS, NOMBRE, ARMA_USADA)
                using (IDbCommand comando = dbConexion.CreateCommand())
                {
                    comando.CommandText = 
                        "INSERT INTO CLASIFICACION (PUNTUACION, JUGADOR, ARMA_UTILIZADA) " +
                        "VALUES (@puntuacion, @jugador, @arma)";
                    
                    var param1 = comando.CreateParameter();
                    param1.ParameterName = "@puntuacion";
                    param1.Value = puntuacion;
                    comando.Parameters.Add(param1);
                    
                    var param2 = comando.CreateParameter();
                    param2.ParameterName = "@jugador";
                    param2.Value = nombreJugador;
                    comando.Parameters.Add(param2);
                    
                    var param3 = comando.CreateParameter();
                    param3.ParameterName = "@arma";
                    param3.Value = nombreArma;
                    comando.Parameters.Add(param3);

                    comando.ExecuteNonQuery();
                }
                dbConexion.Close();
            }   
        }

        //Un POST a la BBDD para reiniciar TODO.
        public void reiniciarDatos()
        {
                                    
            //Nos conectamos a la BBDD
            using (IDbConnection conexion = new SqliteConnection(_connectionString))
            {
                                
                //Preparamos para la query.
                conexion.Open();
                using (IDbCommand comando = conexion.CreateCommand())
                {
                    //Es un DELETE FROM, eliminaremos cualquier registro en la BBDD.
                    comando.CommandText = "DELETE FROM CLASIFICACION";
                    comando.ExecuteNonQuery();
                }
                conexion.Close();
            }
        }
        
        //Mostrar las puntuaciones en la tabla "Clasificacion".
        public void mostrarPuntuaciones()
        {
            //Limpiamos la lista en caso de si ha quedado algún registro no deseado.
            puntuaciones.Clear();
            //Cogemos todos los datos de nuevo para tener los más recientes.
            GetPuntuaciones();
            
            //Mientras haya datos en la lista iteramos.
            for (int i = 0; i < puntuaciones.Count; i++)
            {
                //Creamos un objeto temporar
                var datoActual = puntuaciones[i];
                
                //Creamos una copia de un registro
                GameObject gameObject = Instantiate(registroPuntuacionGameObject);
                
                //Mappeamos el contenido al objeto.
                gameObject.GetComponent<PuntuacionMapper>()
                    .setPuntuacion("#" + (i + 1),
                        datoActual.puntuacion.ToString(),
                        datoActual.nombreJugador,
                        datoActual.nombreArmaUsada);
                
                //Y le hacemos un append al padre como en XML.
                gameObject.transform.SetParent(tablaPadre);
            }

            //gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

    }
}