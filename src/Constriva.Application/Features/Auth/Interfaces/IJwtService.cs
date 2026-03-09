using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constriva.Application.Features.Auth.Interfaces
{
    public interface IJwtService
    {
        (string AccessToken, string RefreshToken, DateTime ExpiresAt) GenerateTokens(Domain.Entities.Tenant.Usuario usuario);
        Guid? ValidateToken(string token);
    }
}
