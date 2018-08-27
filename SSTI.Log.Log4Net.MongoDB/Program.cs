using System;

namespace SSTI.Log.Log4Net.MongoDB
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log.Info (new Models.Log { Mensagem = "Log de informação" });
            log.Error(new Models.Log { Mensagem = "Log de erro" });
            log.Warn (new Models.Log { Mensagem = "Log de alerta" });

            Console.Read();
        }
    }
}