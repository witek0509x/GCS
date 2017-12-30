using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS
{
    class Command
    {
        string IP;
        int port;
        string command = "";
        public Command(string ip, int Port)
        {
            IP = ip;
            port = Port;
        }
        public void Create(string name, List<string> columns)
        {
            command = "create " + name;
            foreach (string atribut in columns) command += " " + atribut;
            CommandExecute();
        }
        public void SendUpdate(string name, List<string> values, string picture)
        {
            command = "SendUpdate " + name + " ";
            foreach (string value in values) command += value + " ";
            command += picture;
            CommandExecute();
        }
        public string ReciveUpdate(string name, int MissionTime)
        {
            string command = "ReciveUpdate " + name + " " + MissionTime.ToString();
            return CommandExecute();
        }
        public List<string> GetColumns(string name)
        {
            command = "GetColumns " + name;
            string response = CommandExecute();
            List<string> Atributs = new List<string>();
            string[] splited = response.Split(' ');
            foreach (string element in splited) Atributs.Add(element);
            return Atributs;
        }
        string CommandExecute()
        {
            string respond;
            if (command == "") return "NoCommandError";
            else respond = TCP.Connect(IP, command, 3456);
            clear();
            return respond;
        }
        void clear()
        {
            command = "";
        }
    }
}
