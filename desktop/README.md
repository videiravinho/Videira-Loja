# VIDEIRA — painel conectado ao site

## Escolha o aplicativo
- VIDEIRA PAINEL DO SITE.exe: abre somente o catálogo do site.
- Instalar VIDEIRA GESTÃO 3.9.exe: atualiza o Gestão e cria/atualiza o atalho. Abra Site > Catálogo do site. Feche a versão antiga antes de instalar.
- VIDEIRA GESTÃO 3.9.exe: versão completa, para abrir diretamente.

A área Site usa o banco online. Estoque, custos, vendas, clientes e financeiro continuam na base local do Gestão. Cadastrar um item no estoque local não o publica no site.

## Primeiro acesso
1. O e-mail é videiravinhoteca@gmail.com.
2. Digite uma nova senha com pelo menos 10 caracteres e clique em Criar primeiro acesso.
3. Abra o e-mail de confirmação e confirme o cadastro. Depois volte ao aplicativo e entre com sua senha.
4. Se já criou a conta, clique apenas em Entrar. A senha não é gravada no computador.

## Produtos
Clique em Novo produto. Preencha nome, categoria, preço, país, uva, bodega e descrição. Categorias podem ser digitadas livremente. Preço vazio significa sob consulta; para centavos use 89,90.

Para fotos, use Escolher e enviar foto (JPG, PNG ou WebP até 6 MB) ou informe uma URL HTTPS. Enviar a foto não salva o produto: finalize com Salvar produto no site.

Publicado na loja controla a visibilidade. Destaque na página inicial coloca o vinho na seleção da home. Se não houver destaques, a home apresenta os produtos mais recentes.

Selecione um item e clique em Editar selecionado para alterar. Ocultar no site retira o produto da vitrine sem apagar o cadastro. Para republicar, edite e marque Publicado na loja.

Salvar grava no mesmo Supabase usado por https://videira-loja.vercel.app/. A confirmação só aparece depois da resposta do servidor. Atualize o navegador para ver as mudanças. Atualizar no painel recarrega a lista online. Não há fila de salvamento offline.

O WhatsApp também pode ser alterado na parte inferior do formulário; informe o código do país e DDD.

## Validação realizada
Executáveis compilados, telas renderizadas e testes existentes do Gestão aprovados. O cliente C# leu catálogo/contato online e confirmou bloqueio de edição sem login. Inclusão e edição com a política administradora foram verificadas em transação revertida, sem produtos de teste publicados. Auditoria Supabase sem apontamentos de segurança. O teste com sua sessão real e o envio autenticado de foto dependem do seu primeiro acesso e da confirmação do e-mail.
