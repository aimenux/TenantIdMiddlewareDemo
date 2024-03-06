using WebApi.Configuration;
using WebApi.Services;

namespace WebApi;

public class Startup
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SectionName));
    }

    public void Configure(WebApplication app)
    {
        app.UseTenantId();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.DisplayRequestDuration());
        }
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}