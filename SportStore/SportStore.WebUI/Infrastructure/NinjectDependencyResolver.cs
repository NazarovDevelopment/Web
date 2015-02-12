﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;

using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;

using Ninject;

namespace SportStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void AddBindings()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product{Name = "Football",Price = 25},
                new Product{Name = "Surf", Price = 179},
                new Product{Name = "Running shoes", Price = 95}
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }
    }
}