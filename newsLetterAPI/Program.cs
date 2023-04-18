using Supabase;
using Supabase.Interfaces;
using newsLetterAPI.Contracts;
using newsLetterAPI.Models;
using newsletterAPI.Contracts;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
            builder.Configuration["SupabaseURL"],
            builder.Configuration["SupabaseKey"],
            new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            }));
var app = builder.Build();

app.MapPost("/newsletters",
    async (
        CreateNewsletterRequest request,
         Supabase.Client client) =>
    {
        var newsletter = new Newsletter
        {
            Name = request.Name,
            Description = request.Description,
            ReadTime = request.ReadTime
        };

        var response = await client.From<Newsletter>().Insert(newsletter);

        var newNewsletter = response.Models.First();

        return Results.Ok(newNewsletter.Id);
    });

app.MapGet("/newsletters/{id}", async (long id, Supabase.Client client) =>
{
    var response = await client
        .From<Newsletter>()
        .Where(n => n.Id == id)
        .Get();

    var newsletter = response.Models.FirstOrDefault();

    if (newsletter is null)
    {
        return Results.NotFound();
    }

    var newsletterResponse = new CreateNewsletterResponse
    {
        Id = newsletter.Id,
        Name = newsletter.Name,
        Description = newsletter.Description,
        ReadTime = newsletter.ReadTime,
        CreatedAt = newsletter.CreateedAt
    };

    return Results.Ok(newsletterResponse);
});


app.MapDelete("/newsletters/{id}", async (long id, Supabase.Client client) =>
{
    await client
    .From<Newsletter>()
    .Where(n => n.Id == id)
    .Delete();

    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

