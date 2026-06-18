# API de Gerenciamento de Salas de Reunião

API REST para gerenciar salas e reservas, com validações de horário comercial e prevenção de conflitos.

## Tecnologias

- .NET 8 / ASP.NET Core
- Entity Framework Core + SQLite
- Swagger

## Como Executar

```bash
dotnet restore
dotnet run
```

Acesse: `https://localhost:5001/swagger`

O banco de dados (`reunioes.db`) é criado automaticamente na primeira execução.

## Estrutura

```
SalasReunioes/
├── Controllers/
│   ├── SalasController.cs
│   └── ReservasController.cs
├── Models/
│   ├── Sala.cs
│   └── Reserva.cs
├── Services/
│   ├── SalaService.cs
│   └── ReservaService.cs
└── Data/
    └── AppDbContext.cs
```

## Endpoints

### Salas (`/api/Salas`)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/Salas?pagina=1&busca=nome` | Listar salas (paginado, filtrável por nome, ordenado por andar) |
| POST | `/api/Salas` | Criar sala |
| PUT | `/api/Salas/{id}` | Atualizar sala |
| DELETE | `/api/Salas/{id}` | Remover sala |

### Reservas (`/api/Reservas`)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/Reservas` | Listar reservas |
| POST | `/api/Reservas` | Criar reserva |
| PUT | `/api/Reservas/{id}/reagendar` | Reagendar reserva |
| DELETE | `/api/Reservas/{id}` | Cancelar reserva |
| GET | `/api/Reservas/resumo` | Total de reuniões nos últimos 7 dias |
| GET | `/api/Reservas/horas-livres?inicio=2026-06-20&fim=2026-06-25` | Horas livres por sala no período |

## Regras de Negócio

- Reservas permitidas apenas entre **08:00 e 19:00**
- Não permite reservas conflitantes na mesma sala
- Só é possível **reagendar** reservas futuras
- Cancelamento permitido para qualquer reserva

## Exemplos

### Criar sala

```json
{
  "nome": "Sala Executiva",
  "andar": 5,
  "quantidadeAssentos": 12
}
```

### Criar reserva

```json
{
  "inicio": "2026-06-20T10:00:00",
  "fim": "2026-06-20T11:30:00",
  "salaId": 1
}
```

### Reagendar reserva

```json
{
  "inicio": "2026-06-21T14:00:00",
  "fim": "2026-06-21T16:00:00",
  "salaId": 1
}
```
## Banco de Dados

O banco SQLite (`reunioes.db`) é criado automaticamente na primeira execução via Entity Framework Core.

O arquivo `schema.sql` contém o script de criação das tabelas caso queira criar manualmente.
