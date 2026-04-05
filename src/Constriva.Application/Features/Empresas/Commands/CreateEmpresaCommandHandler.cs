using MediatR;
using BCrypt.Net;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Empresas.DTOs;

namespace Constriva.Application.Features.Empresas.Commands;

public record CreateEmpresaCommand(CreateEmpresaDto Dto) : IRequest<EmpresaDto>;

public class CreateEmpresaCommandHandler : IRequestHandler<CreateEmpresaCommand, EmpresaDto>
{
    private readonly IEmpresaRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;
    private readonly IEmailService _emailService;

    public CreateEmpresaCommandHandler(
        IEmpresaRepository repo, IUsuarioRepository usuarioRepo,
        IUnitOfWork uow, IEmailService emailService)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
        _uow = uow;
        _emailService = emailService;
    }

    public async Task<EmpresaDto> Handle(CreateEmpresaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var cnpjClean = new string(dto.Cnpj.Where(char.IsDigit).ToArray());
        var email = dto.Email.Trim().ToLowerInvariant();
        var emailAdmin = dto.EmailAdmin.Trim().ToLowerInvariant();

        var existing = await _repo.GetByCnpjAsync(cnpjClean, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException($"CNPJ já cadastrado: {dto.Cnpj}");

        // Validar duplicidade de e-mail do administrador (globalmente, pois é credencial de acesso)
        var emailAdminExists = await _usuarioRepo.EmailExistsAsync(emailAdmin, null, cancellationToken);
        if (emailAdminExists)
            throw new InvalidOperationException($"E-mail do administrador já está em uso: {emailAdmin}");

        // Validação de duplicidade por nome (razão social OU nome fantasia já cadastrados)
        var nomeExists = await _repo.ExistsByNomeAsync(dto.RazaoSocial.Trim(), dto.NomeFantasia.Trim(), cancellationToken);
        if (nomeExists)
            throw new InvalidOperationException(
                $"Já existe uma empresa cadastrada com a Razão Social '{dto.RazaoSocial}' ou Nome Fantasia '{dto.NomeFantasia}'.");

        var empresa = new Empresa
        {
            RazaoSocial = dto.RazaoSocial.Trim(),
            NomeFantasia = dto.NomeFantasia.Trim(),
            Cnpj = cnpjClean,
            Email = email,
            Telefone = dto.Telefone.Trim(),
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Cep = dto.Cep,
            Plano = dto.Plano,
            MaxUsuarios = dto.MaxUsuarios,
            MaxObras = dto.MaxObras,
            Status = StatusEmpresaEnum.Ativa
        };

        await _repo.AddAsync(empresa, cancellationToken);

        var adminUsuario = new Usuario
        {
            EmpresaId = empresa.Id,
            Nome = dto.NomeAdmin.Trim(),
            Email = emailAdmin,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.SenhaAdmin),
            Perfil = PerfilUsuarioEnum.AdminEmpresa,
            IsAdminEmpresa = true,
            Ativo = true
        };

        await _usuarioRepo.AddAsync(adminUsuario, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        await _emailService.SendTemplateAsync(emailAdmin, "Bem-vindo ao Constriva",
            new { NomeAdmin = dto.NomeAdmin, RazaoSocial = dto.RazaoSocial }, cancellationToken);

        return new EmpresaDto(
            empresa.Id, empresa.RazaoSocial, empresa.NomeFantasia, empresa.Cnpj,
            empresa.Email, empresa.Telefone, null,
            empresa.Status, empresa.Plano, null,
            empresa.MaxUsuarios, empresa.MaxObras, empresa.Status == StatusEmpresaEnum.Ativa,
            empresa.Cidade, empresa.Estado,
            new ModulosConfigDto(true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false),
            0, 0, empresa.CreatedAt);
    }
}
