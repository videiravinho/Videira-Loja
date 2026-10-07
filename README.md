# Videira Loja

Catálogo responsivo em HTML, CSS e JavaScript, conectado ao Supabase Videira Loja. Não processa pagamentos: a seleção segue para o WhatsApp.

## Administrador
1. Em Supabase > Authentication > Users, crie e confirme a conta administrativa com o e-mail informado pelo proprietário. Defina a senha diretamente no Supabase.
2. O banco já está configurado para atribuir role admin automaticamente ao e-mail confirmado do proprietário.
3. Entre em /admin com essa conta.

Somente role admin em app_metadata permite cadastrar ou editar. Visitantes leem apenas produtos ativos. Nunca coloque service_role no frontend.

## Catálogo
Cadastre produtos no painel; o preço vazio aparece como Sob consulta. As imagens precisam de URL HTTPS. Use Publicado para ocultar ou mostrar rótulos. A seleção é salva neste navegador e os pedidos dependem de confirmação humana.

## Desenvolvimento
Sirva esta pasta por um servidor HTTP estático. Não há dependências de build. Configure a Vercel com framework Other, sem comando de build, diretório de saída raiz. /loja abre o catálogo, /admin abre o painel.

## Estado inicial
Catálogo vazio conforme solicitado; WhatsApp 5545999056277. Esquema do banco em schema.sql. A fonte visual é videiravinhoteca.shop, com ilustração original feita em CSS.
