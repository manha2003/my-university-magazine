using Microsoft.OpenApi.Models;
using DataAccessLayer.Data;
using System.Reflection;
using BusinessLogicLayer.Validator;
using BusinessLogicLayer.Helpers;
using Swashbuckle.AspNetCore.Filters;
using BusinessLogicLayer.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Repositories.UserRepository;
using BusinessLogicLayer.Services.UserService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Services.AdminService;
using BusinessLogicLayer.Services.FacultyService;
using BusinessLogicLayer.Services.TokenService;
using BusinessLogicLayer.Services.ContributionService;
using BusinessLogicLayer.Services.CommentService;
using BusinessLogicLayer.Services.AcademicTermService;
using System.Text;
using BusinessLogicLayer.Validator;
using DataAccessLayer.Repositories.FacultyRepository;
using DataAccessLayer.Repositories.ContributionRepository;
using DataAccessLayer.Repositories.FileRepository;

using DataAccessLayer.Repositories.AcademicTermRepository;
using Microsoft.AspNetCore.Hosting;
using BusinessLogicLayer.Services;
using PresentationLayer.Service;
using DataAccessLayer.Repositories.CommentRepository;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(UserMapperProfiles));
builder.Services.AddAutoMapper(typeof(FacultyMapperProfiles));
builder.Services.AddAutoMapper(typeof(ContributionMapperProfiles));
builder.Services.AddAutoMapper(typeof(AcademicTermMapperProfiles));
builder.Services.AddAutoMapper(typeof(CommentMapperProfiles));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IContributionRepository, ContributionRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAcademicTermRepository, AcademicTermRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IContributionService, ContributionService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAcademicTermService, AcademicTermService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<ICommentEmailService, CommentEmailService>();
builder.Services.AddTransient<IUserCreatedEmailService, UserCreatedEmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<StatusValidator>();
builder.Services.AddScoped<IRoleValidator,RoleValidator>();
builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddScoped<IAcademicTermValidator, AcademicTermValidator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    }) ;
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:Token").Value!))
    };

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader() 
                   .AllowAnyMethod()
                   .AllowCredentials();

});
    
});

/*builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();*/
builder.Services.AddDbContext<OnlineUniversityMagazineDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineUniversityMagazineConnectionString"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigins");



app.UseAuthorization();


app.MapControllers();

app.Run();
