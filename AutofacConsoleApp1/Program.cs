using Autofac;
using AutofacConsoleApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutofacConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainer container;
            var builder = new ContainerBuilder();
            //builder.RegisterType<Goose>().As<IBird>().AsSelf();
            builder.RegisterType<Magpie>().As<IBird>();

            //builder.Register((c, p) => {
            //    return new Goose(p.Named<string>(""))
            //})

            //builder.Register(c => {
            //    var result = new Goose();
            //    var dep = c.Resolve<TheDependency>();
            //    result.SetTheDependency(dep);
            //    return result;
            //});
            //Autofac.Core.IModule
            //Autofac.Features.OwnedInstances.Owned

            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    //.Except<T>(c=>c....)
            //    .AsImplementedInterfaces();

            container = builder.Build();

            //var bird= container.Resolve<IBird>();
            //var bird2 = container.Resolve<Magpie>();
            //var list= container.Resolve<IList<IBird>>();
            var magpie1 = new Magpie();
      
            var bird = container.Resolve<IBird>();
            bird.Say();

            var tb = bird.GetType();
            Console.ReadKey();
        }
    }
}
