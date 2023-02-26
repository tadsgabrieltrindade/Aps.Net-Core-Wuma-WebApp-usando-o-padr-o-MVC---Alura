using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        //Resquest Pipeline - FLuxo de requisão e resposta - O parâmetro da classe que construirá todo o pipeline para a aplicação
        public void Configure(IApplicationBuilder app)
        {
                
            //indica que toda requisição que chegar é para executar essa função dentro do Run 
            app.Run(Roteamento);
        }

        public Task Roteamento(HttpContext context)
        {
            IDictionary<String, RequestDelegate> rotas = new Dictionary<String, RequestDelegate>();
            var path = context.Request.Path;

            rotas.Add("/livros/paraler", LivrosParaLer);
            rotas.Add("/livros/lendo", LivrosLendo);
            rotas.Add("/livros/lidos", LivrosLidos);
            rotas.Add("/", WelcomeMessage);

            if (rotas.ContainsKey(path))
            {
                var metodo = rotas[path];
                return metodo.Invoke(context);
            }

            //return context.Response.WriteAsync(rotas.);
            return Error(context);

        }

        public Task Error(HttpContext context)
        {
            context.Response.StatusCode = 404;
            return context.Response.WriteAsync("Caminho Inexistente");
        }

        //Recebe como parâmetro um obj de requisição contento as infos necessárias 
        public Task LivrosParaLer(HttpContext context)
        {
            LivroRepositorioCSV _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }

        public Task LivrosLendo(HttpContext context)
        {
            LivroRepositorioCSV _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {
            LivroRepositorioCSV _repo = new LivroRepositorioCSV();
            return  context.Response.WriteAsync(_repo.Lidos.ToString());
        }

        public Task WelcomeMessage(HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.WriteAsync("<html><head><title>Bem-vindo</title></head><body>");
            context.Response.WriteAsync("<h2 style=\"font-family: Verdana;\">Bem vindo ao nosso site</h2>");
            return context.Response.WriteAsync("</body></html>");
        }


    }
}