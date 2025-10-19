# 💼 Portfolio — Matheus Szoke

<p align="center">
  <img src="https://mathszoke.com/assets/banner.png" alt="Portfolio Banner" width="800"/>
</p>

<p align="center">
  <b>My personal portfolio developed with .NET Aspire and React, showcasing my professional journey, experiences, and projects in a fully integrated cloud ecosystem.</b>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/React-61DAFB?style=for-the-badge&logo=react&logoColor=black"/>
  <img src="https://img.shields.io/badge/Azure-0078D4?style=for-the-badge&logo=microsoftazure&logoColor=white"/>
  <img src="https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white"/>
</p>

---

## 🧠 About the Project

This portfolio was created to represent my journey as a full-stack developer, bringing together my **professional experiences**, **real-world projects**, and **technical skills** in an interactive and elegant way.

The application reflects my development philosophy: **organization, scalability, and architectural clarity**.

> “Clean code is an extension of the developer’s mind — simple, predictable, and elegant.”

---

## ⚙️ Core Stack

| Layer | Technologies |
|:--|:--|
| **Frontend** | React + Fluent UI + i18next |
| **Backend** | .NET Aspire 9 (Clean Architecture, CQRS, DDD) |
| **Database** | PostgreSQL (via EF Core) |
| **Infrastructure** | Azure App Service + Azure Redis + Postgres Flexible Server |

---

## 🧩 Project Structure

```
src/
 ├─ Aspire/Portfolio.AppHost/         → Orquestrador do .NET Aspire
 │                                     - Define ServiceDefaults, health checks, configuração distribuída
 │                                     - Orquestra serviços (API, AIAgent, DB, Redis, etc.) no dev
 │                                     - Facilita observabilidade e composition local

 ├─ Backend/Portfolio.Api/            → Camada de Apresentação (API)
 │                                     - Endpoints/Controllers (REST)
 │                                     - Autenticação/Autorização, filtros/middlewares
 │                                     - DI/Composition Root: registra Application + Infrastructure
 │                                     - Versionamento, Swagger/OpenAPI, validação de requests

 ├─ Backend/Portfolio.Application/    → Camada de Aplicação (Casos de Uso)
 │                                     - Handlers CQRS (Commands/Queries), orquestra regra de negócio
 │                                     - DTOs/ViewModels/Mappers
 │                                     - Validações (FluentValidation), regras de aplicação
 │                                     - Portas/Interfaces (ex.: IEmailSender, IUnitOfWork, ICurrentUser)
 │                                     - Publica/consome eventos de domínio via MediatR (quando aplicável)
 │                                     - NÃO conhece detalhes de infra nem do HTTP – só contratos

 ├─ Backend/Portfolio.Domain/         → Camada de Domínio (Coração do negócio)
 │                                     - Entidades, Agregados, Value Objects
 │                                     - Eventos de domínio
 │                                     - Erros de domínio, Result types base, BaseEntity, etc.
 │                                     - Zero dependência de infraestrutura/UI; só C# puro

 ├─ Backend/Portfolio.Infrastructure/ → Camada de Infraestrutura (implementações técnicas)
 │                                     - EF Core (DbContext, configurations), repositórios concretos
 │                                     - Migrations (se não estiverem em Infra.Database)
 │                                     - Integrações externas (Azure, fila/mensageria, email, cache, storage)
 │                                     - Logging/Telemetry, Polly, HttpClients
 │                                     - Implementações de portas da Application (ex.: IEmailSender)
 │                                     - Depende de Domain e expõe implementações para Application/API

 ├─ Backend/Portfolio.AIAgent/        → Serviço de IA (em desenvolvimento)
 │                                     - Adapters para modelos (ex.: Ollama e GPT para produção)
 │                                     - Endpoints internos/Worker para tarefas de IA
 │                                     - Pipelines de prompt, ferramentas/agents, caching de respostas

 ├─ Frontend/Frontend.Modules.UI/     → Interface Web (React + Fluent UI)
 │                                     - Páginas/Seções (About, Projects), i18n, temas
 │                                     - Componentes UI (carrossel, badges, cards)
 │                                     - Integração com API, roteamento, animações

 ├─ Infrastructure/Infra.Database/    → Projeto de Banco (opcional/separado)
 │                                     - Migrações EF Core isoladas
 │                                     - Scripts SQL, seeds, utilitários para DB

 └─ SharedKernel/                     → Núcleo compartilhado
                                       - Tipos utilitários (Result<T>, Error, Paginação)
                                       - Abstrações base (BaseEntity, DomainEvent, IHasDomainEvents)
                                       - Contratos comuns (ex.: IDateTime, IGuidGenerator)
                                       - Constantes, exceptions e helpers sem dependência pesada
```

---

## 🌟 Key Features

- Detailed display of professional experiences  
- Dynamic and multilingual *About Me* section (🇧🇷 / 🇺🇸)  
- Integrated *Projects* section powered by the API
- Fully responsive design optimized for desktop and mobile
- Scalable cloud deployment managed on AWS Lambda for API and Github pages for frontend React.  

---

## 🎨 Design & UX

The visual design aims for a **clean and modern professional identity**, focusing on readability, smooth animations, and a neutral color palette.  
Each component was designed using Fluent UI and styled with `makeStyles` to maintain a consistent experience across Microsoft’s ecosystem.

---

## 🌐 Live Demo

🔗 **[mathszoke.com](https://mathszoke.com)**  
> The production version is continuously updated via automated GitHub Actions pipelines.

---

## 📫 Contact

📧 **Email:** [matheusszoke@gmail.com](mailto:matheusszoke@gmail.com)  
💼 **LinkedIn:** [linkedin.com/in/matheusszoke](https://linkedin.com/in/matheusszoke)  
🌐 **Website:** [mathszoke.com](https://mathszoke.com)

---

<p align="center">
  <sub>Made with 💚 by <strong>Matheus Szoke</strong></sub>
</p>
