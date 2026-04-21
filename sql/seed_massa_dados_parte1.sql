-- ============================================================================
-- CONSTRIVA - Script de Massa de Dados
-- Empresa: Constriva Engenharia Ltda
-- ============================================================================
SET search_path TO public;

-- ============================================================================
-- 1. CLIENTES (5 registros)
-- ============================================================================
INSERT INTO "Clientes" (
    "Id", "EmpresaId", "Codigo", "TipoPessoa", "Nome", "NomeFantasia",
    "Documento", "InscricaoEstadual", "InscricaoMunicipal",
    "Email", "Telefone", "Celular", "Site",
    "Status", "Observacoes", "EnderecoId",
    "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy",
    "IsDeleted", "DeletedAt", "DeletedBy"
) VALUES
-- Cliente 1: Pessoa Juridica - Incorporadora
(
    'c1000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    'CLI-001', 2,
    'Incorporadora Paulista S.A.', 'Incorporadora Paulista',
    '12345678000190', '123456789012', '12345678',
    'contato@incorporadorapaulista.com.br', '(11) 3456-7890', '(11) 99876-5432', 'www.incorporadorapaulista.com.br',
    1, 'Cliente desde 2023. Projetos residenciais de alto padrao.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Cliente 2: Pessoa Juridica - Construtora parceira
(
    'c1000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000001',
    'CLI-002', 2,
    'MRV Engenharia e Participacoes S.A.', 'MRV Engenharia',
    '08684547000157', '987654321098', '87654321',
    'obras.sp@mrv.com.br', '(11) 3222-1100', '(11) 98765-4321', 'www.mrv.com.br',
    1, 'Parceria em empreendimentos populares.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Cliente 3: Pessoa Fisica
(
    'c1000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000001',
    'CLI-003', 1,
    'Carlos Eduardo Mendes da Silva', NULL,
    '52398476801', NULL, NULL,
    'carlos.mendes@gmail.com', '(11) 3045-6789', '(11) 97654-3210', NULL,
    1, 'Proprietario de terreno na Vila Mariana. Obra residencial unifamiliar.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Cliente 4: Pessoa Juridica - Prefeitura
(
    'c1000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000001',
    'CLI-004', 2,
    'Prefeitura Municipal de Sao Paulo', 'PMSP',
    '46395000000139', 'ISENTO', '00000001',
    'licitacoes@prefeitura.sp.gov.br', '(11) 3113-8000', NULL, 'www.prefeitura.sp.gov.br',
    1, 'Contrato via licitacao publica. Obras de infraestrutura viaria.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Cliente 5: Pessoa Fisica
(
    'c1000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000001',
    'CLI-005', 1,
    'Ana Beatriz Ferreira Lopes', NULL,
    '38712564900', NULL, NULL,
    'anabeatriz.lopes@hotmail.com', NULL, '(11) 96543-2109', NULL,
    1, 'Reforma comercial no centro de SP.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
);


-- ============================================================================
-- 2. FORNECEDORES (8 registros)
-- ============================================================================
INSERT INTO "Fornecedores" (
    "Id", "EmpresaId", "Codigo", "TipoPessoaEnum", "RazaoSocial", "NomeFantasia",
    "Documento", "InscricaoEstadual", "Email", "Telefone", "Celular",
    "Site", "Contato", "Tipo", "Ativo", "Homologado",
    "Classificacao", "Prazo", "Observacoes",
    "BancoNome", "BancoAgencia", "BancoConta", "PixChave",
    "EnderecoId",
    "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy",
    "IsDeleted", "DeletedAt", "DeletedBy"
) VALUES
-- Fornecedor 1: Material de construcao
(
    'f1000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    'FOR-001', 2,
    'Votorantim Cimentos S.A.', 'Votorantim Cimentos',
    '01637895000132', '110456789012',
    'vendas@votorantimcimentos.com.br', '(11) 4003-1010', '(11) 99888-7766',
    'www.vfrancal.com.br', 'Roberto Nascimento', 1, true, true,
    'A', 30, 'Fornecedor principal de cimento e concreto.',
    'Banco do Brasil', '1234-5', '56789-0', '01637895000132',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 2: Aco e ferragens
(
    'f1000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000001',
    'FOR-002', 2,
    'Gerdau Acos Longos S.A.', 'Gerdau',
    '07358761000169', '220567890123',
    'comercial.sp@gerdau.com.br', '(11) 3094-6000', '(11) 98777-6655',
    'www.gerdau.com.br', 'Fernanda Costa', 1, true, true,
    'A', 28, 'Fornecedor de vergalhoes, telas e perfis metalicos.',
    'Itau Unibanco', '0567-8', '12345-6', '07358761000169',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 3: Material eletrico
(
    'f1000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000001',
    'FOR-003', 2,
    'Leroy Merlin Cia Brasileira de Bricolagem', 'Leroy Merlin',
    '01438784000105', '330678901234',
    'empresas@leroymerlin.com.br', '(11) 4020-4100', '(11) 97666-5544',
    'www.leroymerlin.com.br', 'Marcos Oliveira', 5, true, true,
    'B', 15, 'Material eletrico, hidraulico e acabamentos em geral.',
    'Bradesco', '1890-2', '67890-1', 'empresas@leroymerlin.com.br',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 4: Servico de terraplenagem
(
    'f1000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000001',
    'FOR-004', 2,
    'Terraplenagem Sao Paulo Ltda', 'Terra SP',
    '23456789000145', '440789012345',
    'orcamento@terrasp.com.br', '(11) 3567-8901', '(11) 96555-4433',
    NULL, 'Joao Pedro Almeida', 2, true, true,
    'B', 7, 'Servicos de terraplenagem, escavacao e transporte de terra.',
    'Caixa Economica', '2345-6', '78901-2', '23456789000145',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 5: Empreiteiro de mao de obra
(
    'f1000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000001',
    'FOR-005', 1,
    'Jose Aparecido dos Santos', 'Ze Santos Empreiteira',
    '45678912300', NULL,
    'ze.santos.empreiteira@gmail.com', '(11) 3678-9012', '(11) 95444-3322',
    NULL, 'Jose Aparecido', 3, true, true,
    'B', 0, 'Empreiteiro de alvenaria e acabamento. Equipe de 20 pessoas.',
    'Banco do Brasil', '3456-7', '89012-3', '45678912300',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 6: Locacao de equipamentos
(
    'f1000000-0000-0000-0000-000000000006',
    '10000000-0000-0000-0000-000000000001',
    'FOR-006', 2,
    'Mills Estruturas e Servicos de Engenharia S.A.', 'Mills',
    '27093558000115', '550890123456',
    'locacao.sp@mills.com.br', '(11) 3789-0123', '(11) 94333-2211',
    'www.mills.com.br', 'Patricia Souza', 4, true, true,
    'A', 30, 'Locacao de guindastes, plataformas e andaimes.',
    'Santander', '4567-8', '90123-4', '27093558000115',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 7: Concreto usinado
(
    'f1000000-0000-0000-0000-000000000007',
    '10000000-0000-0000-0000-000000000001',
    'FOR-007', 2,
    'Concrebras Concreto do Brasil Ltda', 'Concrebras',
    '34567890000156', '660901234567',
    'vendas@concrebras.com.br', '(11) 3890-1234', '(11) 93222-1100',
    'www.concrebras.com.br', 'Luciana Martins', 1, true, true,
    'A', 14, 'Concreto usinado, bombeamento e laboratorio de ensaios.',
    'Itau Unibanco', '5678-9', '01234-5', '34567890000156',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fornecedor 8: Instalacoes hidraulicas (servico + material)
(
    'f1000000-0000-0000-0000-000000000008',
    '10000000-0000-0000-0000-000000000001',
    'FOR-008', 2,
    'Hidrotec Instalacoes e Comercio Ltda', 'Hidrotec',
    '45678901000167', '770012345678',
    'contato@hidrotec.com.br', '(11) 3901-2345', '(11) 92111-0099',
    'www.hidrotec.com.br', 'Andre Ribeiro', 5, true, true,
    'B', 21, 'Servicos e materiais hidraulicos. Inclusive sistemas de incendio.',
    'Bradesco', '6789-0', '12345-6', 'contato@hidrotec.com.br',
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
);


-- ============================================================================
-- 3. OBRAS (3 registros)
-- ============================================================================
INSERT INTO "Obras" (
    "Id", "EmpresaId", "Codigo", "Nome", "Descricao",
    "Tipo", "Status", "TipoContrato",
    "ClienteId", "NomeCliente",
    "ResponsavelTecnico", "CreaResponsavel", "NumeroART", "NumeroRRT",
    "NumeroAlvara", "ValidadeAlvara",
    "AreaTotal", "AreaConstruida", "NumeroAndares", "NumeroUnidades",
    "DataInicioPrevista", "DataFimPrevista", "DataInicioReal", "DataFimReal",
    "ValorContrato", "ValorOrcado", "ValorRealizado", "PercentualConcluido",
    "FotoUrl", "Observacoes", "EnderecoId",
    "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy",
    "IsDeleted", "DeletedAt", "DeletedBy"
) VALUES
-- Obra 1: Residencial Vila Nova (Em Andamento)
(
    'b1000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    'OBR-001', 'Residencial Vila Nova',
    'Condominio residencial com 4 torres de 12 andares, totalizando 192 apartamentos. Padrao medio-alto com areas de lazer completas.',
    1, 3, 1,
    'c1000000-0000-0000-0000-000000000001', 'Incorporadora Paulista S.A.',
    'Eng. Ricardo Almeida Fonseca', 'CREA-SP 5061234567', 'ART-2025-001234', NULL,
    'ALV-2025-0456', '2027-06-30',
    15000.00, 11200.00, 12, 192,
    '2025-03-01', '2027-09-30', '2025-03-15', NULL,
    45000000.00, 42500000.00, 12750000.00, 30.00,
    NULL, 'Obra com financiamento Caixa. Acompanhamento mensal de medicao.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Obra 2: Edificio Comercial Centro (Em Andamento)
(
    'b1000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000001',
    'OBR-002', 'Edificio Comercial Centro',
    'Edificio comercial de 8 andares com 64 salas comerciais, 2 subsolos de garagem e lobby de entrada com pe-direito duplo.',
    2, 3, 2,
    'c1000000-0000-0000-0000-000000000005', 'Ana Beatriz Ferreira Lopes',
    'Eng. Mariana Costa Pereira', 'CREA-SP 5061987654', 'ART-2025-005678', NULL,
    'ALV-2025-0789', '2027-03-31',
    4500.00, 3800.00, 8, 64,
    '2025-06-01', '2027-06-30', '2025-06-10', NULL,
    28000000.00, 26500000.00, 5300000.00, 20.00,
    NULL, 'Projeto com certificacao LEED. Foco em sustentabilidade.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Obra 3: Ponte Sobre Rio Tiete (Orcamento)
(
    'b1000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000001',
    'OBR-003', 'Ponte Sobre Rio Tiete',
    'Ponte estaiada com 450m de extensao sobre o Rio Tiete, ligando as marginais Tiete e Pinheiros. Projeto de infraestrutura viaria municipal.',
    4, 1, 5,
    'c1000000-0000-0000-0000-000000000004', 'Prefeitura Municipal de Sao Paulo',
    'Eng. Paulo Henrique Barbosa', 'CREA-SP 5062345678', NULL, NULL,
    NULL, NULL,
    8500.00, 4200.00, NULL, NULL,
    '2026-01-15', '2028-07-31', NULL, NULL,
    120000000.00, 115000000.00, 0.00, 0.00,
    NULL, 'Aguardando aprovacao de orcamento e liberacao de verba municipal. Licitacao concorrencia publica.', NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
);


-- ============================================================================
-- 4. FASES DE OBRA (14 fases - 5 por obra em andamento, 4 para orcamento)
-- ============================================================================
INSERT INTO "FasesObra" (
    "Id", "EmpresaId", "ObraId", "FasePaiId",
    "Nome", "Descricao", "Ordem",
    "Status", "DataInicioPrevista", "DataFimPrevista",
    "DataInicioReal", "DataFimReal",
    "PercentualConcluido", "ValorPrevisto", "ValorRealizado",
    "ResponsavelId", "Cor",
    "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy",
    "IsDeleted", "DeletedAt", "DeletedBy"
) VALUES
-- ── Obra 1: Residencial Vila Nova ─────────────────────────────────────────
-- Fase 1.1: Fundacao (Concluida)
(
    'b2000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000001', NULL,
    'Fundacao', 'Estacas pre-moldadas, blocos de coroamento e vigas baldrame.', 1,
    3, '2025-03-15', '2025-06-30',
    '2025-03-15', '2025-06-25',
    100.00, 6500000.00, 6320000.00,
    NULL, '#4CAF50',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 1.2: Estrutura (Em Andamento)
(
    'b2000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000001', NULL,
    'Estrutura', 'Pilares, vigas e lajes em concreto armado para as 4 torres.', 2,
    2, '2025-06-01', '2026-04-30',
    '2025-06-10', NULL,
    45.00, 12800000.00, 5760000.00,
    NULL, '#2196F3',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 1.3: Alvenaria (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000001', NULL,
    'Alvenaria', 'Alvenaria de vedacao em blocos ceramicos e divisorias internas.', 3,
    1, '2026-02-01', '2026-10-31',
    NULL, NULL,
    0.00, 8200000.00, 0.00,
    NULL, '#FF9800',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 1.4: Instalacoes (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000001', NULL,
    'Instalacoes', 'Instalacoes eletricas, hidraulicas, gas, SPDA e cabeamento estruturado.', 4,
    1, '2026-05-01', '2027-03-31',
    NULL, NULL,
    0.00, 9500000.00, 0.00,
    NULL, '#9C27B0',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 1.5: Acabamento (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000001', NULL,
    'Acabamento', 'Revestimentos, pisos, pintura, esquadrias e louças.', 5,
    1, '2026-09-01', '2027-08-31',
    NULL, NULL,
    0.00, 5500000.00, 0.00,
    NULL, '#E91E63',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- ── Obra 2: Edificio Comercial Centro ─────────────────────────────────────
-- Fase 2.1: Fundacao (Em Andamento)
(
    'b2000000-0000-0000-0000-000000000006',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000002', NULL,
    'Fundacao', 'Estacas helice continua e bloco sobre estacas. Incluindo 2 subsolos.', 1,
    2, '2025-06-10', '2025-10-31',
    '2025-06-10', NULL,
    60.00, 4200000.00, 2520000.00,
    NULL, '#4CAF50',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 2.2: Estrutura (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000007',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000002', NULL,
    'Estrutura', 'Estrutura metalica com lajes steel deck para os 8 pavimentos.', 2,
    1, '2025-10-01', '2026-06-30',
    NULL, NULL,
    0.00, 7800000.00, 0.00,
    NULL, '#2196F3',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 2.3: Alvenaria e Fechamento (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000008',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000002', NULL,
    'Alvenaria e Fechamento', 'Fechamento em alvenaria e fachada em pele de vidro.', 3,
    1, '2026-04-01', '2026-11-30',
    NULL, NULL,
    0.00, 5500000.00, 0.00,
    NULL, '#FF9800',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 2.4: Instalacoes (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000009',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000002', NULL,
    'Instalacoes', 'Sistemas eletrico, HVAC, hidraulico, elevadores e automacao predial.', 4,
    1, '2026-07-01', '2027-03-31',
    NULL, NULL,
    0.00, 6200000.00, 0.00,
    NULL, '#9C27B0',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 2.5: Acabamento (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000010',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000002', NULL,
    'Acabamento', 'Pisos, forros, pintura, paisagismo e areas comuns.', 5,
    1, '2026-12-01', '2027-05-31',
    NULL, NULL,
    0.00, 2800000.00, 0.00,
    NULL, '#E91E63',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- ── Obra 3: Ponte Sobre Rio Tiete ─────────────────────────────────────────
-- Fase 3.1: Fundacao (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000011',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000003', NULL,
    'Fundacao', 'Tubuloes a ceu aberto e estacas raiz nos apoios da ponte.', 1,
    1, '2026-01-15', '2026-07-31',
    NULL, NULL,
    0.00, 22000000.00, 0.00,
    NULL, '#4CAF50',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 3.2: Infraestrutura e Pilares (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000012',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000003', NULL,
    'Infraestrutura e Pilares', 'Pilares de sustentacao e mastro central estaiado.', 2,
    1, '2026-06-01', '2027-03-31',
    NULL, NULL,
    0.00, 35000000.00, 0.00,
    NULL, '#2196F3',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 3.3: Superestrutura (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000013',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000003', NULL,
    'Superestrutura', 'Tabuleiro em concreto protendido e cabos estaiados.', 3,
    1, '2027-01-01', '2027-12-31',
    NULL, NULL,
    0.00, 40000000.00, 0.00,
    NULL, '#FF9800',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- Fase 3.4: Pavimentacao e Acabamento (Nao Iniciada)
(
    'b2000000-0000-0000-0000-000000000014',
    '10000000-0000-0000-0000-000000000001',
    'b1000000-0000-0000-0000-000000000003', NULL,
    'Pavimentacao e Acabamento', 'Pavimentacao asfaltica, guard-rails, iluminacao e sinalizacao.', 4,
    1, '2027-10-01', '2028-07-31',
    NULL, NULL,
    0.00, 18000000.00, 0.00,
    NULL, '#E91E63',
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
);


-- ============================================================================
-- 5. FUNCIONARIOS (15 registros)
-- ============================================================================
INSERT INTO "Funcionarios" (
    "Id", "EmpresaId", "Matricula", "Nome", "NomeSocial",
    "DataNascimento", "Cpf", "Rg", "OrgaoExpedidor",
    "Cnh", "CategoriaCnh", "ValidadeCnh",
    "Ctps", "SeriCtps", "Pis",
    "Email", "Telefone", "Celular",
    "FotoUrl", "Genero", "EstadoCivil", "Escolaridade",
    "Naturalidade", "Nacionalidade",
    "EnderecoId",
    "TipoContratacaoEnum", "Status",
    "CargoId", "DepartamentoId", "ObraAtualId",
    "DataAdmissao", "DataDemissao", "MotivoDemissao",
    "SalarioBase", "HoraExtra50", "HoraExtra100", "JornadaDiaria",
    "DadosBancariosId",
    "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy",
    "IsDeleted", "DeletedAt", "DeletedBy"
) VALUES
-- 1. Diretor de Engenharia
(
    'e1000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-001', 'Ricardo Almeida Fonseca', NULL,
    '1978-05-12', '12345678901', '23456789-0', 'SSP-SP',
    '98765432100', 'B', '2027-08-15',
    '12345', '0012', '12345678901',
    'ricardo.fonseca@constriva.com.br', '(11) 3456-7001', '(11) 99001-0001',
    NULL, 'Masculino', 'Casado', 'Superior Completo',
    'Sao Paulo - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001', NULL,
    '2023-01-15', NULL, NULL,
    18000.00, 123.75, 247.50, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 2. Gerente de Obras
(
    'e1000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-002', 'Mariana Costa Pereira', NULL,
    '1982-09-23', '23456789012', '34567890-1', 'SSP-SP',
    '87654321099', 'B', '2026-11-20',
    '23456', '0023', '23456789012',
    'mariana.pereira@constriva.com.br', '(11) 3456-7002', '(11) 99002-0002',
    NULL, 'Feminino', 'Solteira', 'Pos-Graduacao',
    'Campinas - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000002',
    '2023-03-01', NULL, NULL,
    15000.00, 103.13, 206.25, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 3. Engenheiro Civil
(
    'e1000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-003', 'Paulo Henrique Barbosa', NULL,
    '1985-03-17', '34567890123', '45678901-2', 'SSP-SP',
    '76543210988', 'AB', '2027-05-10',
    '34567', '0034', '34567890123',
    'paulo.barbosa@constriva.com.br', '(11) 3456-7003', '(11) 99003-0003',
    NULL, 'Masculino', 'Casado', 'Superior Completo',
    'Santos - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 'b1000000-0000-0000-0000-000000000001',
    '2024-02-01', NULL, NULL,
    12000.00, 82.50, 165.00, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 4. Tecnico de Edificacoes
(
    'e1000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-004', 'Fernanda Rodrigues Lima', NULL,
    '1990-11-05', '45678901234', '56789012-3', 'SSP-SP',
    NULL, NULL, NULL,
    '45678', '0045', '45678901234',
    'fernanda.lima@constriva.com.br', '(11) 3456-7004', '(11) 99004-0004',
    NULL, 'Feminino', 'Solteira', 'Tecnico',
    'Guarulhos - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001',
    '2024-06-15', NULL, NULL,
    5500.00, 37.81, 75.63, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 5. Mestre de Obras
(
    'e1000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-005', 'Sebastiao Ferreira dos Santos', NULL,
    '1970-07-28', '56789012345', '67890123-4', 'SSP-SP',
    '65432109877', 'D', '2026-03-22',
    '56789', '0056', '56789012345',
    'sebastiao.santos@constriva.com.br', NULL, '(11) 99005-0005',
    NULL, 'Masculino', 'Casado', 'Ensino Medio',
    'Osasco - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001',
    '2023-05-01', NULL, NULL,
    6500.00, 44.69, 89.38, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 6. Pedreiro
(
    'e1000000-0000-0000-0000-000000000006',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-006', 'Jose Carlos da Silva', NULL,
    '1975-01-14', '67890123456', '78901234-5', 'SSP-SP',
    NULL, NULL, NULL,
    '67890', '0067', '67890123456',
    'jose.silva@constriva.com.br', NULL, '(11) 99006-0006',
    NULL, 'Masculino', 'Casado', 'Ensino Fundamental',
    'Sao Paulo - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001',
    '2024-01-08', NULL, NULL,
    2800.00, 19.25, 38.50, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 7. Pedreiro
(
    'e1000000-0000-0000-0000-000000000007',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-007', 'Antonio Marcos Oliveira', NULL,
    '1980-04-02', '78901234567', '89012345-6', 'SSP-SP',
    NULL, NULL, NULL,
    '78901', '0078', '78901234567',
    'antonio.oliveira@constriva.com.br', NULL, '(11) 99007-0007',
    NULL, 'Masculino', 'Casado', 'Ensino Fundamental',
    'Diadema - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001',
    '2024-01-08', NULL, NULL,
    2500.00, 17.19, 34.38, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 8. Servente
(
    'e1000000-0000-0000-0000-000000000008',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-008', 'Francisco Souza Nascimento', NULL,
    '1992-08-19', '89012345678', '90123456-7', 'SSP-SP',
    NULL, NULL, NULL,
    '89012', '0089', '89012345678',
    'francisco.nascimento@constriva.com.br', NULL, '(11) 99008-0008',
    NULL, 'Masculino', 'Solteiro', 'Ensino Fundamental',
    'Maua - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001',
    '2024-03-01', NULL, NULL,
    1800.00, 12.38, 24.75, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 9. Armador
(
    'e1000000-0000-0000-0000-000000000009',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-009', 'Luiz Fernando Ribeiro', NULL,
    '1983-12-30', '90123456789', '01234567-8', 'SSP-SP',
    NULL, NULL, NULL,
    '90123', '0090', '90123456789',
    'luiz.ribeiro@constriva.com.br', NULL, '(11) 99009-0009',
    NULL, 'Masculino', 'Casado', 'Ensino Medio',
    'Santo Andre - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000001',
    '2024-04-15', NULL, NULL,
    3200.00, 22.00, 44.00, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 10. Eletricista
(
    'e1000000-0000-0000-0000-000000000010',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-010', 'Marcos Vinicius Alves', NULL,
    '1988-06-08', '01234567890', '12345678-9', 'SSP-SP',
    '54321098766', 'B', '2027-01-18',
    '01234', '0001', '01234567890',
    'marcos.alves@constriva.com.br', NULL, '(11) 99010-0010',
    NULL, 'Masculino', 'Solteiro', 'Tecnico',
    'Sao Bernardo do Campo - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000002',
    '2024-07-01', NULL, NULL,
    3800.00, 26.13, 52.25, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 11. Encanador
(
    'e1000000-0000-0000-0000-000000000011',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-011', 'Reginaldo Pereira da Cruz', NULL,
    '1979-10-15', '11234567890', '22345678-0', 'SSP-SP',
    NULL, NULL, NULL,
    '11234', '0112', '11234567890',
    'reginaldo.cruz@constriva.com.br', NULL, '(11) 99011-0011',
    NULL, 'Masculino', 'Casado', 'Ensino Medio',
    'Sao Paulo - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000002',
    '2024-08-01', NULL, NULL,
    3500.00, 24.06, 48.13, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 12. Almoxarife
(
    'e1000000-0000-0000-0000-000000000012',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-012', 'Claudia Regina Santos', NULL,
    '1987-02-25', '22345678901', '33456789-1', 'SSP-SP',
    NULL, NULL, NULL,
    '22345', '0223', '22345678901',
    'claudia.santos@constriva.com.br', '(11) 3456-7012', '(11) 99012-0012',
    NULL, 'Feminino', 'Divorciada', 'Ensino Medio',
    'Taboao da Serra - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000004', NULL,
    '2024-02-15', NULL, NULL,
    2800.00, 19.25, 38.50, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 13. Tecnico de Seguranca do Trabalho
(
    'e1000000-0000-0000-0000-000000000013',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-013', 'Roberto Nascimento Gomes', NULL,
    '1984-07-11', '33456789012', '44567890-2', 'SSP-SP',
    '43210987655', 'B', '2026-09-05',
    '33456', '0334', '33456789012',
    'roberto.gomes@constriva.com.br', '(11) 3456-7013', '(11) 99013-0013',
    NULL, 'Masculino', 'Casado', 'Tecnico',
    'Barueri - SP', 'Brasileira',
    NULL,
    1, 1,
    '80000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000008', NULL,
    '2023-08-01', NULL, NULL,
    4500.00, 30.94, 61.88, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 14. Mestre de Obras (PJ)
(
    'e1000000-0000-0000-0000-000000000014',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-014', 'Edson Luiz de Moraes', NULL,
    '1972-03-09', '44567890123', '55678901-3', 'SSP-SP',
    '32109876544', 'D', '2026-06-14',
    NULL, NULL, NULL,
    'edson.moraes@constriva.com.br', NULL, '(11) 99014-0014',
    NULL, 'Masculino', 'Casado', 'Ensino Medio',
    'Itaquaquecetuba - SP', 'Brasileira',
    NULL,
    2, 1,
    '80000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', 'b1000000-0000-0000-0000-000000000002',
    '2025-05-01', NULL, NULL,
    7500.00, NULL, NULL, 8,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
),
-- 15. Engenheiro Civil (Autonomo)
(
    'e1000000-0000-0000-0000-000000000015',
    '10000000-0000-0000-0000-000000000001',
    'FUNC-015', 'Juliana Mendes Cardoso', NULL,
    '1989-11-22', '55678901234', '66789012-4', 'SSP-SP',
    '21098765433', 'B', '2027-02-28',
    NULL, NULL, NULL,
    'juliana.cardoso@constriva.com.br', '(11) 3456-7015', '(11) 99015-0015',
    NULL, 'Feminino', 'Solteira', 'Pos-Graduacao',
    'Sorocaba - SP', 'Brasileira',
    NULL,
    3, 1,
    '80000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000007', NULL,
    '2025-09-01', NULL, NULL,
    10000.00, NULL, NULL, 6,
    NULL,
    NOW(), NOW(), '20000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002',
    false, NULL, '00000000-0000-0000-0000-000000000000'
);
