# Atualização 3.10 — Catálogo do site

A conta videiravinhoteca@gmail.com estava confirmada, mas sem o perfil administrador. O perfil foi corrigido no banco e a regra de atribuição foi ajustada para preservá-lo nas atualizações do cadastro.

Feche a versão antiga, instale a versão 3.10 e abra Site > Catálogo do site. Entre com o mesmo e-mail e senha. Não é necessário criar outra conta. Se tiver sessão aberta, use Sair da conta e entre novamente para receber a permissão atualizada.

A área Site usa agora fonte Tahoma, botões, ícones, cores, título e grade compartilhados com o restante do Gestão. Produtos online continuam separados de estoque, custos, vendas e financeiro locais.

Verificação: conta confirmada com app_metadata.role=admin; perfil preservado após atualização de metadados em teste revertido; inclusão e edição de preço sob a política administradora em transação revertida; conexão HTTPS do executável e bloqueio de edição anônima conferidos; interface renderizada.

O teste de entrada com sua senha e sessão real deve ser feito por você; não acessamos nem alteramos sua senha.

A auditoria Supabase também indicou proteção contra senhas vazadas desativada: [documentação de proteção de senhas](https://supabase.com/docs/guides/auth/password-security#password-strength-and-leaked-password-protection).
