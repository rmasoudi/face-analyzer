using OpenFace.controller;
using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows.Forms;
namespace OpenFace
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var config = new HttpSelfHostConfiguration("http://0.0.0.0:6677");

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MessageHandlers.Add(new CORSFilter());
            Labrator.InitModel();
            config.MaxReceivedMessageSize = 3000000;
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                while (true)
                {
                    Thread.Sleep(2);
                }
            }
        }


    }
}
