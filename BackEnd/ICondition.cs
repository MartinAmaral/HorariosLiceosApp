using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public interface ICondition
    {
        public bool MetCondition(List<List<byte>> semana);
    }
}
