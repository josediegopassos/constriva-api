using Constriva.Domain.Enums;

namespace Constriva.Infrastructure.Integrations.OpenAI.Extrator;

public class ExtratorPropostaException : Exception
{
    public MotivoFalhaProcessamentoEnum Motivo { get; }

    public int? NivelConfiancaObtido { get; }

    public ExtratorPropostaException(
        MotivoFalhaProcessamentoEnum motivo,
        string message,
        int? nivelConfiancaObtido = null)
        : base(message)
    {
        Motivo = motivo;
        NivelConfiancaObtido = nivelConfiancaObtido;
    }

    public ExtratorPropostaException(
        MotivoFalhaProcessamentoEnum motivo,
        string message,
        Exception innerException,
        int? nivelConfiancaObtido = null)
        : base(message, innerException)
    {
        Motivo = motivo;
        NivelConfiancaObtido = nivelConfiancaObtido;
    }
}
