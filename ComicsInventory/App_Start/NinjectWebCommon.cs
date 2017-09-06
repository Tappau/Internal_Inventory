using System;
using System.Web;
using ComicsInventory;
using ComicsInventory.DAL.Repositories.Interfaces;
using ComicsInventory.DAL.Repositories.Inventory;
using ComicsInventory.Services;
using ComicsInventory.Services.BLLInterfaces;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectWebCommon), "Stop")]

namespace ComicsInventory
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //DAL Repository Bindings
            kernel.Bind<IBoxRepo>().To<BoxRepo>();
            kernel.Bind<IIssueConditionRepo>().To<IssueConditionRepo>();
            kernel.Bind<IIssueRepo>().To<IssueRepo>();
            kernel.Bind<IPublisherRepo>().To<PublisherRepo>();
            kernel.Bind<ISeriesRepo>().To<SeriesRepo>();

            //BLL Bindings
            kernel.Bind<IGeneralServices>().To<GeneralServices>();
            kernel.Bind<IIssueService>().To<IssueService>();
            kernel.Bind<IBoxService>().To<BoxService>();
        }
    }
}