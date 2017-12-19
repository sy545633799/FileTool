using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using StarEdit.Tools;

namespace StarEdit.MysqlServices
{
    class MysqlService
    {
        static string database;
        static string linker;
        static string respath;
        static string titleName;
        static MysqlService()
        {
            IniFile ini = new IniFile("./config.ini");
            respath = ini.IniReadValue("Database","respath");
            titleName = ini.IniReadValue("Database", "title");
            database = ini.IniReadValue("Database", "Database");
            string datasource = ini.IniReadValue("Database", "Data Source");
            string userid = ini.IniReadValue("Database", "User Id");
            string password = ini.IniReadValue("Database", "Password");
            linker = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", database, datasource, userid, password);
        }
        public static string getNamever()
        {
            return titleName;
        }
        public static string getResPath()
        {
            return respath;
        }
        public static void ClearData(string tablename)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("TRUNCATE {0}", tablename);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static OldDataPack GetAllData(string tablename)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();

            MySqlCommand command = new MySqlCommand(String.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '{0}' AND TABLE_SCHEMA = '{1}'", tablename, database), conn);
            MySqlDataReader reader = command.ExecuteReader();

            OldDataPack pack = new OldDataPack(tablename);
            while (reader.Read())
            {
                pack.header.Add(reader.GetString(0));
                pack.datatype.Add(reader.GetString(1));
                pack.datasize.Add(reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
                pack.comment.Add(reader.GetString(3));
            }
            reader.Close();

            command = new MySqlCommand(String.Format("SELECT * FROM `{0}` ORDER BY `{1}`", tablename, pack.header[0]), conn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                List<String> item = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    item.Add(reader.GetString(i));
                }
                int id = int.Parse(item[0]);
                pack.maxid = Math.Max(pack.maxid, id);
                pack.data.Add(id, item);
            }
            conn.Close();
            pack.cachTime = DateTime.Now;

            return pack;
        }

        public static OldBiDataPack GetAllBiData(string tablename)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();

            MySqlCommand command = new MySqlCommand(String.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '{0}' AND TABLE_SCHEMA = '{1}'", tablename, database), conn);
            MySqlDataReader reader = command.ExecuteReader();

            OldBiDataPack pack = new OldBiDataPack(tablename);
            while (reader.Read())
            {
                pack.header.Add(reader.GetString(0));
                pack.datatype.Add(reader.GetString(1));
                pack.datasize.Add(reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
                pack.comment.Add(reader.GetString(3));
            }
            reader.Close();

            //#region 专为实现默认掉落组
            //if (tablename == "cfg_monster_group_drop")
            //{
            //    command = new MySqlCommand(String.Format("SELECT `mid`,`mid`,1000,0,1439 FROM `cfg_monster`", tablename), conn);
            //    reader = command.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        List<String> item = new List<string>();
            //        for (int i = 0; i < reader.FieldCount; i++)
            //        {
            //            item.Add(reader.GetString(i));
            //        }
            //        pack.data.Add(new BiData(int.Parse(item[0]), int.Parse(item[1])), item);
            //        int keyid = int.Parse(item[0]);
            //        if (pack.keys.ContainsKey(keyid))
            //        {
            //            pack.keys[keyid]++;
            //        }
            //        else
            //        {
            //            pack.keys.Add(keyid, 1);
            //        }
            //    }
            //    reader.Close();
            //}
            //#endregion

            command = new MySqlCommand(String.Format("SELECT * FROM `{0}` ORDER BY `{1}`, `{2}`", tablename, pack.header[0], pack.header[1]), conn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                List<String> item = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    item.Add(reader.GetString(i));
                }
                pack.data.Add(new BiData(int.Parse(item[0]), int.Parse(item[1])), item);
                int keyid = int.Parse(item[0]);
                if (pack.keys.ContainsKey(keyid))
                {
                    pack.keys[keyid]++;
                }
                else
                {
                    pack.keys.Add(keyid, 1);
                }
            }
            conn.Close();
            pack.cachTime = DateTime.Now;

            return pack;
        }

        public static OldTriDataPack GetAllTriData(string tablename)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();

            MySqlCommand command = new MySqlCommand(String.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '{0}' AND TABLE_SCHEMA = '{1}'", tablename, database), conn);
            MySqlDataReader reader = command.ExecuteReader();

            OldTriDataPack pack = new OldTriDataPack(tablename);
            while (reader.Read())
            {
                pack.header.Add(reader.GetString(0));
                pack.datatype.Add(reader.GetString(1));
                pack.datasize.Add(reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
                pack.comment.Add(reader.GetString(3));
            }
            reader.Close();

            command = new MySqlCommand(String.Format("SELECT * FROM `{0}` ORDER BY `{1}`, `{2}`, `{3}`", tablename, pack.header[0], pack.header[1], pack.header[2]), conn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                List<String> item = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    item.Add(reader.GetString(i));
                }
                pack.data.Add(new TriData(int.Parse(item[0]), int.Parse(item[1]), int.Parse(item[2])), item);
                int keyid = int.Parse(item[0]);
                if (pack.keys.ContainsKey(keyid))
                {
                    pack.keys[keyid]++;
                }
                else
                {
                    pack.keys.Add(keyid, 1);
                }
            }
            conn.Close();
            pack.cachTime = DateTime.Now;

            return pack;
        }

        public static void InsertData(string tablename, List<String> data)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String parms = String.Join("','", data.ToArray());
            String sql = String.Format("INSERT INTO {0} VALUES ('{1}')", tablename, parms);
            string temp = sql.Replace("\"\"","\"");
            string secTemp = temp.Replace("\'\"", "\'");
            temp = secTemp.Replace("\"\'", "\'");
            MySqlCommand command = new MySqlCommand(temp, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void RemoveData(string tablename, string keyname, int id)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("DELETE FROM {0} WHERE `{1}` = '{2}'", tablename, keyname, id);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void RemoveData(string tablename, string keyname, int id, string keyname2, int id2)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("DELETE FROM {0} WHERE `{1}` = '{2}' AND `{3}` = '{4}'", tablename, keyname, id, keyname2, id2);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void RemoveData(string tablename, string keyname, int id, string keyname2, int id2, string keyname3, int id3)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("DELETE FROM {0} WHERE `{1}` = '{2}' AND `{3}` = '{4}' AND `{5}` = '{6}'", tablename, keyname, id, keyname2, id2, keyname3, id3);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateData(string tablename, string keyname, int id, string fieldname, string fieldvalue)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("UPDATE {0} SET `{1}` = '{2}' WHERE `{3}` = '{4}'", tablename, fieldname, fieldvalue, keyname, id);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateData(string tablename, string keyname, int id, string keyname2, int id2, string fieldname, string fieldvalue)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("UPDATE {0} SET `{1}` = '{2}' WHERE `{3}` = '{4}' AND `{5}` = '{6}'", tablename, fieldname, fieldvalue, keyname, id, keyname2, id2);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateData(string tablename, string keyname, int id, string keyname2, int id2, string keyname3, int id3, string fieldname, string fieldvalue)
        {
            MySqlConnection conn = new MySqlConnection(linker);
            conn.Open();
            String sql = String.Format("UPDATE {0} SET `{1}` = '{2}' WHERE `{3}` = '{4}' AND `{5}` = '{6}' AND `{7}` = '{8}'", tablename, fieldname, fieldvalue, keyname, id, keyname2, id2, keyname3, id3);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }
    }
}
