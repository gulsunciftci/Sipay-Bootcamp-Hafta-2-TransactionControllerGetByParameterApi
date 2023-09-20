using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SipayApi.DataAccess.ApplicationDbContext;
using SipayApi.DataAccess.Repository;
using SipayApi.DataAccess.Repository.AccountRepository;
using SipayApi.DataAccess.Repository.CustomerRepository;
using SipayApi.DataAccess.Repository.TransactionRepository;

namespace SipayApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sipay Api Collection", Version = "v1" });
        });


        ////dbcontext
        var dbType = Configuration.GetConnectionString("DbType");
    
        if (dbType == "PostgreSql")
        {
            var dbConfig = Configuration.GetConnectionString("PostgreSqlConnection");
            services.AddDbContext<SimDbContext>(opts =>
            opts.UseNpgsql(dbConfig));

        }
        //else if (dbType == "Default")
        //{
        //    services.AddDbContext<SimDbContext>(options =>
        //    options.UseSqlServer(
        //    Configuration.GetConnectionString("DefaultConnection")));
        //}


        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository,TransactionRepository>();

    }

  
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sipay v1"));
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
