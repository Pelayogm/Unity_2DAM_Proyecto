using UnityEngine;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

namespace Funciones.BBDD
{
    public class BBDDManager : MonoBehaviour
    {
        private string _connectionString;

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

            GetPuntuaciones();
        }

        private void GetPuntuaciones()
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
    }
}