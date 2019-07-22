using LDN.Expression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDN.Expression.Core
{
    public class BirdService
    {
        public void Say()
        {
            Func<Bird, bool> temp = x => x.Id > 0;
        }
    }
}
