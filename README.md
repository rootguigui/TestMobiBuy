# TestMobiBuy
Teste para MobiBuy
## Sobre a API

Esta API oferece um sistema completo para gerenciamento de clientes, permitindo a criação, consulta.

## Requisitos para Desenvolvimento
### Docker e Docker Compose

Para executar a aplicação, você precisará:

1. Ter o Docker instalado em sua máquina
2. Ter o Docker Compose instalado (geralmente vem junto com a instalação do Docker Desktop)

O Docker e o Docker Compose facilitam a configuração do ambiente de desenvolvimento, permitindo:
- Criação de contêineres isolados para a aplicação e suas dependências
- Orquestração de múltiplos serviços (aplicação, banco de dados, etc.)
- Eliminação da necessidade de instalar dependências diretamente em sua máquina

### .NET SDK

Se preferir executar sem Docker:

- .NET 8.0 SDK ou superior instalado
- Visual Studio 2022, Visual Studio Code ou outro editor de sua preferência

### Configuração do Banco de Dados

Antes de iniciar a aplicação, você precisa:

1. Criar um banco de dados PostgreSQL, ou
2. Ajustar a string de conexão no arquivo `appsettings.json` para apontar para seu banco existente

### Executando a Aplicação

Para compilar o projeto, execute o seguinte comando:
dotnet build src/TestMobiBuy.Api/TestMobiBuy.Api.csproj

Para rodar o projeto
dotnet run --project src/TestMobiBuy.Api/TestMobiBuy.Api.csproj

### Executando com Docker

Para executar a aplicação utilizando Docker, siga os passos abaixo:

1. Execute o comando para construir e iniciar os contêineres:
   ```
   docker-compose up -d
   ```
2. A API estará disponível em `http://localhost:5006` (ou na porta configurada no Docker Compose)
3. Para parar os contêineres, execute:
   ```
   docker-compose down
   ```

Observação: O Docker Compose irá configurar automaticamente o banco de dados PostgreSQL e a aplicação .NET, criando toda a infraestrutura necessária para o funcionamento do sistema.
