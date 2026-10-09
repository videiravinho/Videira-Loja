begin;
create table if not exists public.catalog_terms (
 id uuid primary key default gen_random_uuid(),
 kind text not null check(kind in ('tipo','uva','pais','bodega','colecao')),
 name text not null check(length(btrim(name)) between 1 and 100),
 parent_id uuid references public.catalog_terms(id) on delete restrict,
 unique(kind,name),check(parent_id is distinct from id)
);
alter table public.catalog_terms enable row level security;
grant select on public.catalog_terms to anon,authenticated;
grant insert,update,delete on public.catalog_terms to authenticated;
create policy "Catalog term reads" on public.catalog_terms for select to anon,authenticated using(true);
create policy "Catalog term admin" on public.catalog_terms for all to authenticated using((select auth.jwt())->'app_metadata'->>'role'='admin') with check((select auth.jwt())->'app_metadata'->>'role'='admin');
create or replace function public.validate_catalog_parent() returns trigger language plpgsql security invoker set search_path=public as $$
declare p uuid; k text; seen uuid[] := array[new.id];
begin
 p:=new.parent_id;
 while p is not null loop
  if p=any(seen) then raise exception 'A hierarquia não pode formar um ciclo'; end if;
  seen:=array_append(seen,p);
  select kind,parent_id into k,p from public.catalog_terms where id=p;
  if k is distinct from new.kind then raise exception 'A subcategoria deve pertencer ao mesmo grupo da categoria principal'; end if;
 end loop;
 return new;
end $$;
create trigger catalog_parent_guard before insert or update on public.catalog_terms for each row execute function public.validate_catalog_parent();
alter table public.products add column term_ids uuid[] not null default '{}', add column sale_price numeric,add column pix_price numeric,add column card_price numeric,add column promotion_start timestamptz,add column promotion_end timestamptz;
alter table public.products add constraint catalog_sale_valid check(sale_price is null or (price is not null and sale_price>=0 and sale_price<price)), add constraint catalog_pix_valid check(pix_price is null or (price is not null and pix_price>=0 and pix_price<=price)),add constraint catalog_card_valid check(card_price is null or (price is not null and card_price>=0)),add constraint catalog_promo_dates check(promotion_start is null or promotion_end is null or promotion_end>=promotion_start);
insert into public.catalog_terms(kind,name) select 'tipo',name from public.product_categories on conflict do nothing;
insert into public.catalog_terms(kind,name) select distinct 'tipo',category from public.products where btrim(coalesce(category,''))<>'' on conflict do nothing;
insert into public.catalog_terms(kind,name) select distinct 'pais',country from public.products where btrim(coalesce(country,''))<>'' on conflict do nothing;
insert into public.catalog_terms(kind,name) select distinct 'uva',grape from public.products where btrim(coalesce(grape,''))<>'' on conflict do nothing;
insert into public.catalog_terms(kind,name) select distinct 'bodega',winery from public.products where btrim(coalesce(winery,''))<>'' on conflict do nothing;
update public.products p set term_ids=(select coalesce(array_agg(t.id),'{}') from public.catalog_terms t where (t.kind='tipo' and t.name=p.category) or (t.kind='pais' and t.name=p.country) or (t.kind='uva' and t.name=p.grape) or (t.kind='bodega' and t.name=p.winery));
create or replace function public.sync_catalog_product() returns trigger language plpgsql security invoker set search_path=public as $$
begin
 if exists(select 1 from unnest(new.term_ids) i where not exists(select 1 from public.catalog_terms t where t.id=i)) then raise exception 'Uma categoria não existe mais. Atualize o catálogo'; end if;
 if cardinality(new.term_ids)>0 then
  select coalesce(string_agg(name,', ' order by name),'') into new.category from public.catalog_terms where id=any(new.term_ids) and kind='tipo';
  select coalesce(string_agg(name,', ' order by name),'') into new.country from public.catalog_terms where id=any(new.term_ids) and kind='pais';
  select coalesce(string_agg(name,', ' order by name),'') into new.grape from public.catalog_terms where id=any(new.term_ids) and kind='uva';
  select coalesce(string_agg(name,', ' order by name),'') into new.winery from public.catalog_terms where id=any(new.term_ids) and kind='bodega';
 end if;
 return new;
end $$;
create trigger catalog_product_sync before insert or update on public.products for each row execute function public.sync_catalog_product();
create or replace function public.sync_catalog_term_name() returns trigger language plpgsql security invoker set search_path=public as $$
begin
 if new.name is distinct from old.name then update public.products set term_ids=term_ids where new.id=any(term_ids); end if;
 return new;
end $$;
create trigger catalog_term_rename after update on public.catalog_terms for each row execute function public.sync_catalog_term_name();
alter table public.store_settings add column store_name text not null default 'Videira Vinhoteca',add column tagline text not null default 'Descubra seu próximo vinho',add column contact_email text not null default '',add column instagram_url text not null default '',add column address text not null default '',add column opening_hours text not null default '',add column footer_text text not null default 'Beba com moderação. Venda de bebidas alcoólicas proibida para menores de 18 anos.',add column logo_url text not null default '',add column primary_color text not null default '#73233a' check(primary_color ~ '^#[0-9a-fA-F]{6}$'),add column hero_title text not null default '',add column hero_text text not null default '',add column hero_image_url text not null default '',add column hero_link_label text not null default '',add column hero_link_url text not null default '/loja',add column featured_title text not null default 'A seleção Videira',add column announcement text not null default '',add column meta_description text not null default '',add column show_prices boolean not null default true,add column card_installments integer not null default 1 check(card_installments between 1 and 12),add column card_min_installment numeric check(card_min_installment is null or card_min_installment>0),add column card_interest_free boolean not null default false,add column payment_note text not null default 'Pagamento e disponibilidade confirmados no atendimento.';
notify pgrst,'reload schema';
commit;
