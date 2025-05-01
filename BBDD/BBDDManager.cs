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
        
        public static BBDDManager Instance { get; private set; }
        
        private string _connectionString;
        private List<Puntuacion> puntuaciones = new List<Puntuacion>();

        public GameObject registroPuntuacionGameObject;
        [SerializeField] public Transform tablaPadre;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
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

        public void GetPuntuaciones()
        {
            puntuaciones.Clear();
            using (IDbConnection dbConexion = new SqliteConnection(_connectionString))
            {
                dbConexion.Open();
                using (IDbCommand comando = dbConexion.CreateCommand())
                {
                    comando.CommandText = "SELECT * FROM CLASIFICACION CLA ORDER BY CLA.PUNTUACION DESC;";
                    using (IDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Puntuacion puntuacionActual = new Puntuacion(lector.GetInt32(0),
                                lector.GetInt32(1), lector.GetString(2), lector.GetString(3));
                            puntuaciones.Add(puntuacionActual);
                        }
                        lector.Close();
                        dbConexion.Close();
                    }
                }
            }
        }

        public void insertarPuntuacion(int puntuacion, string nombreJugador, string nombreArma)
        {
            using (IDbConnection dbConexion = new SqliteConnection(_connectionString))
            {
                dbConexion.Open();
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

        public void reiniciarDatos()
        {
            using (IDbConnection conexion = new SqliteConnection(_connectionString))
            {
                conexion.Open();
                using (IDbCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = "DELETE FROM CLASIFICACION";
                    comando.ExecuteNonQuery();
                }
                conexion.Close();
            }
        }
        
        public void mostrarPuntuaciones()
        {
            foreach (Transform t in tablaPadre) Destroy(t.gameObject);
            puntuaciones.Clear();
            GetPuntuaciones();
            
            for (int i = 0; i < puntuaciones.Count; i++)
            {
                var datoActual = puntuaciones[i];
                GameObject gameObject = Instantiate(registroPuntuacionGameObject, tablaPadre, false);
                
                gameObject.GetComponent<PuntuacionMapper>()
                    .setPuntuacion("#" + (i + 1),
                        datoActual.puntuacion.ToString(),
                        datoActual.nombreJugador,
                        datoActual.nombreArmaUsada);
                
                gameObject.transform.SetAsLastSibling();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(tablaPadre.GetComponent<RectTransform>());
        }

    }
}