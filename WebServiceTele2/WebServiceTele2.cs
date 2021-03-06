using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json.Serialization;
using WebServiceTele2.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

PersonsApi.PeopleModel personModel = new PersonsApi.PeopleModel();

async Task<IResult> GetPeople(int page = 0, string sex = "Any", Age? age = default)
{
    DTO.Sex sexValue;
    if (!Enum.TryParse<DTO.Sex>(sex, true, out sexValue))
        return Results.NotFound();
    var people = await personModel.GetPeople(
        page: page,
        sex: sexValue,
        minAge: age.MinAge,
        maxAge: age.MaxAge);
    var peopleWithoutAge = people.Select(n => (DTO.PersonSmall)n).ToList();
    if(peopleWithoutAge.Count > 0)
        return Results.Ok(peopleWithoutAge);
    else
        return Results.NotFound();
}

async Task<IResult> GetPerson(string id)
{
    try
    {
        var person = await personModel.GetPerson(id);
        return Results.Ok(person);
    }
    catch (InvalidDataException e)
    {
        return Results.NotFound();
    }
}

app.MapGet("/people", GetPeople).WithName("GetPeople");

app.MapGet("/person", GetPerson).WithName("GetPersonById");


app.Run();