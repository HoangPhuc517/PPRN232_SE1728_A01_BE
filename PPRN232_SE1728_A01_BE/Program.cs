using Infrastructure;
using Microsoft.AspNetCore.OData;
using Services.DTOs;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddInfrastructureDI(builder.Configuration);
        builder.Services.Configure<AdminDTO>(builder.Configuration.GetSection("AdminConfig"));
        //Config OData
        builder.Services.AddControllers().AddOData(options =>
        {
			options.AddRouteComponents("odata", ConfigOData.GetEdmModel());
			options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);

		});
		builder.Services.AddCors(options =>
		{
			options.AddPolicy("AllowLocalhost",
				policy =>
				{
					policy.WithOrigins("https://localhost:7065")
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
		});


		builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowLocalhost");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}