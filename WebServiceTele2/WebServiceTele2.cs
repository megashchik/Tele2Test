using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;


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

async Task<List<DTO.PersonSmall>> GetPeople(int page = 0, string sex = "Any", int minAge = int.MinValue, int maxAge = int.MaxValue)
{
    DTO.Sex sexValue;
    if (!Enum.TryParse<DTO.Sex>(sex, true, out sexValue))
        return new List<DTO.PersonSmall>();
    var people = await personModel.GetPeople(
        page: page,
        sex: sexValue,
        minAge: minAge,
        maxAge: maxAge);
    return people.Select(n => (DTO.PersonSmall)n).ToList();
}

async Task<DTO.Person> GetPerson(string id)
{
    var person = await personModel.GetPerson(id);
    return person;
}

app.MapGet("/people", GetPeople).WithName("GetPeople");

app.MapGet("/person", GetPerson).WithName("GetPersonById");


app.Run();
