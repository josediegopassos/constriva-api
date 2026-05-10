namespace Constriva.Domain.Enums;

public enum MotivoFalhaProcessamentoEnum
{
    ConfiancaInsuficiente = 0,
    RespostaNaoEProposta = 1,
    ErroApiOpenAI = 2,
    MidiaInacessivel = 3,
    FormatoNaoSuportado = 4,
    FornecedorNaoIdentificado = 5,
    Desconhecido = 99
}
