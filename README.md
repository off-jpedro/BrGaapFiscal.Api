# API de Gerenciamento de Notas Fiscais

## Sobre
Este projeto é uma API para gerenciar notas fiscais, clientes e fornecedores. A API foi desenvolvida utilizando C# com o framework .NET e o banco de dados PostgreSQL. O objetivo é fornecer uma base sólida para operações CRUD (Create, Read, Update, Delete) para as entidades Nota Fiscal, Cliente e Fornecedor.

## Tecnologias Utilizadas
- **Linguagem:** C#
- **Framework:** .NET
- **Banco de Dados:** PostgreSQL
- **Frontend:** SAPUI5 (integração com a API)

## Funcionalidades
- Cadastro de notas fiscais com cliente e fornecedor
- Listagem de notas fiscais
- Atualização e remoção de notas fiscais
- Integração com frontend SAPUI5

## Instalação e Configuração
### Requisitos:
- .NET SDK instalado
- PostgreSQL instalado e configurado

### Passos:
1. Clone o repositório:
   ```sh
   git clone https://github.com/off-jpedro/BrGaapFiscal.Api.git
   ```
2. Acesse o diretório do projeto:
   ```sh
   cd seu-repositorio
   ```
3. Configure a string de conexão com o banco de dados no `appsettings.json`.
4. Execute as migrações para criar o banco de dados:
   ```sh
   dotnet ef database update
   ```
5. Inicie a API:
   ```sh
   dotnet run
   ```

## Endpoints Principais
### Notas Fiscais
- `GET /api/notafiscal` - Lista todas as notas fiscais
- `POST /api/notafiscal` - Cria uma nova nota fiscal
- `PUT /api/notafiscal/{id}` - Atualiza uma nota fiscal
- `DELETE /api/notafiscal/{id}` - Remove uma nota fiscal

## Contribuição
Contribuições são bem-vindas! Para contribuir:
1. Faça um fork do projeto
2. Crie uma branch para sua funcionalidade: `git checkout -b minha-feature`
3. Faça commit das suas mudanças: `git commit -m 'Adiciona nova funcionalidade'`
4. Envie para o repositório remoto: `git push origin minha-feature`
5. Abra um Pull Request


