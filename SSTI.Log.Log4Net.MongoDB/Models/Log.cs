using System;

namespace SSTI.Log.Log4Net.MongoDB.Models
{
    public class Log
    {
        public DateTime Data     { get; set; } = DateTime.Now;
        public string   Mensagem { get; set; }

        public override string ToString()
        {
            return $"Data: {Data} - Mensagem: {Mensagem}";
        }
    }
}