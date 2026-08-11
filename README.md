🚀 Library Api

Projeto de portfólio sobre uma Api RESTful para o gerenciamento de uma livraria.
Este projeto possui objetivo de aplicar e aprimorar conhecimentos na área de programação Back-end, portanto
não é considerado um projeto válido para produção e sim para aprendizado.
Foi utilizado a língua inglesa para a elaboração do projeto a fim de manter um padrão dos projetos atuais

🧱 Estrutura do Projeto
* Back-end: Responsável pelo gerenciamento de dados e lógica dos modelos de domínio da Api
* Front-end: Responsável pela interface acessada pelo usuário e integração com a Api

🛠️ Tecnologias Utilizadas
- Backend: C# (Net 8.0), ASP .NET CORE
- Banco de Dados: MySQL
- DevOps/Outros: Git
- Documentação: Swagger
- Autenticação: JWT

⚙️ Instalação e Configuração

Para que o projeto funcione em sua máquina, é necessário alguns requisitos básicos, como:
* Clonagem do repositório do github em sua máquina
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* MySQL instalado e em execução na sua máquina
* Dependências do projeto em versões não conflitantes

### Passo a passo pós-clone

1. Restaure as dependências do projeto:

   ```powershell
   dotnet restore
   ```

2. Configure a string de conexão com o seu banco MySql usando **User Secrets**
   (os dados ficam salvos apenas na sua máquina e nunca vão para o git):

   ```powershell
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;port=3306;DataBase=LivrariaDb;Uid=SEU_USUARIO;password=SUA_SENHA"
   ```

   > Se preferir, você pode editar diretamente a `ConnectionStrings:DefaultConnection`
   > dentro do arquivo `appsettings.json`, que já vem com um valor placeholder.

3. Aplique as migrações para criar as tabelas do banco:

   ```powershell
   dotnet ef database update
   ```

4. Execute a aplicação:

   ```powershell
   dotnet run
   ```

5. Acesse a documentação Swagger em `https://localhost:<porta>/swagger`.

🔒 Autenticação Jwt

Neste projeto, foi implementado a autenticação Jwt para que os endpoints sejam divididos. Assim endpoints importantes
serão acessados somente para quem tem permissão

Observação: A configuração da SecretKey deve ser realizada no arquivo `appsettings.json` e a mesma deve seguir um padrão
que satisfaça o algoritmo HMAC-SHA256 para que a mesma seja assinada. O arquivo já vem com uma chave de teste.

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
  "FileLogging": {
    "LogLevel": "Information",
    "FilePath": "Log.txt"
  },
  "Jwt": {
    "ValidAudience": "LibraryApi",
    "ValidIssuer": "LibraryApi",
    "SecretKey": "Minha@Chave@Secreta@do@JwtAspNetCore&2025",
    "TokenValidityInMinutes": 60,
    "RefreshTokenValidityInMinutes": 120
  },
  "AllowedHosts": "*"
}
```

> Dica: o caminho do arquivo de log é configurável na seção `FileLogging`.
> Use um caminho absoluto (ex.: `D:\Logs\Log.txt`) ou relativo ao projeto (ex.: `Logs\Log.txt`).


🌍 Dependências do projeto

<img width="378" height="214" alt="image" src="https://github.com/user-attachments/assets/61bedceb-308d-4927-9783-027cbd9b41cc" />






 

  
