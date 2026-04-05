namespace Constriva.Domain.Enums;

/// <summary>
/// Indica qual método foi utilizado para extração dos dados do documento.
/// </summary>
public enum MetodoExtracaoLensEnum
{
    /// <summary>Pipeline OCR local com Tesseract. Custo zero.</summary>
    OCR = 0,

    /// <summary>Fallback via GPT-4o mini vision API. Usado quando OCR tem baixa confiança.</summary>
    VisionAI = 1
}
