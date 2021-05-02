## Desafio Técnico - Backend

Olá! Seja bem vindo ;)

## Índice
1. [Projeto e Conteúdo](#Projeto-e-Conteudo)
2. [Como executar essa aplicação](#Como-executar-essa-aplicacao)
3. [Adicionando e configurando o JWT](#Adicionando-e-configurando-o-JWT)
4. [Entity Framework e SQLITE](#Entity-Framework-e-SQLITE)


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

>  Exemplo de implementação
```C#
using System;
using Microsoft.Data.Sqlite;

namespace sqlite_app {
    class Program {
        static void Main (string[] args) {
            var connectionStringBuilder = new SqliteConnectionStringBuilder ();

            //Use DB in project directory.  If it does not exist, create it:
            connectionStringBuilder.DataSource = "./SqliteDB.db";

            using (var connection = new SqliteConnection (connectionStringBuilder.ConnectionString)) {
                connection.Open ();

                //Create a table (drop if already exists first):
                var delTableCmd = connection.CreateCommand ();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS Cards";
                delTableCmd.ExecuteNonQuery ();


                var createTableCmd = connection.CreateCommand ();
                createTableCmd.CommandText = @"CREATE TABLE Cards (
                                                id longtext PRIMARY KEY,
                                                titulo longtext NULL,
                                                conteudo longtext NULL,
                                                lista longtext NULL
                                            );";
                createTableCmd.ExecuteNonQuery ();

                //Seed some data:
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

                using (var reader = selectCmd.ExecuteReader ()) {
                    while (reader.Read ()) {
                        var message = reader.GetString (0)+reader.GetString (1)+reader.GetString (2)+reader.GetString (3);
                        Console.WriteLine (message);
                    }
                }
            }
        }
    }
}
```