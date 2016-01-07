using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDDD.Core.IdGenerator
{
    public class IdentityGeneratorFactory
    {
        private static IIdentityGenerator _identityGenerator;
        private static readonly object _lockObject = new object();

        public static IIdentityGenerator Instance
        {
            get
            {
                if (_identityGenerator == null)
                {
                    lock (_lockObject)
                    {
                        _identityGenerator = IoC.Resolve<IIdentityGenerator>();
                    }
                }
                return _identityGenerator;
            }
        }
    }
}
