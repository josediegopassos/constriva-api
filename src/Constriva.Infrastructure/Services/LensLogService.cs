using MongoDB.Driver;
using MongoDB.Bson;
using Constriva.Application.Features.Lens.Interfaces;

namespace Constriva.Infrastructure.Services;

public class LensLogService : ILensLogService
{
    private readonly IMongoDatabase? _mongoDb;

    public LensLogService(IMongoDatabase? mongoDb = null)
    {
        _mongoDb = mongoDb;
    }

    public async Task<object?> GetLogByProcessamentoIdAsync(Guid processamentoId, Guid empresaId, CancellationToken ct)
    {
        if (_mongoDb is null)
            return null;

        var collection = _mongoDb.GetCollection<BsonDocument>("log_processamento_lens");

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("processamentoId", processamentoId.ToString()),
            Builders<BsonDocument>.Filter.Eq("empresaId", empresaId.ToString()));

        var logs = await collection
            .Find(filter)
            .Sort(Builders<BsonDocument>.Sort.Descending("timestamp"))
            .ToListAsync(ct);

        if (logs.Count == 0)
            return null;

        return logs.Select(log =>
        {
            log.Remove("_id");
            return BsonTypeMapper.MapToDotNetValue(log);
        }).ToList();
    }
}
