# Petshop Fily

Este projeto é um projeto de conclusão da Pós Graduação em Engenharia de Software da Puc Minas.

O projeto foi desenvolvido utilizando a tecnologia .NET Core, com padrões de desenvolvimento DDD(Drive Domain Design), banco de dados SQL 
e uma interface utilizando o tema Fily.

## Como utilizar:
- Será necessário a utilização da versão 2017 ou superior do Visual Studio com os SDK's do .NET Core instalados.
- Para utilização da base de dados deste projeto, você deverá ter o Microsoft SQL Server versão 2017 ou superior.

Você também poderá utilizar este projeto no Visual Studio Code (Windows, Linux e MacOS).
Para saber mais em como fazer a migração e setup do projeto, visite a documentação oficial da Microsoft [Microsoft .NET Download Guide](https://www.microsoft.com/net/download)

## Tecnologias implementadas:
- ASP.NET Core 2.2
- Entity Framework Core 2.2.6
  - Entity Framework Core Design 2.2.6
  - Entity Framework Core SQLServer 2.2.6
  - Extensions Configuration Json 2.2.6
- Extensions DependencyInjection
- X.PagedList Mvc Core

## Arquitetura implementada:
- Arquitetura com separação de responsabilidade, código limpo e otimizado
- Domain Driven Design (DDD)
- Dependency Injection (IoC)
- Repository e Generic Repository
- Abstract Controllers
- Notificações na aplicação via Middleware

Para ler mais sobre a utilização do padrão DDD em versões recentes do .NET Core, visite [Projeetando um microsserviço orientado a DDD](https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## Interface implementada (UI):
- As views foram construidas com base no ASP.NET Razor e algumas consultas com jQuery Ajax
- Todas dependências das views podem ser encontradas na camada Application(Site)
- A guia de estilo utilizada foi baseada no tema [Fily](https://themefisher.com/products/quixlab-admin-dashboard-theme/)

## Configuração do banco de dados:
Na camada **Data** existe um container de Script(Sql), nela você encontra alguns arquivos
- **Script-SqlServer** com os comandos para criação do banco de dados e persistência inicial

- Para conectar este projeto no seu banco de dados, seja ele remoto ou local, você precisa apenas mudar o arquivo **appsettings.json** 

## Camandas de implementações:
- Data: Camada de acesso ao banco de dados, nela, estão as entidades que representam a database. Nesta camada, encontra-se o contexto do Entity Framework.
- Infra: (IoC) é a camada que realiza as injeções de dependência das interfaces e a services, esta camanda é quem informa como as implementações devem funcionar e ser reconhecidas.
- Domain: (Interfaces) é a camada que implementa as assinaturas das entidades individualmente, assim como, a repository.
- Domain: (Services) é a camada que implementa as interfaces.
- Middleware: Camada que realiza conversões, traduções e pequenas operações.
- Application: Camada que contém a interface e os controladores do negócio.

## Sobre
O petshop Fily foi desenvolvido por Thiago Araújo entre os dias 10/07/2019 e 26/09/2019.
