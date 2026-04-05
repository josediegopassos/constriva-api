using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.Extensions;

public static class TipoDocumentoLensExtensions
{
    public static string ToLensApiString(this TipoDocumentoLensEnum tipo) => tipo switch
    {
        TipoDocumentoLensEnum.Nfe => "NFE",
        TipoDocumentoLensEnum.Nfse => "NFSE",
        TipoDocumentoLensEnum.CupomFiscal => "CUPOM_FISCAL",
        TipoDocumentoLensEnum.Boleto => "BOLETO",
        TipoDocumentoLensEnum.ComprovantePagamento => "COMPROVANTE_PAGAMENTO",
        TipoDocumentoLensEnum.Recibo => "RECIBO",
        TipoDocumentoLensEnum.Fatura => "FATURA",
        TipoDocumentoLensEnum.Rpa => "RPA",
        TipoDocumentoLensEnum.Cte => "CTE",
        TipoDocumentoLensEnum.BoletimMedicao => "BOLETIM_MEDICAO",
        TipoDocumentoLensEnum.Nfce => "NFCE",
        _ => tipo.ToString().ToUpper()
    };

    public static string ToDescricao(this TipoDocumentoLensEnum tipo) => tipo switch
    {
        TipoDocumentoLensEnum.Nfe => "Nota Fiscal Eletrônica (NF-e)",
        TipoDocumentoLensEnum.Nfse => "Nota Fiscal de Serviço Eletrônica (NFS-e)",
        TipoDocumentoLensEnum.CupomFiscal => "Cupom Fiscal",
        TipoDocumentoLensEnum.Boleto => "Boleto Bancário",
        TipoDocumentoLensEnum.ComprovantePagamento => "Comprovante de Pagamento",
        TipoDocumentoLensEnum.Recibo => "Recibo",
        TipoDocumentoLensEnum.Fatura => "Fatura",
        TipoDocumentoLensEnum.Rpa => "Recibo de Pagamento Autônomo (RPA)",
        TipoDocumentoLensEnum.Cte => "Conhecimento de Transporte Eletrônico (CT-e)",
        TipoDocumentoLensEnum.BoletimMedicao => "Boletim de Medição",
        TipoDocumentoLensEnum.Nfce => "Nota Fiscal de Consumidor Eletrônica (NFC-e)",
        _ => tipo.ToString()
    };
}
