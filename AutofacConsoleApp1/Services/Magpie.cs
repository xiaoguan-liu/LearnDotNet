using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacConsoleApp1.Services
{
    /// <summary>
    /// 喜鹊
    /// </summary>
    public class Magpie : IBird
    {
        public string Say()
        {
            return "我是喜鹊";
        }

        public string Eat()
        {
            return "喜鹊 喜欢吃什么呀？";
        }
    }
}
