## Desafio Técnico - Backend

Olá! Seja bem vindo ;)

## Índice
1. [Projeto e Conteúdo](#Projeto-e-Conteudo)
2. [Como executar essa aplicação](#Como-executar-essa-aplicacao)
3. [Adicionando e configurando o JWT](#Adicionando-e-configurando-o-JWT)
4. [Entity Framework e SQLITE](#Entity-Framework-e-SQLITE)
5. [Log com utilização de Filter](#Log-com-utilizacao-de-Filter)
6. [Testes unitários (xUnit)](#Testes-unitArios-Xunit)
7. [Publicação](#Publicação)


## Projeto e Conteúdo
Este repositório contém uma implementação para backend de um sistema de gerenciamento, CRUD, autenticação e persistencia em banco de dados SQLITE.

## Como executar essa aplicação
Para executar essa aplicação, primeiro é necessário instalar o .NET Core. Depois disso, você deve seguir os passos abaixo:
1. Clone ou faça o download deste repositório.
2. Extraia o conteúdo se o download for um arquivo zip. Verifique se os arquivos estão com read-write.
3. Execute o comando abaixo no prompt de comando.
```shell
dotnet run
```
4. A aplicação deverá estar disponivel em seu navegador no endereço: https://0.0.0.0:5000/swagger
![swagger](/BACK/Kanban/assets/swagger.png)

## JWT
O JWT (JSON Web Token) nada mais é que um padrão (RFC-7519) de mercado que define como transmitir e armazenar objetos JSON de forma simples, compacta e segura entre diferentes aplicações, muito utilizado para validar serviços em Web Services pois os dados contidos no token gerado pode ser validado a qualquer momento uma vez que ele é assinado digitalmente.

JSON Web Tokens (JWT) é um padrão stateless porque o servidor autorizador não precisa manter nenhum estado; o próprio token é sulficiente para verificar a autorização de um portador de token.

Os JWTs são assinados usando um algoritmo de assinatura digital (por exemplo, RSA) que não pode ser forjado. Por isso, qualquer pessoa que confie no certificado do assinante pode confiar com segurança que o JWT é autêntico. Não há necessidade de um servidor consultar o servidor emissor de token para confirmar sua autenticidade.

fonte: https://jwt.io/introduction/

> Para importar esse componente para seu projeto, basta executar no prompt o comando abaixo ou utilizar o Nuget 

```shell
dotnet add package System.IdentityModel.Tokens.Jwt
```

### Adicionando e configurando o JWT

> Definir chave secreta no arquivo `appsettings.json`:

```Json
 },
  "SecurityKey": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
```
> Implementar uma classe Controller para gestão das credenciais, nessa implementação é a  `TokenController.cs`.
- Injetar intância de Configuration
- Criar método Action POST RequestToken()
 - Validar credenciais do usuário 
 - Definir claims
 - Definir chave 
 - Definir credenciais 
 - Gerar token (data expiração)

 
> Adicione o gerador manipulador de autenticação `services.AddAuthentication` à coleção de serviços no método `Startup.ConfigureServices`. No método `Startup.Configure`, ative o middleware para requisitar a autenticação: 

```C#
        public void ConfigureServices (IServiceCollection services) {
             services.AddScoped<ClienteRepository>();
            //especifica o esquema usado para autenticacao do tipo Bearer
            // e 
            //define configurações como chave,algoritmo,validade, data expiracao...
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "aplicacao",
                    ValidAudience = "canal",
                    IssuerSigningKey = new SymmetricSecurityKey (
                    Encoding.UTF8.GetBytes (Configuration["SecurityKey"]))
                    };

                    options.Events = new JwtBearerEvents {
                        OnAuthenticationFailed = context => {
                                Console.WriteLine ("Token inválido..:. " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context => {
                                Console.WriteLine ("Toekn válido...: " + context.SecurityToken);
                                return Task.CompletedTask;
                            }
                    };
                });

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseAuthentication ();
            app.UseMvc ();
        }
```

## Entity Framework e SQLITE

O Entity Framework é uma ferramenta ORM da Microsoft madura e testada pelo mercado que pode ser usada para aplicações que usam o .NET Framework.

O SQLite é uma biblioteca de código aberto (open source) desenvolvido na linguagem C que permite a disponibilização de um pequeno banco de dados na própria aplicação, sem a necessidade de acesso a um SGDB separado. A estrutura de banco junto com a aplicação é denominada de “banco de dados embutido” e é indicada para aplicações de pequeno porte, que utilizam poucos dados.   grande vantagem dos bancos de dados embutidos está em sua simplicidade: é mais prático implementar e administrar do que a implementação de SGDB´s separados, utilizando soluções como SQL Server e Oracle. Por outro lado, a performance e limitação de recursos são desvantagens do SQLite e soluções semelhantes. Para escolher a opção mais adequada, devem ser levados em consideração parâmetros como os exemplificados a seguir.(fonte Portal GSTI)

Nessa implementação, foi utilizada essas duas ferramentas para acelerar o desenvolvimento e prototipação da aplicação.

>  Exemplo de implementação
```C#
using System;
using Microsoft.Data.Sqlite;

namespace sqlite_app {
    class Program {
        static void Main (string[] args) {
            var connectionStringBuilder = new SqliteConnectionStringBuilder ();

            //Use o banco de dados no diretório do projeto. Se não existir, crie-o:
            connectionStringBuilder.DataSource = "./SqliteDB.db";

            using (var connection = new SqliteConnection (connectionStringBuilder.ConnectionString)) {
                connection.Open ();

                //Create a table (drop se já existir):
                var delTableCmd = connection.CreateCommand ();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS Cards";
                delTableCmd.ExecuteNonQuery ();

                //criando uma tabela
                var createTableCmd = connection.CreateCommand ();
                createTableCmd.CommandText = @"CREATE TABLE Cards (
                                                id longtext PRIMARY KEY,
                                                titulo longtext NULL,
                                                conteudo longtext NULL,
                                                lista longtext NULL);";
                createTableCmd.ExecuteNonQuery ();

                //Inserindo algum dado:
                using (var transaction = connection.BeginTransaction ()) {
                    var insertCmd = connection.CreateCommand ();

                    insertCmd.CommandText = @"INSERT INTO Cards VALUES('EB913853-1BF2-4364-42FB-08D90CBBE8D8'
                                            ,'titulo exemplo','conteudo descrito','lista detalhada')";
                    insertCmd.ExecuteNonQuery ();

                    transaction.Commit ();
                }

                //Read the newly inserted data:
                var selectCmd = connection.CreateCommand ();
                selectCmd.CommandText = "SELECT * FROM Cards";

                selectCmd.CommandText = "SELECT  * FROM Cards";

                using (var reader = selectCmd.ExecuteReader ()) {
                    while (reader.Read ()) {
                        for (int i = 0; i < reader.FieldCount; i++) {
                            Console.WriteLine (reader.GetString (i));
                        }
                    }
                }
            }
        }
    }
}
```

## Log com utilização de Filter

`Filter` permitem que você execute código em determinados estágios do pipeline de processamento da solicitação. Um filtro de ação é um filtro executado antes ou depois da execução de um método de ação. Usando filtros de ação, você pode tornar seus métodos de ação enxutos, limpos e fáceis de manter.

>  Exemplo de implementação

```C#
public class CustomActionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;
    public CustomActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("CustomActionFilter");
        }
     public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogWarning("Log de alguma ação durante a execução...");
            base.OnActionExecuting(context);
        }
    public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogWarning("Log de alguma ação após a execução...");
            base.OnActionExecuted(context);
        }
    public override void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogWarning("Log de algum resultado durante a execução...");
            base.OnResultExecuting(context);
        }
    public override void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogWarning("Log de algum resultado após a execução...");
            base.OnResultExecuted(context);
        }
    }
```
Registe o serviço na classe `Startup.cs`

```C#
public void ConfigureServices(IServiceCollection services)
{
  services.AddScoped<CustomActionFilter>();
}
```

Adcione a Tag `ServiceFilter` no acima do metodo para ser monitoriado, exemplo:
```C#
        [ServiceFilter(typeof(CustomActionFilter))]
        [HttpDelete]
        public async Task<ActionResult<Card>> Delete (Guid id) {
```
O resultado pode ser visto no prompt. Nessa implementação o filter é ativado sempre que os entrypoints de alteração ou remoção forem usados
![swagger](/BACK/Kanban/assets/filter.png)

## Testes unitários (xUnit)

Teste de unidade é toda a aplicação de teste nas assinaturas de entrada e saída de um sistema. Consiste em validar dados válidos e inválidos via I/O (entrada/saída) sendo aplicado por desenvolvedores ou analistas de teste. Uma unidade é a menor parte testável de um programa de computador. Em programação procedural, uma unidade pode ser uma função individual ou um procedimento. Idealmente, cada teste de unidade é independente dos demais, o que possibilita ao programador testar cada módulo isoladamente.
O xUnit é uma ferramenta de teste de unidade focada na comunidade, gratuita e de código aberto para o .NET Framework. Escrito pelo inventor original do NUnit v2, xUnit.net é a mais recente tecnologia para testes de unidade C #, F #, VB.NET e outras linguagens .NET. xUnit.net funciona com ReSharper, CodeRush, TestDriven.NET e Xamarin. Faz parte da .NET Foundation e opera de acordo com seu código de conduta. Ele está licenciado sob Apache 2 (uma licença aprovada pela OSI).

> Utilize o comando abaixo para instalar o componente, e gerar a pasta de testes:
```shell
dotnet add package Xunit
dotnet new xunit -o DotnetCoreApp.Tests
```

> exemplo de classe de teste
```C#
using System;
using Xunit;

namespace DotnetCoreApp.test
{
    public class UnitTest1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
```
> Para executar utilize o comando abaixo na pasta
```shell
dotnet test /p:CollectCoverage=true
```

## Publicação

Ao publicar a sua aplicação, é necessário específicar o sistema operacional e arquitetura de CPU. Ao publicar seu aplicativo e criar um executável, você pode publicar o aplicativo como independente ou dependente de tempo de execução. 
Você pode criar um executável para `-r <RID> --self-contained false` uma plataforma dotnet publish específica passando os parâmetros para o comando. Quando `-r` o parâmetro é omitido, um executável é criado para sua plataforma atual. Todos os pacotes NuGet que tenham dependências específicas da plataforma para a plataforma-alvo são copiados para a pasta de publicação.

```shell
dotnet publish -c Release -r win-x64 --self-contained true
```