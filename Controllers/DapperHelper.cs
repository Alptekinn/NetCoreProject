using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace PetimOl.Controllers
{
    public interface IDapperHelper
    {
        List<T> GetList<T>(string sql, object? param = null);
        bool ExecuteScalar(string sql, object? param = null);
        int Execute(string sql, object? param = null);
        T QueryFirst<T>(string sql, object? param = null);
    }

    public class DapperHelper : IDapperHelper
    {
        private static string ConnectionString = "Server=.;Database=PetDB;Trusted_Connection=True;";

        public List<T> GetList<T>(string sql, object? param = null)
        {
            try
            {
                IDbConnection conn = new SqlConnection(ConnectionString);
                conn.Open();

                return conn.Query<T>(sql, param).ToList();
            }
            catch (Exception e)
            {

                throw (e);
            }
        }

        public bool ExecuteScalar(string sql, object? param = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.ExecuteScalar<bool>(sql, param);
            }
        }
        public int Execute(string sql, object? param = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Execute(sql, param);
            }
        }
        public T QueryFirst<T>(string sql, object? param = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.QueryFirst<T>(sql, param);
            }
        }
    }
}
