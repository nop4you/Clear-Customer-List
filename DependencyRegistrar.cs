using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Widgets.ClearCustomerList.Services;
using Nop.Web.Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Widgets.ClearCustomerList
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //builder.RegisterType<KlarnaPHelper>().As<IKlarnaPHelper>().InstancePerLifetimeScope();
            builder.RegisterType<ClearCustomerService>().As<IClearCustomerService>().InstancePerLifetimeScope();
            
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
