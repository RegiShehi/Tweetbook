﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetBook.Data;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}
