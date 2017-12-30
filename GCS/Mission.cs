using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS
{
    class Mision
    {
        bool admin;
        string ServerIP;
        int MissionTime;
        int AtributsNumber;
        string name;
        string IP;
        Command command;
        DB db = new DB("LocalDB.db");
        List<string> Atributs = new List<string>();
        public Mision()
        {

        }
        public int Init(string Name, string ServerIP, bool Admin = false, List<string> atributs = null)
        {
            name = Name;
            admin = Admin;
            MissionTime = 0;
            IP = ServerIP;
            command = new Command(ServerIP, 3456);
            if (DBExist(name))
            {
                return 1;
            }
            else
            {
                if (admin)
                {
                    Atributs = atributs;
                    AtributsNumber = atributs.Count;
                    MissionTime = 0;
                    string sql = "CREATE TABLE " + name + " (time int";
                    foreach (string element in Atributs) sql += "," + element + " real";
                    sql += ")";
                    db.Query(sql);
                    command.Create(name, Atributs);
                }
                else
                {

                    db.Query("Create Table " + name);
                    Atributs = command.GetColumns(name);
                    string sql = "CREATE TABLE " + name + " (time int";
                    foreach (string element in Atributs) sql += "," + element + " real";
                    sql += ")";
                    db.Query(sql);
                    save(command.ReciveUpdate(name, 0));
                }
                return 0;
            }
        }
        void save(string value)
        {
            string[] splited = value.Split(' ');
            for(int i = 0; i < Int32.Parse(splited[0]); i++)
            {
                string sql = "insert into " + name + " values (";
                for (int j = 1 + i * (Atributs.Count + 1); j <= (i+1) * (Atributs.Count + 1); j++)
                {
                    if (j != (i + 1) * (Atributs.Count + 1)) sql += splited[j] + ",";
                    else
                    {
                        if (i == Int32.Parse(splited[0]) - 1) MissionTime = Int32.Parse(splited[1 + i * (Atributs.Count + 1)]);
                        sql += splited[j];
                    }
                }
                sql += ")";
                db.Query(sql);
            }
        }
        public void ReciveUpdate()
        {
            string command = "ReciveUpdate " + name + " " + MissionTime.ToString();
            string response = TCP.Connect("127.0.0.1", command, 3456);
            save(response);
        }
        bool DBExist(string Name)
        {
            return !(db.Query("select * from dbstructure where Name = '" + Name + "'") == "");
        }
        public bool SendUpdate()
        {
            int i = 0;
            List<string> tmp = new List<string>();
            if (TXT.ReadOneLine(0) == "FileIsEmpty") return false;
            if (TXT.ReadOneLine(0) == "SomeExeption") return false;
            else
            {
                while(TXT.ReadOneLine(i) != null)
                {
                    tmp.Add(TXT.ReadOneLine(i));
                    i++;
                }
                command.SendUpdate(name, tmp, "123");
                TXT.clear();
                return true;
            }
        }

    }
}
