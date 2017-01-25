[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(acct.webapi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(acct.webapi.App_Start.NinjectWebCommon), "Stop")]

namespace acct.webapi.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using acct.common.Repository;
    using acct.repository.ef6;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
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
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISalesmanRepo>().To<SalesmanRepo>();
            kernel.Bind<ICustomerRepo>().To<CustomerRepo>();
            kernel.Bind<IGSTRepo>().To<GSTRepo>();
            kernel.Bind<IInvoiceRepo>().To<InvoiceRepo>();
            kernel.Bind<IQuotationRepo>().To<QuotationRepo>();
            kernel.Bind<IExpenseRepo>().To<ExpenseRepo>();
            kernel.Bind<IExpenseCategoryRepo>().To<ExpenseCategoryRepo>();
            kernel.Bind<IOrderDetailRepo>().To<OrderDetailRepo>();
            kernel.Bind<IPaymentRepo>().To<PaymentRepo>();
            kernel.Bind<IPaymentDetailRepo>().To<PaymentDetailRepo>();
            kernel.Bind<IOptionsRepo>().To<OptionsRepo>();
        }        
    }
}
