using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Domain.Entities.Common;

namespace Constriva.Application.Features.RH.Commands;

public record CreateFuncionarioCommand(Guid EmpresaId, CreateFuncionarioDto Dto)
    : IRequest<FuncionarioDto>, ITenantRequest;

public class CreateFuncionarioCommandHandler : IRequestHandler<CreateFuncionarioCommand, FuncionarioDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateFuncionarioCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FuncionarioDto> Handle(CreateFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var exists = await _repo.CpfExisteAsync(request.EmpresaId, dto.Cpf, cancellationToken);
        if (exists) throw new InvalidOperationException($"Já existe um funcionário com o CPF {dto.Cpf}.");

        var count = await _repo.GetFuncionariosCountAsync(request.EmpresaId, cancellationToken);

        var funcionario = new Funcionario
        {
            EmpresaId = request.EmpresaId,
            Matricula = $"FUNC-{(count + 1):D5}",
            Nome = dto.Nome,
            NomeSocial = dto.NomeSocial,
            Cpf = dto.Cpf,
            Rg = dto.Rg,
            OrgaoExpedidor = dto.OrgaoExpedidor,
            DataNascimento = dto.DataNascimento ?? DateTime.MinValue,
            Cnh = dto.Cnh,
            CategoriaCnh = dto.CategoriaCnh,
            ValidadeCnh = dto.ValidadeCnh,
            Ctps = dto.CtpsNumero,
            SeriCtps = dto.CtpsSerie,
            Pis = dto.Pis,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            FotoUrl = dto.FotoUrl,
            Genero = dto.Genero,
            EstadoCivil = dto.EstadoCivil,
            Escolaridade = dto.Escolaridade,
            Naturalidade = dto.Naturalidade,
            Nacionalidade = dto.Nacionalidade,
            TipoContratacaoEnum = dto.TipoContratacao ?? TipoContratacaoEnum.CLT,
            CargoId = dto.CargoId,
            DepartamentoId = dto.DepartamentoId,
            ObraAtualId = dto.ObraId,
            DataAdmissao = dto.DataAdmissao,
            SalarioBase = dto.SalarioBase,
            HoraExtra50 = dto.HoraExtra50,
            HoraExtra100 = dto.HoraExtra100,
            JornadaDiaria = dto.JornadaDiaria ?? 8,
            Status = dto.Status,
            DadosBancarios = dto.DadosBancarios != null ? new DadosBancarios
            {
                EmpresaId = request.EmpresaId,
                BancoId = dto.DadosBancarios.BancoId,
                Agencia = dto.DadosBancarios.Agencia,
                Conta = dto.DadosBancarios.Conta,
                PixChave = dto.DadosBancarios.PixChave
            } : null,
            Endereco = dto.Endereco != null ? new Endereco
            {
                EmpresaId = request.EmpresaId,
                Logradouro = dto.Endereco.Logradouro,
                Numero = dto.Endereco.Numero,
                Complemento = dto.Endereco.Complemento,
                Bairro = dto.Endereco.Bairro,
                Cidade = dto.Endereco.Cidade,
                Estado = dto.Endereco.Estado,
                Cep = dto.Endereco.Cep
            } : null
        };

        await _repo.AddFuncionarioAsync(funcionario, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new FuncionarioDto(
            funcionario.Id, funcionario.Matricula, funcionario.Nome, funcionario.NomeSocial, funcionario.Cpf, funcionario.Email,
            funcionario.Telefone, funcionario.CargoId, null,
            funcionario.DepartamentoId, null,
            funcionario.ObraAtualId, null,
            funcionario.TipoContratacaoEnum, funcionario.DataAdmissao, funcionario.SalarioBase, funcionario.Status);
    }
}
