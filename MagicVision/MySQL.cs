using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;


namespace PoolVision
{
    public class MySqlClient
    {
        private MySqlConnection sql;

        public DataRow dbRow(String query)
        {
            MySqlCommand command = sql.CreateCommand();
            command.CommandText = query;

            DataTable selectDT = new DataTable();
            MySqlDataAdapter dataAd = new MySqlDataAdapter(command);

            dataAd.Fill(selectDT);

            if (selectDT.Rows.Count > 0)
                return selectDT.Rows[0];
            else
                return null;
        }

        public int lastInsertId()
        {
            DataRow r = dbRow("SELECT last_insert_id() as lid");

            Int64 id = (Int64)r[0];

            return (int)id;
        }

        public int affectedRows()
        {
            DataRow r = dbRow("SELECT ROW_COUNT()");
            int id = (int)r[0];

            return id;
        }

        public DataTable dbResult(String query)
        {
            MySqlCommand command = sql.CreateCommand();
            command.CommandText = query;

            DataTable selectDT = new DataTable();
            MySqlDataAdapter dataAd = new MySqlDataAdapter(command);

            dataAd.Fill(selectDT);

            return selectDT;

        }

        internal int dbNone(string query)
        {
            MySqlCommand command = sql.CreateCommand();
            //MySqlDataReader Reader;
            command.CommandText = query;
            return command.ExecuteNonQuery();
        }

        public MySqlClient(String SqlConString)
        {
            sql = new MySqlConnection(SqlConString);
            sql.Open();
        }

        public DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
