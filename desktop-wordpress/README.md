# Videira Site

Abra Videira Site.exe e entre com videiravinhoteca@gmail.com e a mesma senha já cadastrada. Não é necessário criar outra conta.

O aplicativo tem o visual de administração da referência WordPress e usa o banco online do catálogo Videira. As alterações dependem de conexão com a internet e do login de administrador. Sua senha e os tokens de sessão não são gravados em arquivos.

- Painel: resumo do catálogo e criação rápida de rascunho de produto.
- Produtos: pesquisar, filtrar, editar e mover produtos para rascunho.
- Adicionar produto: nome, preço, categoria, país, uva, bodega, descrição, foto, publicação e destaque. Um produto novo fica como rascunho até marcar Publicado na loja. Preço vazio significa sob consulta.
- Categorias: criar e renomear. Renomear atualiza também os produtos vinculados no site.
- Mídia: imagens associadas aos produtos. Dê dois cliques para editar o produto. Envie novas fotos pelo editor (JPG, PNG ou WebP até 6 MB), ou use URL HTTPS.
- Configurações: WhatsApp da loja com país e DDD.

Salvar rascunho guarda o produto online, sem exibir na loja. Salvar/atualizar produto grava no mesmo banco do site. Após salvar, atualize a loja no navegador para visualizar o resultado. O envio da foto é uma etapa separada: salve o produto para associá-la ao cadastro.

## Gestão separado
Feche a versão antiga e execute Instalar VIDEIRA GESTÃO 3.11.exe para atualizar o Gestão. Essa versão remove o menu Site. Os dados locais de estoque, custos, vendas e financeiro são preservados. O novo Videira Site.exe funciona independentemente do Gestão.

## Validação
As telas do novo painel foram abertas e renderizadas. O cliente C# consultou o catálogo e as categorias online. A política administradora e a renomeação conjunta de categoria/produtos foram verificadas numa transação revertida. A conta administradora permanece com o perfil admin. Os testes existentes do Gestão passaram. O teste com sua senha e sessão real será feito por você ao entrar; não acessamos nem alteramos sua senha.

A auditoria do Supabase mantém o aviso já existente sobre [proteção de senhas vazadas](https://supabase.com/docs/guides/auth/password-security#password-strength-and-leaked-password-protection).
