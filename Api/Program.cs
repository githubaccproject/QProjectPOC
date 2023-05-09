using Application.Mapping;
using Application.ServicesRegistration;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
RepositoryRegistration.RegisterRepositories(builder.Services);
AuthenticationRegistration.RegisterAuthentication(builder.Services, builder.Configuration);
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




