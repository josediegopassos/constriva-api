using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetFuncionarioByIdQuery(Guid Id, Guid EmpresaId) : IRequest<FuncionarioDetalhadoDto?>, ITenantRequest;

public class GetFuncionarioByIdHandler : IRequestHandler<GetFuncionarioByIdQuery, FuncionarioDetalhadoDto?>
{
    private readonly IRHRepository _repo;
    public GetFuncionarioByIdHandler(IRHRepository repo) => _repo = repo;

    public async Task<FuncionarioDetalhadoDto?> Handle(GetFuncionarioByIdQuery r, CancellationToken ct)
    {
        var f = await _repo.GetFuncionarioByIdAsync(r.Id, r.EmpresaId, ct);
        if (f == null || f.IsDeleted) return null;

        var endereco = f.Endereco != null
            ? new EnderecoDto(f.Endereco.Logradouro, f.Endereco.Numero, f.Endereco.Complemento, f.Endereco.Bairro, f.Endereco.Cidade, f.Endereco.Estado, f.Endereco.Cep)
            : null;

        var dadosBancarios = f.DadosBancarios != null
            ? new DadosBancariosDto(f.DadosBancarios.BancoId, f.DadosBancarios.Banco?.Nome, f.DadosBancarios.Agencia, f.DadosBancarios.Conta, f.DadosBancarios.PixChave)
            : null;

        return new FuncionarioDetalhadoDto(
            f.Id, f.Matricula, f.Nome, f.NomeSocial,
            f.DataNascimento, f.Cpf, f.Rg, f.OrgaoExpedidor,
            f.Cnh, f.CategoriaCnh, f.ValidadeCnh,
            f.Ctps, f.SeriCtps, f.Pis,
            f.Email, f.Telefone, f.Celular,
            f.FotoUrl,
            f.Genero, f.EstadoCivil, f.Escolaridade,
            f.Naturalidade, f.Nacionalidade,
            endereco,
            f.TipoContratacaoEnum, f.Status,
            f.CargoId, f.Cargo?.Nome,
            f.DepartamentoId, f.Departamento?.Nome,
            f.ObraAtualId, f.ObraAtual?.Nome,
            f.DataAdmissao, f.DataDemissao, f.MotivoDemissao,
            f.SalarioBase, f.HoraExtra50, f.HoraExtra100,
            f.JornadaDiaria,
            dadosBancarios);
    }
}
