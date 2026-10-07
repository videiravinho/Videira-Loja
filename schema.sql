create table public.products (
id uuid primary key default gen_random_uuid(), name text not null check(length(trim(name))>0), country text not null default '', grape text not null default '', winery text not null default '', category text not null default 'Tinto', description text not null default '', image_url text not null default '', price numeric(12,2) check(price>=0), active boolean not null default true, featured boolean not null default false, created_at timestamptz not null default now());
alter table public.products enable row level security;
grant select on public.products to anon, authenticated;
grant insert, update, delete on public.products to authenticated;
create policy "Published catalog" on public.products for select to anon, authenticated using (active);
create policy "Admin products" on public.products for all to authenticated using (((select auth.jwt())->'app_metadata'->>'role')='admin') with check (((select auth.jwt())->'app_metadata'->>'role')='admin');
create table public.store_settings (id boolean primary key default true check(id), whatsapp text not null default '' check(whatsapp='' or whatsapp ~ '^[0-9]{10,15}$'));
alter table public.store_settings enable row level security;
grant select on public.store_settings to anon, authenticated;
grant update on public.store_settings to authenticated;
create policy "Public settings" on public.store_settings for select to anon, authenticated using(true);
create policy "Admin settings" on public.store_settings for update to authenticated using (((select auth.jwt())->'app_metadata'->>'role')='admin') with check (((select auth.jwt())->'app_metadata'->>'role')='admin');
insert into public.store_settings(id) values(true);