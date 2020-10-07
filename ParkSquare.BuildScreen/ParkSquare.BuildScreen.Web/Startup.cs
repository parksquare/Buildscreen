using System;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkSquare.BuildScreen.AzureDevOps;
using ParkSquare.BuildScreen.AzureDevOps.Avatar;
using ParkSquare.BuildScreen.AzureDevOps.Build;
using ParkSquare.BuildScreen.AzureDevOps.User;
using ParkSquare.BuildScreen.Core.Avatar;
using ParkSquare.BuildScreen.Core.Build;
using ParkSquare.BuildScreen.Core.Imaging;
using ParkSquare.BuildScreen.Gravatar;
using ParkSquare.ConfigDump.AspNetCore;
using Serilog;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace ParkSquare.BuildScreen.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigDump();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IImageResizer, ImageResizer>();
            services.AddSingleton<IAvatarService, AvatarService>();
            services.AddSingleton<IBuildService, BuildService>();
            services.AddHttpClient();

            // Gravatar avatar provider
            services.AddSingleton<IAvatarProvider, GravatarProvider>();
            services.AddSingleton<ImageFormatManager, ImageFormatManager>(x => GetImageFormatManager());

            // Azure DevOps avatar provider
            services.AddSingleton<IAvatarProvider, AzureDevOpsAvatarProvider>();
            services.AddSingleton<IUserRepository, UserRepository>();

            // Default avatar provider (returns a single transparent pixel)
            services.AddSingleton<IAvatarProvider, TransparentPixelAvatarProvider>();

            // Azure DevOps build provider 
            services.AddSingleton<IBuildProvider, AzureDevOpsBuildProvider>();
            services.AddSingleton<IBuildDtoConverter, BuildDtoConverter>();

            services.AddHttpClient<IAzureDevOpsClient, AzureDevOpsClient>().ConfigurePrimaryHttpMessageHandler(config =>
                new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                });
            
            services.AddSingleton<IBuildFilter, LatestBuildsFilter>();
            services.AddSingleton<IBuildFilter, CompletedPullRequestFilter>();
            services.AddSingleton<ITestResultsProvider, TestResultsProvider>();
            services.AddSingleton<IAzureDevOpsConfig, AzureDevOpsConfig>();
            services.AddSingleton<IBranchNameConverter, BranchNameConverter>();
            services.AddSingleton<IDisplayTransformer, DisplayTransformer>();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static ImageFormatManager GetImageFormatManager()
        {
            var manager = new ImageFormatManager();
            manager.AddImageFormat(JpegFormat.Instance);
            manager.AddImageFormat(PngFormat.Instance);

            return manager;
        }
    }
}
