using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Alura.ListaLeitura.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var _repo = new LivroRepositorioCSV();

            //Responsável por construir um hospedeiro web
            IWebHost host = new WebHostBuilder()
                .UseKestrel() //indica para o builder qual é o servidor que vai utiizar o modelo htttp
                .UseStartup<Startup>() //indica qual classe vai ser inicializada com o código de configuração
                .Build();
            host.Run();



            //ImprimeLista(_repo.ParaLer);
            //ImprimeLista(_repo.Lendo);
            //ImprimeLista(_repo.Lidos);
        }

        static void ImprimeLista(ListaDeLeitura lista)
        {
            Console.WriteLine(lista);
        }
    }
}
