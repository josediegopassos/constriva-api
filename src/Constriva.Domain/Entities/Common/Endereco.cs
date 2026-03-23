namespace Constriva.Domain.Entities.Common;

public class Endereco : TenantEntity
{
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
