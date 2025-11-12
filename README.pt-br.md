🚀 Library Api

Projeto de portfólio sobre uma Api RESTful para o gerenciamento de uma livraria.
Este projeto possui objetivo de aplicar e aprimorar conhecimentos na área de programação Back-end, portanto
não é considerado um projeto válido para produção e sim para aprendizado.
Foi utilizado a língua inglesa para a elaboração do projeto a fim de manter um padrão dos projetos atuais

🧱 Estrutura do Projeto
* Back-end: Responsável pelo gerenciamento de dados e lógica dos modelos de domínio da Api
* Front-end: Responsável pela interface acessada pelo usuário e integração com a Api

🛠️ Tecnologias Utilizadas
- Backend: C#, ASP .NET CORE
- Banco de Dados: MySQL
- DevOps/Outros: Git
- Documentação: Swagger
- Autenticação: JWT

⚙️ Instalação e Configuração

Para que o projeto funcione em sua máquina, é necessário alguns requisitos básicos, como:
* Clonagem do repositório do github em sua máquina
* String de conexão com o banco de dados MySql
* Dependências do projeto em versões não conflitantes
* Aplicação das migrações (migrations) realizadas no projeto para a criação de tabelas do modelo de domínio
* Configurar a SecretKey no arquivo appsettings.json

🔒 Autenticação Jwt

Neste projeto, foi implementado a autenticação Jwt para que os endpoints sejam divididos. Assim endpoints importantes
serão acessados somente para quem tem permissão

Observação: A configuração da SecretKey deve ser realizada no arquivo appsetting.json e a mesma deve seguir um padrão
que satisfaça o algoritmo HMAC-SHA256 para que a mesma seja assinada

Exemplo de SecretKey

## ⚙️ Configuração do `appsettings.json`

Este é um exemplo da estrutura de configuração necessária para a Api, incluindo a string de conexão com o banco de dados e a chave de autenticação Jwt.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "String de conexão MySql"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "ValidAudience": "https://localhost:XXXX",
    "ValidIssuer": "https://localhost:XXXX",
    "SecretKey": "Minha@Chave@Secreta@do@JwtAspNetCore&2025",
    "TokenValidityInMinutes": 10,
    "RefreshTokenValidityInMinutes": 10
  },
  "AllowedHosts": "*"
}
```



 

  

