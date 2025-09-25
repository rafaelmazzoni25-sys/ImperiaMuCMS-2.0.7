
# Ferramentas de Licenciamento do ImperiaMuCMS

Este diretório reúne o gerenciador desktop em Windows Forms e o novo servidor de licenças escrito em C#, ambos compartilhando a mesma camada de modelos e persistência.

## Recursos principais do LicenseManager

- Visualização das licenças por status (ativas, inativas e banidas) com contadores no rodapé.
- Filtros rápidos por status **e tipo de licença** para acompanhar facilmente os diferentes planos.
- Cadastro, edição, exclusão e mudança de status das licenças.
- Definição do tipo de licença compatível com o CMS (`Lite`, `Bronze`, `Silver`, `Gold`, `Premium` e `PremiumPlus`).
- Definição opcional de data de expiração, com suporte a notas/observações e aos campos extras utilizados pelo CMS.
- Persistência dos dados em `licenses.json`, utilizando JSON legível e compartilhado com o servidor C#.
- Autenticação de usuários com auditoria completa das operações realizadas no gerenciador desktop.
- Backups automáticos versionados do arquivo `licenses.json`, facilitando a restauração de estados anteriores.
- Painel administrativo web com login e controle remoto das licenças, compartilhando o mesmo repositório de dados.

## Estrutura do projeto

```
license-manager/
 ├─ LicenseManager.sln
 ├─ LicenseManager.Core/         # Modelos, enums e repositório compartilhado
 ├─ LicenseManager/              # Aplicativo Windows Forms
 └─ LicenseServer/               # Servidor HTTP compatível com o CMS
```

## Como compilar o gerenciador desktop

1. Instale o [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) em uma máquina Windows.
2. Abra o arquivo `LicenseManager.sln` no Visual Studio 2022 ou execute o comando abaixo em um terminal do Developer Command Prompt:

   ```bash
   dotnet restore
   dotnet build
   ```

3. Execute o aplicativo via Visual Studio (`F5`) ou com o comando:

   ```bash
   dotnet run --project LicenseManager/LicenseManager.csproj
   ```

O arquivo `licenses.json` será carregado automaticamente da pasta de saída. Você pode editar este arquivo manualmente caso deseje fazer importações em massa.

## Como executar o servidor de licenças em C#

1. Garanta que o .NET 6 SDK esteja instalado (Windows ou Linux).
2. Ajuste, se necessário, o caminho de armazenamento em `LicenseServer/appsettings.json` (por padrão ele aponta para `../LicenseManager/licenses.json`).
3. Execute o servidor com:

   ```bash
   dotnet run --project LicenseServer/LicenseServer.csproj
   ```

4. Os endpoints expostos são equivalentes aos utilizados pelo CMS original:
  - `GET /apiversion.php` → versão mínima da API (`1`).
  - `GET /version.php` → versão atual do CMS (`2.0.7`).
  - `GET /applications/nexus/interface/licenses/?info|check|activate` → rotas de informação, verificação e ativação, retornando respostas criptografadas como o servidor original.

As licenças ativas, tipos e campos personalizados são lidos do mesmo `licenses.json` gerenciado pelo aplicativo desktop.

### Painel administrativo web

Com o servidor em execução, acesse `http://localhost:5000/admin` (ou a porta configurada pelo `dotnet run`) para utilizar o painel remoto. Faça login com um usuário cadastrado em `users.json` (por padrão `admin`/`admin`) e:

- Cadastre, edite ou exclua licenças diretamente pelo navegador.
- Altere rapidamente o status das chaves.
- Consulte o histórico de auditoria das ações realizadas tanto no desktop quanto via web.

> Recomenda-se alterar a senha padrão do usuário `admin` antes de expor o painel à internet. O arquivo `users.json` aceita múltiplos usuários com senhas criptografadas em SHA-256.

## Próximos passos sugeridos

- Disponibilizar um fluxo seguro para alteração de senhas diretamente pela interface.
- Permitir a gestão de múltiplos usuários e perfis de acesso diferenciados.
- Configurar notificações automáticas (e-mail/webhook) para licenças próximas da expiração.
