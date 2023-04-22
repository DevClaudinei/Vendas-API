<h1 align="center">Vendas-API</h1>

## Descrição do desafio

<p align="center">Analisar e desenvolver uma API de vendas</p>

### Regras do desafio

- Construir uma API REST utilizando .Net Core, Java ou NodeJs (com Typescript);
- A API deve expor uma rota com documentação swagger (http://.../api-docs).
- A API deve possuir 3 operações:
  1) Registrar venda: Recebe os dados do vendedor + itens vendidos. Registra venda com status "Aguardando pagamento";
  2) Buscar venda: Busca pelo Id da venda;
  3) Atualizar venda: Permite que seja atualizado o status da venda.
     * OBS.: Possíveis status: `Pagamento aprovado` | `Enviado para transportadora` | `Entregue` | `Cancelada`.
- Uma venda contém informação sobre o vendedor que a efetivou, data, identificador do pedido e os itens que foram vendidos;
- O vendedor deve possuir id, cpf, nome, e-mail e telefone;
- A inclusão de uma venda deve possuir pelo menos 1 item;
- A atualização de status deve permitir somente as seguintes transições: 
  - De: `Aguardando pagamento` Para: `Pagamento Aprovado`
  - De: `Aguardando pagamento` Para: `Cancelada`
  - De: `Pagamento Aprovado` Para: `Enviado para Transportadora`
  - De: `Pagamento Aprovado` Para: `Cancelada`
  - De: `Enviado para Transportador`. Para: `Entregue`
- A API não precisa ter mecanismos de autenticação/autorização;
- A aplicação não precisa implementar os mecanismos de persistência em um banco de dados, eles podem ser persistidos "em memória".

## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
[Git](https://git-scm.com), [.NET](https://dotnet.microsoft.com/en-us/download) e ter o banco [MySQL](https://www.mysql.com/downloads/) instalado e configurado na máquina.
Além disto é bom ter um editor para trabalhar com o código como [Visual Studio Code ou Visual Studio](https://code.visualstudio.com/download) 

## 🎲 Rodando o Back End (servidor)

```bash
# Clone este repositório
$ git clone https://github.com/DevClaudinei/Vendas-API.git

# Acesse a pasta do projeto no terminal/cmd
$ cd Vendas-API\src\PaymentApi

# Execute a aplicação através do comando:
$ dotnet run

# O servidor inciará na porta: 7145 
- Utilizando a URL <https://localhost:7145/swagger/index.html>
```

## Tela inicial:
![image](https://user-images.githubusercontent.com/103595662/233754899-20fcae09-0b84-4f0e-ac8a-67ab149308e0.png)

### Post
![image](https://user-images.githubusercontent.com/103595662/233754921-e38375cc-1250-4e8c-a92f-46f4e3c1be9d.png)

### Get
![image](https://user-images.githubusercontent.com/103595662/233754955-deade5f1-2258-48fb-a7f5-4f51be9e52e8.png)

### Put
![image](https://user-images.githubusercontent.com/103595662/233754976-c4dd97fb-9544-4d0c-a2eb-8e41672cfbf3.png)

### Schemas
![image](https://user-images.githubusercontent.com/103595662/233755004-19dfdbe9-39cf-4cea-a4e0-fc5dbf5f84f2.png)

## 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [.NET](https://dotnet.microsoft.com/en-us/)
- [MySQL](https://www.mysql.com/downloads/)
- [AutoMapper](https://automapper.org/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [UnitOfWork](https://www.nuget.org/packages/EntityFrameworkCore.Data.UnitOfWork)
- [EntityFramework](https://learn.microsoft.com/pt-br/ef/)
- [XUnit](https://xunit.net/)
- [Moq](https://documentation.help/Moq/)
- [Bogus](https://github.com/bchavez/Bogus)
- [Fluent Assertions](https://fluentassertions.com/)
