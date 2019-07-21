//[assembly: WebActivator.PostApplicationStartMethod(typeof(Erp.BackOffice.App_Start.IoCContainer), "InitializeContainer")]

using Autofac;
using Autofac.Integration.Mvc;
using Erp.BackOffice.Controllers;
using Erp.Domain;
using Erp.Domain.Account;
using Erp.Domain.Account.Repositories;
using Erp.Domain.Crm;
using Erp.Domain.Crm.Repositories;
using Erp.Domain.Interfaces;
//using Erp.Domain.RealEstate;
//using Erp.Domain.RealEstate.Repositories;
using Erp.Domain.Repositories;
using Erp.Domain.Sale;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Repositories;
using qts.webapp.backend.domain.Services.Crm;
using qts.webapp.domain.Infrastructure;
using qts.webapp.domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace Erp.BackOffice.App_Start
{
    public static class IoCContainer
    {
        public static void InitializeContainer()
        {
            // MVC setup documentation here:
            // http://autofac.readthedocs.io/en/latest/integration/mvc.html
            // WCF setup documentation here:
            // http://autofac.readthedocs.io/en/latest/integration/wcf.html
            //
            // First we'll register the MVC/WCF stuff...
            var builder = new ContainerBuilder();

            // MVC - Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(BackOfficeServiceAPIController).Assembly);


            //// MVC - OPTIONAL: Register model binders that require DI.
            //builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            //builder.RegisterModelBinderProvider();

            //// MVC - OPTIONAL: Register web abstractions like HttpContextBase.
            //builder.RegisterModule<AutofacWebTypesModule>();

            //// MVC - OPTIONAL: Enable property injection in view pages.
            //builder.RegisterSource(new ViewRegistrationSource());

            //// MVC - OPTIONAL: Enable property injection into action filters.
            //builder.RegisterFilterProvider();

            // Scan an assembly for components
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                   .Where(t => t.FullName.StartsWith("Erp.Domain.Repositories") == true && t.FullName != "Erp.Domain.Repositories.GenericRepository")                   
                   .AsImplementedInterfaces()
                   .WithParameter("context", new ErpDbContext());

            // Scan an assembly for components (tên class repository nào cũng được)
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                   .Where(t => t.FullName.StartsWith("Erp.Domain.Sale.Repositories") == true && t.FullName != "Erp.Domain.Sale.Repositories.GenericRepository")
                   .AsImplementedInterfaces()
                   .WithParameter("context", new ErpSaleDbContext());

            // Scan an assembly for components (tên class repository nào cũng được)
            builder.RegisterAssemblyTypes(typeof(CampaignRepository).Assembly)
                   .Where(t => t.FullName.StartsWith("Erp.Domain.Crm.Repositories") == true && t.FullName != "Erp.Domain.Crm.Repositories.GenericRepository")
                   .AsImplementedInterfaces()
                   .WithParameter("context", new ErpCrmDbContext());

            // Scan an assembly for components (tên class repository nào cũng được)
            builder.RegisterAssemblyTypes(typeof(CustomerRepository).Assembly)
                   .Where(t => t.FullName.StartsWith("Erp.Domain.Account.Repositories") == true && t.FullName != "Erp.Domain.Account.Repositories.GenericRepository")
                   .AsImplementedInterfaces()
                   .WithParameter("context", new ErpAccountDbContext());

            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(NoteRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();
            // Services
            builder.RegisterAssemblyTypes(typeof(NoteService).Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //using (var ctx = new ErpDbContext())
            //{
            //    InteractiveViews
            //        .SetViewCacheFactory(
            //            ctx,
            //            new FileViewCacheFactory(HttpContext.Current.Server.MapPath("~/") + "MyViews.xml"));
            //}
        }
    }
}