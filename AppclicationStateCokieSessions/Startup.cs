using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppclicationStateCokieSessions
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Items.Add("cntMiddleware",1);
                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                int cnt = ((int)context.Items["cntMiddleware"]);
                context.Items["cntMiddleware"] = ++cnt;
                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                int cnt = ((int)context.Items["cntMiddleware"]);
                context.Items["cntMiddleware"] = ++cnt;
                await next.Invoke();
            });
            app.Run(async context =>
            {
                object cnt = 0;
                if (context.Items.ContainsKey("cntMiddleware"))
                    await context.Response.WriteAsync($"here are {context.Items["cntMiddleware"]} Middleware other than this");
                else
                    await context.Response.WriteAsync($"Hello world =)");
            });
        }
    }
}
