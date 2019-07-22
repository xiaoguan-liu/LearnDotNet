using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacConsoleApp1.Services
{
    public class Goose : IBird
    {
        public string Say()
        {
            return "我是鹅";
        }
    }
}
