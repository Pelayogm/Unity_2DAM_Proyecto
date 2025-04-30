using UnityEngine;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

namespace Funciones.BBDD
{
    public class BBDDManager : MonoBehaviour
    {
        
        public static BBDDManager Instance { get; private set; }
        
        private string _connectionString;

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
        }

        public void GetPuntuaciones()
        {
            using (IDbConnection dbConexion = new SqliteConnection(_connectionString))
            {
                dbConexion.Open();
                using (IDbCommand comando = dbConexion.CreateCommand())
                {
                    comando.CommandText = "SELECT * FROM CLASIFICACION";
                    using (IDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Debug.Log(lector.GetString(3));
                        }
                        
                        dbConexion.Close();
                        lector.Close();
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
                    dbConexion.Close();
                }
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
            }
        }
    }
}