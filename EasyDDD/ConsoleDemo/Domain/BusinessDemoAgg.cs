using EasyDDD.Core.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDemo.Domain
{
    /// <summary>
    /// Step:声明业务Aggregate继承AggregateRoot
    /// </summary>
    public class BusinessDemoAgg : AggregateRoot
    {
        public string StringProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public int IntProperty { get; set; }
    }
}
