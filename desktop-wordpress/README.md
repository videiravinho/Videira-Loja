# Videira Site 2.0

Painel Windows separado do Gestão, com organização inspirada na administração do WordPress.

- Layout ajustado para janelas de 1024 pixels ou maiores, com navegação lateral e rolagem.
- Resumo do catálogo: total, publicados, rascunhos, categorias e campos incompletos.
- Pesquisa por nome, categoria, país, uva e produtor; filtros de publicação, destaque, foto e categoria; ordenação por nome e preço.
- Publicar, mover para rascunho, marcar e remover destaque em lote. A seleção e a confirmação do servidor são verificadas.
- Duplicação abre uma cópia como rascunho; salvar cria um novo produto.
- CSV dos produtos filtrados com escape de texto e proteção contra fórmulas.
- Editor com dados, publicação e imagem; categorias com renomeação conjunta dos produtos; biblioteca pesquisável.
- Autenticação administradora e recuperação de senha iniciada pelo usuário. Não armazena senhas ou tokens em arquivos.

Os dados são gravados no Supabase usado pela loja. O executável contém somente a chave publicável; as políticas do banco restringem gravações ao papel administrador de app_metadata.

Compilar no Windows com .NET Framework 4 e os cinco arquivos C#. `build.ps1` no workspace utiliza o ícone do Gestão; em outra pasta, remova /win32icon ou forneça o ícone local. Nenhuma dependência externa de UI é necessária.

`--self-test` usa dados fictícios apenas em memória, verifica páginas, filtros, limites de largura e listas vazias e gera imagens em screens-v2. `CatalogCheck.cs` verifica a API de atualização em lote com um HttpMessageHandler simulado. `AuthCheck.cs` verifica erro de senha e rejeição de link externo. Os testes não gravam no catálogo real. A entrada autenticada depende da senha do usuário.

Fontes de referência: https://wordpress.org/documentation/article/administration-screens/ e https://help.shopify.com/en/manual/products/collections/search-view.
