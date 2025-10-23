# Microsserviço de Agendamento - Plataforma de Beleza

Este repositório contém o código-fonte e a documentação arquitetural para o **Microsserviço de Agendamento**, um componente central da Plataforma de Serviço de Beleza.

## 1\. Cenário de Negócio

[cite\_start]A plataforma é um ecossistema digital que conecta clientes a profissionais de beleza, como salões e barbearias[cite: 3]. [cite\_start]O objetivo é oferecer confiança, praticidade e eficiência tanto para clientes quanto para os estabelecimentos[cite: 4].

O Microsserviço de Agendamento é o coração da operação, sendo responsável por gerenciar a funcionalidade mais crítica: a marcação e gestão de horários. Os desafios de negócio abordados são:

  * [cite\_start]**Complexidade da Agenda:** Lidar com diferentes durações de serviços, múltiplos profissionais e agendamentos simultâneos[cite: 91].
  * [cite\_start]**Disponibilidade e Confiança:** Oferecer aos clientes acesso a agendas em tempo real ($24/7$) com confirmação instantânea[cite: 44, 45].
  * [cite\_start]**Eficiência Operacional:** Reduzir o tempo gasto por profissionais com agendamentos manuais e diminuir o "não comparecimento" (no-shows) através de lembretes automáticos[cite: 22, 26].

A aplicação de Design Patterns foi fundamental para construir um sistema que atende a esses requisitos de forma manutenível, testável e escalável.

## 2\. Stack Tecnológica

  * **Linguagem:** C\# (.NET 9)
  * **Servidor HTTP:** `HttpListener` (nativo do .NET, sem frameworks web)
  * **Acesso a Dados:** ADO.NET puro (sem ORM)
  * **Drivers de Banco de Dados:**
      * PostgreSQL: `Npgsql`
      * MySQL: `MySqlConnector`
  * **Containerização:** Docker

## 3\. Arquitetura do Projeto

O projeto foi estruturado seguindo o padrão arquitetural **Clean Architecture (Arquitetura Limpa)**, também conhecido como **Ports & Adapters** ou **Arquitetura Hexagonal**. Esta abordagem isola o núcleo da lógica de negócio de dependências externas como banco de dados, frameworks e UI.

### 3.1. Estrutura de Camadas

  * **Domínio (O Coração):** Contém as entidades (`Appointment`), objetos de valor e regras de negócio puras. É a camada mais interna e não possui dependências externas.
  * **Aplicação (Serviços/Casos de Uso):** Orquestra a lógica do domínio para executar os casos de uso do negócio (ex: `AgendarServicoUseCase`). Define as interfaces (**Portas**) para as dependências, como `IAppointmentRepository` e `IUnitOfWork`.
  * **Infraestrutura (Controladores e Persistência):** Camada externa que contém os **Adaptadores**.
      * **Controladores:** O servidor `HttpListener` atua como um adaptador de entrada, recebendo requisições HTTP.
      * **Persistência:** A biblioteca `ShrimpDatabaseManager` é um adaptador de saída que implementa as interfaces de persistência, traduzindo as solicitações da aplicação em comandos SQL.

### 3.2. Estrutura de Pastas

```
/src/
├── domain/
│   └── Appointment.cs         # Entidades e regras de negócio puras
├── application/
│   ├── usecases/              # Lógica dos casos de uso
│   └── ports/                 # Interfaces (IAppointmentRepository, IUnitOfWork)
└── infrastructure/
    ├── web/                   # Servidor HttpListener, roteamento, controllers
    └── persistence/           # Implementação da persistência (ShrimpDatabaseManager)
        ├── Adapters/
        ├── Mappers/
        ├── Repositories/
        └── UnitOfWork/
```

## 4\. Justificativa dos Padrões Aplicados

### Padrão Arquitetural: Ports & Adapters (Hexagonal)

  * **Justificativa Técnica:** Inverte as dependências tradicionais. A lógica de negócio define "Portas" (interfaces), e os componentes de infraestrutura (banco de dados, UI) fornecem "Adaptadores" (implementações concretas).
  * **Benefício para o Negócio:** Isola a lógica vital de agendamento de detalhes tecnológicos, aumentando a testabilidade e a confiabilidade do sistema. A capacidade de usar um `InMemoryAdapter` para testes acelera o desenvolvimento.

### Padrão de Persistência: Repository e Data Mapper

  * **Justificativa Técnica:** O **Repository** abstrai a fonte de dados, agindo como uma coleção de objetos de domínio. O **Data Mapper** isola os objetos de domínio do esquema do banco de dados, realizando a tradução entre eles.
  * **Benefício para o Negócio:** Garante flexibilidade. Se a plataforma decidir migrar do PostgreSQL para um banco NoSQL no futuro, apenas a camada de persistência precisará ser alterada. A lógica de negócio, como validar um horário, permanece intacta.

### Padrão de Transação: Unit of Work

  * **Justificativa Técnica:** Gerencia as transações para garantir a atomicidade das operações de negócio. Agrupa múltiplas inserções e atualizações em uma única transação, realizando `commit` em caso de sucesso ou `rollback` em caso de falha.
  * **Benefício para o Negócio:** É indispensável para a funcionalidade de agendamento. Uma única marcação pode envolver várias operações no banco de dados. [cite\_start]O `Unit of Work` previne inconsistências na agenda (ex: um horário bloqueado sem um agendamento correspondente), o que aumenta a confiança do profissional na ferramenta[cite: 6].

## 5\. Análise Crítica das Decisões Arquiteturais

A decisão de não utilizar um ORM e implementar manualmente a camada de persistência trouxe os seguintes prós e contras:

### Prós

  * **Testabilidade Máxima:** A arquitetura permitiu testes de unidade rápidos e isolados (`InMemoryAdapter`) e testes de integração precisos contra os bancos de dados reais.
  * **Desempenho Otimizado:** O controle total sobre as queries SQL permite otimizações de performance que podem ser difíceis de alcançar com um ORM.
  * **Flexibilidade e Baixo Acoplamento:** O sistema não está acoplado a nenhum framework de persistência, permitindo a troca de tecnologia de banco de dados com esforço mínimo.

### Contras (Trade-offs)

  * **Verbosidade e Complexidade Inicial:** A quantidade de código necessária é maior em comparação com uma abordagem usando um ORM como o Entity Framework Core. Foi preciso criar manualmente interfaces, `DataMappers` e a lógica de transação.
  * **Maior Custo de Desenvolvimento:** O tempo para desenvolver a funcionalidade inicial é maior, pois funcionalidades que um ORM oferece prontas precisaram ser implementadas do zero.
