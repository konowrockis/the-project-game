using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Settings
{
    public interface IOptions<T> 
        where T: class
    {
        T Value { get; }
    }
}
