using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.RH;

namespace Constriva.Application.Features.RH.Commands;

public record UpdateFuncionarioCommand(Guid Id, Guid EmpresaId, UpdateFuncionarioDto Dto)
    : IRequest<FuncionarioDto>, ITenantRequest;

public class UpdateFuncionarioCommandHandler : IRequestHandler<UpdateFuncionarioCommand, FuncionarioDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateFuncionarioCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FuncionarioDto> Handle(UpdateFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var f = await _repo.GetFuncionarioByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Funcionário {request.Id} não encontrado.");

        var dto = request.Dto;

        if (dto.Nome != null)                  f.Nome = dto.Nome;
        if (dto.NomeSocial != null)            f.NomeSocial = dto.NomeSocial;
        if (dto.DataNascimento.HasValue)       f.DataNascimento = dto.DataNascimento.Value;
        if (dto.Cpf != null)                   f.Cpf = dto.Cpf;
        if (dto.Rg != null)                    f.Rg = dto.Rg;
        if (dto.OrgaoExpedidor != null)        f.OrgaoExpedidor = dto.OrgaoExpedidor;
        if (dto.Cnh != null)                   f.Cnh = dto.Cnh;
        if (dto.CategoriaCnh != null)          f.CategoriaCnh = dto.CategoriaCnh;
        if (dto.ValidadeCnh.HasValue)          f.ValidadeCnh = dto.ValidadeCnh;
        if (dto.CtpsNumero != null)            f.Ctps = dto.CtpsNumero;
        if (dto.CtpsSerie != null)             f.SeriCtps = dto.CtpsSerie;
        if (dto.Pis != null)                   f.Pis = dto.Pis;
        if (dto.Email != null)                 f.Email = dto.Email;
        if (dto.Telefone != null)              f.Telefone = dto.Telefone;
        if (dto.Celular != null)               f.Celular = dto.Celular;
        if (dto.Genero != null)                f.Genero = dto.Genero;
        if (dto.EstadoCivil != null)           f.EstadoCivil = dto.EstadoCivil;
        if (dto.Escolaridade != null)          f.Escolaridade = dto.Escolaridade;
        if (dto.Naturalidade != null)          f.Naturalidade = dto.Naturalidade;
        if (dto.Nacionalidade != null)         f.Nacionalidade = dto.Nacionalidade;
        if (dto.TipoContratacao.HasValue)      f.TipoContratacaoEnum = dto.TipoContratacao.Value;
        if (dto.CargoId.HasValue)              f.CargoId = dto.CargoId;
        if (dto.DepartamentoId.HasValue)       f.DepartamentoId = dto.DepartamentoId;
        f.ObraAtualId = dto.ObraId;
        if (dto.DataAdmissao.HasValue)         f.DataAdmissao = dto.DataAdmissao.Value;
        f.DataDemissao = dto.DataDemissao;
        f.MotivoDemissao = dto.MotivoDemissao;
        if (dto.SalarioBase.HasValue)          f.SalarioBase = dto.SalarioBase.Value;
        if (dto.HoraExtra50.HasValue)          f.HoraExtra50 = dto.HoraExtra50;
        if (dto.HoraExtra100.HasValue)         f.HoraExtra100 = dto.HoraExtra100;
        if (dto.JornadaDiaria.HasValue)        f.JornadaDiaria = dto.JornadaDiaria.Value;
        if (dto.FotoUrl != null)               f.FotoUrl = dto.FotoUrl;
        if (dto.Status.HasValue)               f.Status = dto.Status.Value;

        if (dto.Endereco != null)
        {
            if (f.Endereco == null)
            {
                var endereco = new Endereco { EmpresaId = request.EmpresaId };
                await _repo.AddEnderecoAsync(endereco, cancellationToken);
                f.EnderecoId = endereco.Id;
                f.Endereco = endereco;
            }
            if (dto.Endereco.Logradouro != null)    f.Endereco.Logradouro = dto.Endereco.Logradouro;
            if (dto.Endereco.Numero != null)        f.Endereco.Numero = dto.Endereco.Numero;
            if (dto.Endereco.Complemento != null)   f.Endereco.Complemento = dto.Endereco.Complemento;
            if (dto.Endereco.Bairro != null)        f.Endereco.Bairro = dto.Endereco.Bairro;
            if (dto.Endereco.Cidade != null)        f.Endereco.Cidade = dto.Endereco.Cidade;
            if (dto.Endereco.Estado != null)        f.Endereco.Estado = dto.Endereco.Estado;
            if (dto.Endereco.Cep != null)           f.Endereco.Cep = dto.Endereco.Cep;
        }

        if (dto.DadosBancarios != null)
        {
            if (f.DadosBancarios == null)
            {
                var dados = new DadosBancarios { EmpresaId = request.EmpresaId };
                await _repo.AddDadosBancariosAsync(dados, cancellationToken);
                f.DadosBancariosId = dados.Id;
                f.DadosBancarios = dados;
            }
            if (dto.DadosBancarios.BancoId.HasValue)      f.DadosBancarios.BancoId = dto.DadosBancarios.BancoId;
            if (dto.DadosBancarios.Agencia != null)      f.DadosBancarios.Agencia = dto.DadosBancarios.Agencia;
            if (dto.DadosBancarios.Conta != null)        f.DadosBancarios.Conta = dto.DadosBancarios.Conta;
            if (dto.DadosBancarios.PixChave != null)     f.DadosBancarios.PixChave = dto.DadosBancarios.PixChave;
        }

        f.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(cancellationToken);

        return new FuncionarioDto(
            f.Id, f.Matricula, f.Nome, f.NomeSocial, f.Cpf, f.Email,
            f.Telefone, f.CargoId, f.Cargo?.Nome,
            f.DepartamentoId, f.Departamento?.Nome,
            f.ObraAtualId, f.ObraAtual?.Nome,
            f.TipoContratacaoEnum, f.DataAdmissao, f.SalarioBase, f.Status);
    }
}
