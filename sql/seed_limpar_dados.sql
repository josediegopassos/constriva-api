-- ============================================================================
-- CONSTRIVA - Limpar dados de massa de teste
-- Executa na ordem inversa das FKs
-- ============================================================================
SET search_path TO public;

-- Parte 3
DELETE FROM "DocumentosGED" WHERE "Id"::text LIKE 'm2%';
DELETE FROM "PastasDocumentos" WHERE "Id"::text LIKE 'm1%';
DELETE FROM "RegistrosPonto" WHERE "Id"::text LIKE 'h1%';

-- Parte 2
DELETE FROM "LancamentosFinanceiros" WHERE "Id"::text LIKE 'g1%';
DELETE FROM "ItensPedidoCompra" WHERE "Id"::text LIKE 'a4%';
DELETE FROM "PedidosCompra" WHERE "Id"::text LIKE 'a3%';
DELETE FROM "ItensCotacao" WHERE "Id"::text LIKE 'a2%';
DELETE FROM "Cotacoes" WHERE "Id"::text LIKE 'a1%';
DELETE FROM "MedicoesContratuais" WHERE "Id"::text LIKE 'd3%';
DELETE FROM "AditivosContrato" WHERE "Id"::text LIKE 'd2%';
DELETE FROM "Contratos" WHERE "Id"::text LIKE 'd1%';

-- Parte 1
DELETE FROM "Funcionarios" WHERE "Id"::text LIKE 'e1%';
DELETE FROM "FasesObra" WHERE "Id"::text LIKE 'b2%';
DELETE FROM "Obras" WHERE "Id"::text LIKE 'b1%';
DELETE FROM "Fornecedores" WHERE "Id"::text LIKE 'f1%';
DELETE FROM "Clientes" WHERE "Id"::text LIKE 'c1%';
