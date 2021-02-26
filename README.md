# Desafio Técnico - Backend

O propósito desse desafio é a criação de uma API que fará a persistência de dados de um quadro de kanban. Esse quadro possui listas, que contém cards.

## Rodando o Frontend

Um frontend de exemplo foi disponibilizado na pasta FRONT.

Para rodá-lo, faça:

```console
> cd FRONT
> yarn
> yarn start
```

## Desafio

Você precisa criar uma API REST de acordo com os requisitos abaixo, que deve ser desenvolvido na pasta "BACK".

Para criar sua API você pode escolher entre duas tecnologias:

1. Javascript ou Typescript + NodeJS + Express
2. C# + ASP.NET Core + WebApi

## Requisitos

1. O sistema deve ter um mecanismo de login usando JWT, com um entrypoint que recebe `{ "login":"letscode", "senha":"lets@123"}` e gera um token.

2. O sistema deve ter um middleware que valide se o token é correto, valido e não está expirado, antes de permitir acesso a qualquer outro entrypoint. Em caso negativo retorne status 401.

3. O login e senha fornecidos devem estar em variáveis de ambiente e terem uma versão para o ambiente de desenvolvimento vinda de um arquivo .env no node ou de um arquivo de configuração no ASP.NET. Esse arquivo não deve subir ao GIT, mas sim um arquivo de exemplo sem os valores reais. O mesmo vale para qualquer "segredo" do sistema, como a chave do JWT.

4. Um card terá o seguinte formato: 

```
id: int | (guid [c#] | uuid [node])
titulo : string, 
conteudo: string, 
lista: string
```

5. Os entrypoints da aplicação devem usar a porta 5000 e ser:

```
(POST)      http://0.0.0.0:5000/login/

(GET)       http://0.0.0.0:5000/cards/
(POST)      http://0.0.0.0:5000/cards/
(PUT)       http://0.0.0.0:5000/cards/{id}
(DELETE)    http://0.0.0.0:5000/cards/{id}
```

6. Para inserir um card o título, o conteúdo e o nome da lista devem estar preenchidos, o id não deve conter valor. Ao inserir retorne o card completo incluindo o id atribuído com o statusCode apropriado. Caso inválido, retorne status 400.

7. Para alterar um card, o entrypoint deve receber um id pela URL e um card pelo corpo da requisição. Valem as mesmas regras de validação do item acima exceto que o id do card deve ser o mesmo id passado pela URL. Na alteração todos os campos são alterados. Caso inválido, retorne status 400. Caso o id não exista retorne 404. Se tudo correu bem, retorne o card alterado.

8. Para remover um card, o entrypoint deve receber um id pela URL. Caso o id não exista retorne 404. Se a remoção for bem sucedida retorne a lista de cards.

9. A listagem de cards deve enviar todos os cards em formato json, contendo as informações completas. 

10. Deve ser usada alguma forma de persistência, no C# pode-se usar o Entity Framework (in-memory), no nodeJS pode ser usado Sequelize + sqlite (in-memory) ou diretamente o driver do sqlite (in-memory).

11. Se preferir optar por utilizar um banco de dados "real", adicione um docker-compose em seu repositório que coloque a aplicação e o banco em execução, quando executado `docker-compose up` na raiz. A connection string e a senha do banco devem ser setados por ENV nesse arquivo.

12. O campo conteúdo do card aceitará markdown, isso não deve impactar no backend, mas não custa avisar...

13. Faça um filter (asp.net) ou middleware (nodejs) que escreva no console sempre que os entrypoints de alteração ou remoção forem usados, indicando o horário formatado como o datetime a seguir: `01/01/2021 13:45:00`. 

A linha de log deve ter o seguinte formato (se a requisição for válida):

`<datetime> - Card <id> - <titulo> - <Remover|Alterar>`

Exemplo:

```console
> 01/01/2021 13:45:00 - Card 1 - Comprar Pão - Removido
```

14. O projeto deve ser colocado em um repositório GITHUB ou equivalente, estar público, e conter um readme.md que explique em detalhes qualquer comando ou configuração necessária para fazer o projeto rodar. Por exemplo, como configurar as variáveis de ambiente, como rodar migrations (se foram usadas). 

15. A entrega será apenas a URL para clonarmos o repositório.

## Diferenciais e critérios de avaliação

Arquiteturas que separem responsabilidades, de baixo acoplamento e alta-coesão são preferíveis, sobretudo usando dependências injetadas, que permitam maior facilidade para testes unitários e de integração.

Avaliaremos se o código é limpo (com boa nomenclatura de classes, variáveis, métodos e funções) e dividido em arquivos bem nomeados, de forma coesa e de acordo com boas práticas. Bem como práticas básicas como tratamento de erros.

Desacoplar e testar as regras de negócios / validações / repositório com testes unitários será considerado um diferencial.

O uso de typescript no node acompanhado das devidas configurações e tipagens bem feitas, bem como uso de técnicas de abstração usando interfaces (especialmente do repositório) serão consideradas um deferencial.

O uso de Linter será considerado um diferencial.

A criação de um docker-compose e de dockerfiles que ao rodar `docker-compose up` subam o sistema por completo (front, back e banco [se houver]) será considerado um diferencial.

Teve dificuldade com algo, ou fez algo meio esquisito para simplificar algo que não estava conseguindo fazer? Deixe uma observação com a justificativa no readme.md para nós...

Entregou incompleto, teve dificuldade com algo, ou fez algo meio esquisito para simplificar alguma coisa que não estava conseguindo fazer? Deixe uma observação com a justificativa no readme.md para nós...