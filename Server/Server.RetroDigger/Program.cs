using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ScoreStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var apiV1 = app.MapGroup("/api/v1");

apiV1.MapGet("/scores", async (ScoreStore store, int? limit, CancellationToken cancellationToken) =>
{
    var requestedLimit = limit ?? 5;

    if (requestedLimit <= 0)
    {
        return Results.BadRequest("Parameter 'limit' must be greater than 0.");
    }

    var scores = await store.GetTopScoresAsync(requestedLimit, cancellationToken);
    return Results.Ok(scores);
})
.WithName("GetScores");

apiV1.MapPost("/scores", async (ScoreStore store, CreateScoreRequest request, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.PlayerName))
    {
        return Results.BadRequest("PlayerName is required.");
    }

    if (request.Points < 0)
    {
        return Results.BadRequest("Points must be 0 or higher.");
    }

    var createdScore = await store.AddScoreAsync(request.PlayerName.Trim(), request.Points, cancellationToken);
    return Results.Created($"/api/v1/scores/{createdScore.Id}", createdScore);
})
.WithName("CreateScore");

app.Run();

public record Score(
    Guid Id,
    string PlayerName,
    int Points);

public record CreateScoreRequest(
    string PlayerName,
    int Points);

public sealed class ScoreStore
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
    private readonly string _storePath;

    public ScoreStore(IHostEnvironment environment)
    {
        _storePath = Path.Combine(environment.ContentRootPath, "scores.json");
    }

    public async Task<IReadOnlyList<Score>> GetTopScoresAsync(int limit, CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);

        try
        {
            var scores = await LoadScoresAsync(cancellationToken);

            return [.. scores
                .OrderByDescending(s => s.Points)
                .ThenBy(s => s.PlayerName)
                .Take(limit)];
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<Score> AddScoreAsync(string playerName, int points, CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);

        try
        {
            var scores = await LoadScoresAsync(cancellationToken);

            Score score = new(
                Id: Guid.NewGuid(),
                PlayerName: playerName,
                Points: points);

            scores.Add(score);
            await SaveScoresAsync(scores, cancellationToken);

            return score;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<List<Score>> LoadScoresAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_storePath))
        {
            List<Score> seeded =
            [
                new(Guid.NewGuid(), "Max", 1_000_000_000),
                new(Guid.NewGuid(), "Stephi", 1_000_000_000),
                new(Guid.NewGuid(), "Jannik", 1_000)
            ];

            await SaveScoresAsync(seeded, cancellationToken);
            return seeded;
        }

        await using var stream = File.OpenRead(_storePath);
        var loaded = await JsonSerializer.DeserializeAsync<List<Score>>(stream, _jsonOptions, cancellationToken);

        if (loaded is null)
        {
            return new List<Score>();
        }

        EnsureUniqueIds(loaded);
        return loaded;
    }

    private async Task SaveScoresAsync(List<Score> scores, CancellationToken cancellationToken)
    {
        await using var stream = File.Create(_storePath);
        await JsonSerializer.SerializeAsync(stream, scores, _jsonOptions, cancellationToken);
    }

    private static void EnsureUniqueIds(List<Score> scores)
    {
        HashSet<Guid> seen = [];
        for (var i = 0; i < scores.Count; i++)
        {
            var score = scores[i];
            if (score.Id == Guid.Empty || !seen.Add(score.Id))
            {
                scores[i] = score with { Id = Guid.NewGuid() };
                seen.Add(scores[i].Id);
            }
        }
    }
}
