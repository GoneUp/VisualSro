using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MySql.Data.MySqlClient;

namespace Framework
{
    public class cMySQL : IDisposable
    {
        MySqlConnection myConnection;
        object myLock;

        public void Connect(string server, string database, string user, string password)
        {
            myConnection = new MySqlConnection();
            myConnection.ConnectionString = string.Format("server={0};database={1};uid={2};pwd={3};", server, database, user, password);
            myLock = new object();

            try
            {
                myConnection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool Test(string server, string uid, string pwd, string database)
        {
            myConnection = new MySqlConnection(String.Format("server={0};uid={2};pwd={3};database={4};", server, uid, pwd, database));
            myLock = new object();
            try
            {
                myConnection.Open();

                #region TableCheck

                //Check tables.                
                //myConnection.ChangeDatabase("information_schema");
                //var data = Select("SELECT * FROM `TABLES` WHERE `TABLE_SCHEMA` = '{0}' ", database);

                //bool tableMissing = false;
                //List<string> myTables = new List<string>();
                //foreach (var item in data)
                //{
                //    myTables.Add(Convert.ToString(item["TABLE_NAME"]));
                //}

                ////CHECK
                //if (myTables.Contains("account") == false)
                //{
                //    Console.WriteLine("'account' table not found.");
                //    tableMissing = true;
                //}
                ////==================================================
                //if (myTables.Contains("ban") == false)
                //{
                //    Console.WriteLine("'ban' table not found.");
                //    tableMissing = true;
                //}
                ////==================================================
                //if (myTables.Contains("character") == false)
                //{
                //    Console.WriteLine("'character' table not found.");
                //    tableMissing = true;
                //}
                ////==================================================
                //if (myTables.Contains("inventory") == false)
                //{
                //    Console.WriteLine("'inventory' table not found.");
                //    tableMissing = true;
                //}
                ////==================================================
                //if (myTables.Contains("inventory_ava") == false)
                //{
                //    Console.WriteLine("'inventory_ava' table not found.");
                //    tableMissing = true;
                //}
                ////==================================================
                ////if (myTables.Contains("ipban") == false)
                ////{
                ////    Console.WriteLine("'ipban' table not found.");
                ////    tableMissing = true;
                ////}
                ////==================================================
                //if (myTables.Contains("news") == false)
                //{
                //    Console.WriteLine("'news' table not found.");
                //    tableMissing = true;
                //}
                ////==================================================
                //return bool.Equals(tableMissing, false);

                #endregion

                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Execute(string cmd)
        {
            lock (myLock)
            {
                //==================================================
                try
                {
                    MySqlCommand com = new MySqlCommand(cmd, myConnection);

                    //IAsyncResult result = com.BeginExecuteNonQuery();
                    //while (result.IsCompleted == false)
                    //{
                    //    System.Threading.Thread.Sleep(5);
                    //}
                    //com.EndExecuteNonQuery(result);

                    com.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MySQL Error:\n" + ex.Message + "Stack:\n" + ex.StackTrace);
                    return false;
                }
                //==================================================
            }
        }

        public DataRow[] Select(string cmd)
        {
            lock (myLock)
            {
                try
                {
                    var adap = new MySqlDataAdapter(cmd, myConnection);
                    var dst = new DataSet();
                    adap.Fill(dst, Convert.ToString(0));
                    var myTable = dst.Tables[Convert.ToString("0")];

                    DataRow[] myRows = new DataRow[myTable.Rows.Count];
                    myTable.Rows.CopyTo(myRows, 0);

                    return myRows;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MySQL Error:\n" + ex.Message + "Stack:\n" + ex.StackTrace);
                    return new DataRow[] { };
                }
            }
        }

        public DataRow[] Select(string format, params object[] args)
        {
            return Select(String.Format(format, args));
        }

        #region Insert/Update/Delete

        public bool Insert(string cmd)
        {
            return Execute(cmd);
        }
        public bool Insert(string cmd, params object[] args)
        {
            return Execute(String.Format(cmd, args));
        }

        public bool Update(string cmd)
        {
            return Execute(cmd);
        }
        public bool Update(string cmd, params object[] args)
        {
            return Execute(String.Format(cmd, args));
        }

        public bool Delete(string cmd)
        {
            return Execute(cmd);
        }
        public bool Delete(string cmd, params object[] args)
        {
            return Execute(String.Format(cmd, args));
        }

        public uint Count(string table)
        {
            var data = Select("SELECT COUNT(*) FROM `{0}`", table);
            if (data.Length == 1)
            {
                return Convert.ToUInt32(data[0]["COUNT(*)"]);
            }
            return 0;
        }
        public uint Count(string table, string contitions)
        {
            var data = Select("SELECT COUNT(*) FROM `{0}` WHERE {1}", table, contitions);
            if (data.Length == 0)
            {
                return Convert.ToUInt32(data[0]["COUNT(*)"]);
            }
            return 0;
        }

        #endregion

        public void Dispose()
        {
            lock (myLock)
            {
                myConnection.Close();
                myConnection = null;
                myLock = null;
            }
        } 
    }
}
