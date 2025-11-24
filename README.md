# 🔐 Estudo de Identity e Autenticação com ASP.NET Core

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![Status](https://img.shields.io/badge/Status-Concluído-brightgreen)
![License](https://img.shields.io/badge/License-MIT-blue)

Este repositório é um projeto prático focado na implementação avançada de autenticação e autorização utilizando **ASP.NET Core Identity**. 

O objetivo principal foi ir além do template padrão, customizando o usuário, implementando fluxos reais de confirmação de conta via e-mail (SMTP) e aplicando boas práticas de arquitetura com Padrão de Camadas e Injeção de Dependência.

## 🚀 Funcionalidades e Conceitos Aplicados

### 👤 Gestão de Identidade (Identity)
- **Customização de Usuário:** Extensão da classe `IdentityUser` para `UsuarioModel` utilizando **`int`** como Chave Primária (PK) em vez do padrão `string/Guid`.
- **Registro e Login:** Fluxo completo de cadastro e autenticação.
- **Segurança:** Implementação de proteção contra CSRF e hashing de senhas.

### 📧 Integração de E-mail (SMTP)
- **Envio de E-mail Real:** Implementação do envio de e-mails transacionais usando a biblioteca **MailKit**.
- **Confirmação de Conta:** O usuário não pode logar sem confirmar o e-mail.
- **Token Seguro:** Geração de tokens de confirmação e **codificação URL-safe (Base64UrlEncode)** para evitar links quebrados.
- **Lógica de Reenvio:** Se um usuário não confirmado tenta logar, o sistema detecta e oferece o reenvio do link de confirmação.

### 🏗️ Arquitetura e Padrões
- **MVC (Model-View-Controller):** Separação clara de responsabilidades.
- **Repository/Service Pattern:** Encapsulamento da lógica de negócios (`UsuarioService`) e lógica de e-mail (`EmailService`).
- **DTOs (Data Transfer Objects):** Uso de DTOs para tráfego de dados entre View e Controller.
- **TempData & Partial Views:** Sistema de notificação visual (Sucesso/Erro) utilizando serialização customizada no TempData.

### 🧩 Estrutura do Projeto
A solução está organizada para facilitar a leitura e manutenção, seguindo o padrão MVC e separação de responsabilidades:
- `Controllers/`: Gerenciam o fluxo da aplicação e as requisições HTTP (`AccountController`, `HomeController`).

- `Services/`: Contém a lógica de negócio pesada e comunicação externa (`UsuarioService`, `EmailService`).

- `Models/`: Entidades que representam as tabelas do banco de dados (`UsuarioModel`).

- `Dto/`: Modelos de transferência de dados para entrada e validação (`UsuarioCadastroDto`, `LoginDto`).

- `Extensions/`: Métodos de extensão úteis, como helpers para serialização no TempData.

---

## 🛠️ Tecnologias Utilizadas

- **C# / .NET 8.0**
- **ASP.NET Core MVC**
- **Entity Framework Core** (SQLite)
- **ASP.NET Core Identity**
- **MailKit** (Protocolo SMTP)
- **Bootstrap 5** (Interface do Usuário)

---

## ⚙️ Como Configurar e Rodar

Siga os passos abaixo para rodar a aplicação em sua máquina local.

### 1. Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) instalado.

### 2. Clonar o Repositório
```bash
git clone [https://github.com/Ca22io/Estudo_Identity.git](https://github.com/Ca22io/Estudo_Identity.git)
cd Estudo_Identity
```

### 3. Configurar o SMTP (E-mail)
Para que o envio de e-mail funcione, você precisa configurar suas credenciais no arquivo `appsettings.json` (ou user-secrets).

> **⚠️ Nota Importante:** Se utilizar Gmail ou Outlook, você deve gerar uma **Senha de Aplicativo (App Password)**. Não utilize sua senha de login pessoal.

Edite o `appsettings.json`:

```json
"EmailSettings": {
  "Server": "smtp.gmail.com",  // ou smtp-mail.outlook.com
  "Port": 587,
  "SenderName": "Minha App de Estudo",
  "SenderEmail": "seu-email@gmail.com",
  "Username": "seu-email@gmail.com",
  "Password": "SUA_SENHA_DE_APP_AQUI" 
}
```

### 4. Banco de Dados e Migrations
Execute os comandos para criar o banco de dados e aplicar as tabelas do Identity:
```bash
dotnet ef database update
```
Rode o projeto:
```bash
dotnet run
```
