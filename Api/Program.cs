using Application.Mapping;
using Application.Handlers;
using Application.RepositoryRegistration;
using Application.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
AuthenticationRegistration.RegisterAuthentication(builder.Services, builder.Configuration);
RepositoryRegistration.RegisterRepositories(builder.Services);
MediatRRegistration.RegisterMediatRHandlers(builder.Services);
ValidatorRegistration.RegisterValidators(builder.Services);
SwaggerRegistration.RegisterSwagger(builder.Services);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();




