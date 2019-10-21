using Microsoft.Extensions.DependencyInjection;
using ParkSquare.BuildScreen.Core.Avatar;
using ParkSquare.BuildScreen.Core.AzureDevOps;
using ParkSquare.BuildScreen.Core.Gravatar;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace ParkSquare.BuildScreen.Web
{
    public static class AvatarDependencyRegistration
    {
        public static IServiceCollection AddAvatarDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAvatarService, AvatarService>();

            // Gravatar avatar provider
            services.AddSingleton<IAvatarProvider, GravatarProvider>();
            services.AddSingleton<Core.Gravatar.IHttpClientFactory, Core.Gravatar.HttpClientFactory>();
            services.AddSingleton<ImageFormatManager, ImageFormatManager>(x => GetImageFormatManager());

            // Azure DevOps avatar provider
            services.AddSingleton<IAvatarProvider, AzureDevOpsAvatarProvider>();

            // Default avatar provider (returns a single transparent pixel)
            services.AddSingleton<IAvatarProvider, TransparentPixelAvatarProvider>();

            return services;
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
