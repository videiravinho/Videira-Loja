# Videira Loja

Catálogo responsivo em HTML, CSS e JavaScript, conectado ao Supabase Videira Loja. Não processa pagamentos: a seleção segue para o WhatsApp.

## Administrador
1. Em Supabase > Authentication > Users, crie uma conta administrativa e configure sua senha diretamente no painel.
2. No SQL Editor, execute (substituindo o e-mail):

```sql
update auth.users set raw_app_meta_data = coalesce(raw_app_meta_data, '{}'::jsonb) || '{"role":"admin"}'::jsonb where email = 'SEU_EMAIL';
```
3. Entre em /admin com essa conta. Se já estava conectado, saia e entre novamente para atualizar o token.

Somente role admin em app_metadata permite cadastrar ou editar. Visitantes leem apenas produtos ativos. Nunca coloque service_role no frontend.

## Catálogo
Cadastre produtos no painel; o preço vazio aparece como Sob consulta. As imagens precisam de URL HTTPS. Use Publicado para ocultar ou mostrar rótulos. A seleção é salva neste navegador e os pedidos dependem de confirmação humana.

## Desenvolvimento
Sirva esta pasta por um servidor HTTP estático. Não há dependências de build. Configure a Vercel com framework Other, sem comando de build, diretório de saída raiz. /loja abre o catálogo, /admin abre o painel.

## Estado inicial
Catálogo vazio conforme solicitado; WhatsApp 5545999056277. Esquema do banco em schema.sql. A fonte visual é videiravinhoteca.shop, com ilustração original feita em CSS.
