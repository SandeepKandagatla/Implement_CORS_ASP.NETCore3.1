using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImplementCORS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options => options.AddPolicy("AllowEverything", builder => builder.AllowAnyOrigin()
            //                                                                                   .AllowAnyMethod()
            //                                                                                   .AllowAnyHeader()));
            //or
            //services.AddCors(options => options.AddPolicy("ProjectInternal", builder => builder.WithOrigins("htps://localhost:8080")));
            //or
            //var allowOrigins = Configuration.GetValue<string>("AllowOrigins")?.Split(",") ?? new string[0];
            //services.AddCors(options => options.AddPolicy("ProjectInternal", builder => builder.WithOrigins(allowOrigins).AllowCredentials()));
            /*allow credentials will allow the cross origin with Authentiacation credential.
             AllowCredentials() doesnt work with AllowAnyOrigin().*/
            //or
            //services.AddCors(options => options.AddPolicy("PublicApi", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type")));

            //or
            //cors preflight request
            //var allowOrigins = Configuration.GetValue<string>("AllowOrigins")?.Split(",") ?? new string[0];
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("ProjectInternal", builder => builder.WithOrigins(allowOrigins).AllowCredentials().SetPreflightMaxAge(TimeSpan.FromMinutes(12)));//preflight request cache is 10 min for chrome,24 hrs for firefox.therefore from the second request prefilight request is not made by the browser till 12 mins.after that 2 (preflight,actual) request is made for 1 time
            //    options.AddPolicy("PublicApi", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type"));
            //})
            //or
            //with exposed pagination headers
            //var allowOrigins = Configuration.GetValue<string>("AllowOrigins")?.Split(",") ?? new string[0];
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("ProjectInternal", builder =>
            //    {
            //        builder.WithOrigins(allowOrigins);
            //        builder.WithExposedHeaders("PageNo", "PageCount", "PageSize", "PageTotalRecords");

            //    });

            //    options.AddPolicy("PublicApi", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type"));
            //});
            //or 
            //with subdomains
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("ProjectInternal", builder =>
            //    {
            //        builder.WithOrigins("http://*.ProjectInternal.com");
            //        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
            // });
            //or
            //runtime validation:(validate origin on each request) ::::allow cors only at specific condition
            services.AddCors(options =>
            {
                options.AddPolicy("ProjectInternal", builder =>
                {
                    builder.SetIsOriginAllowed(IsOriginAllowed);
                });
            });




            services.AddControllers();
        }
        public static bool IsOriginAllowed(string host)
        {
            var corsOriginAllowed = new[] { "ProjectInternal" };
            return corsOriginAllowed.Any(origin => host.Contains(origin));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors("AllowEverything");
             //app.UseCors("ProjectInternal");
            
            app.UseHttpsRedirection();

            app.UseRouting();//local cors(PublicApi) is on the controllers

            app.UseCors("ProjectInternal");//then use global cors

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
