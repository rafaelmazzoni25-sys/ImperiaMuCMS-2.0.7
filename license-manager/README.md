# License Manager Desktop App

Este diretório contém um aplicativo Windows Forms em C# para gerenciar as licenças do ImperiaMuCMS.

## Recursos principais

- Visualização das licenças por status (ativas, inativas e banidas) com contadores no rodapé.
- Filtros rápidos, busca e atualização sem precisar reiniciar a aplicação.
- Cadastro, edição, exclusão e mudança de status das licenças.
- Definição opcional de data de expiração, com suporte a notas/observações.
- Persistência dos dados em `licenses.json`, utilizando JSON legível.

## Estrutura do projeto

```
license-manager/
 ├─ LicenseManager.sln
 └─ LicenseManager/
    ├─ LicenseManager.csproj
    ├─ Program.cs
    ├─ ApplicationConfiguration.cs
    ├─ MainForm.cs / MainForm.Designer.cs
    ├─ LicenseDialog.cs / LicenseDialog.Designer.cs
    ├─ Models/
    │   ├─ License.cs
    │   └─ LicenseStatus.cs
    ├─ Services/
    │   └─ LicenseRepository.cs
    └─ licenses.json
```

## Como compilar

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

## Próximos passos sugeridos

- Sincronizar com uma API REST (como o servidor de licenças PHP) para manter o desktop em linha com o ambiente de produção.
- Adicionar autenticação de usuários e trilhas de auditoria.
- Criar filtros avançados por data de criação, expiração e texto livre.
