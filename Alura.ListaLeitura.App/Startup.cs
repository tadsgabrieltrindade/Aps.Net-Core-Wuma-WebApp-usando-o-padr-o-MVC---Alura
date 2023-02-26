using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        //A classe configure faz toda a configuração do do request pipeline 
        //A classe configureServices é reponsável por configurar os serviços da aplicação



        //Resquest Pipeline - FLuxo de requisão e resposta - O parâmetro da classe que construirá todo o pipeline para a aplicação
        public void Configure(IApplicationBuilder app)
        {

            //utilizando o serviço de roteamento do asp.net core - cada rota precisa ser capsulada em uma coleção de objetos
            var builder = new RouteBuilder(app); //recebe a aplicação que estamos configurando 

            //indico qual serão as rotas
            builder.MapRoute("livros/paraler", LivrosParaLer);
            builder.MapRoute("livros/lendo", LivrosLendo);
            builder.MapRoute("livros/lidos", LivrosLidos);
            builder.MapRoute("", WelcomeMessage);
            builder.MapRoute("cadastro/novolivro/{nome}/{autor}", NovoLivroParaLer);
            builder.MapRoute("livros/detalhes/{id:int}", ExibirDetalhes); // o id:int indica que só aceite o tipo inteiro
            var rotas = builder.Build(); //método que é reposável por contruir as rotas 

            //Utilizando o modelo de roteamento do Aps.net Core 
            app.UseRouter(rotas);



            //indica que toda requisição que chegar é para executar essa função dentro do Run - 
            //app.Run(Roteamento); 
        }

        public Task ExibirDetalhes(HttpContext context)
        {
            var id = Convert.ToInt32(context.GetRouteValue("id"));
            LivroRepositorioCSV lista = new LivroRepositorioCSV();

            Livro livro = lista.Todos.First(x => x.Id == id);
            return context.Response.WriteAsync(livro.Detalhes());
        }

        public Task NovoLivroParaLer(HttpContext context)
        {
            Livro livro = new Livro();
            livro.Titulo = context.GetRouteValue("nome").ToString();
            livro.Autor = context.GetRouteValue("autor").ToString();

            LivroRepositorioCSV repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync($"Livro {livro.Titulo} do autor {livro.Autor} foi cadastrado com sucesso!");
        }

        public void ConfigureServices(IServiceCollection service)
        {
            //adiciona o serviço de roteamento na aplicação 
            service.AddRouting(); 
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