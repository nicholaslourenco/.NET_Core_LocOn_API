# 🎬 LocOn - API para streaming de filmes
## .NET_Core_LocOn_API

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-blue.svg)](https://www.mysql.com/)
[![Stripe](https://img.shields.io/badge/Stripe-Integration-indigo.svg)](https://stripe.com/)

Uma API REST robusta desenvolvida em C# e .NET 8 que simula o ecossistema de uma plataforma de streaming de filmes. O projeto conta com gerenciamento completo de usuários, catálogos de filmes, planos de assinatura dinâmicos e um fluxo real de pagamento integrado ao **Stripe**.

---

## 🚀 Funcionalidades Principais

### 🔒 Autenticação & Autorização (RBAC)
* Controle de acesso baseado em papéis (**Role-Based Access Control**).
* **Admin:** Permissão total para gerenciar o catálogo de filmes, criar/editar planos de assinatura e visualizar usuários.
* **Padrão:** Acesso à listagem de filmes (condicionada à assinatura ativa) e gerenciamento do próprio perfil.

### 💳 Sistema de Assinaturas & Pagamentos (Stripe)
* **Integração Nativa com o Stripe:** Processamento de pagamentos para a contratação de planos.
* **Ativação Automática:** Assim que o pagamento é confirmado via Stripe, o plano correspondente é vinculado e ativado na conta do usuário, liberando o consumo dos filmes.

### 📁 CRUDs Completos
* **Usuários:** Cadastro, atualização de perfil e histórico de cobrança.
* **Filmes:** Cadastro de títulos, gêneros, sinopses e restrições de idade.
* **Planos:** Criação de pacotes com preços e benefícios customizáveis.

---

## 🛠️ Tecnologias e Arquitetura

O projeto foi construído seguindo boas práticas de desenvolvimento de software e design de código:

* **Runtime:** .NET 8 (C# 12)
* **Banco de Dados:** MySQL 8.0
* **ORM:** Entity Framework Core (Code-First)
* **Gateway de Pagamento:** Stripe .NET SDK
* **Arquitetura:** Onion Architecture / Clean Architecture (Camadas de Domínio, Aplicação, Infraestrutura e API)
* **Segurança:** Autenticação via JWT (JSON Web Tokens) e Hash de senhas seguro (BCrypt / Identity).

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
* Instância do [MySQL](https://dev.mysql.com/downloads/) rodando localmente ou via Docker.
* Uma conta de testes no [Stripe](https://stripe.com/) para obter as chaves de API.

### Passos para Configuração

1. **Clonar o repositório:**

2. **Configurar as Variáveis de Ambiente:**
No arquivo appsettings.json (ou appsettings.Development.json) dentro do projeto principal da API, ajuste as credenciais do banco e as chaves do Stripe:
```bash
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=moviestream_db;Uid=seu_usuario;Pwd=sua_senha;"
    },
    "JwtSettings": {
      "Secret": "SUA_CHAVE_SECRETA_SUPER_SECRETA_DE_32_CARACTERES",
      "Issuer": "MovieStream",
      "Audience": "MovieStreamUsers"
    },
    "Stripe": {
      "SecretKey": "sk_test_sua_chave_secreta_do_stripe",
      "PublishableKey": "pk_test_sua_chave_publicavel_do_stripe"
    }
  }
```

3. **Executar as Migrations:**
```bash
dotnet ef database update
```

4. **Rodar a Aplicação:**
```bash
dotnet run
```
