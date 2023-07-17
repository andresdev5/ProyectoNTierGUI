using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Core
{
    public static class ContainerProvider
    {
        public static IContainer Container { get; set; }

        // create generic Resolve method
        public static T Resolve<T>()
        {
            var scope = Container.BeginLifetimeScope();
            return scope.Resolve<T>();
        }
    }
}
