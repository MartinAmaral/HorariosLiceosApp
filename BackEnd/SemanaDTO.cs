using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public class SemanaDTO
    {
        public List<List<byte>> data = new List<List<byte>>();

        public SemanaDTO(List<List<byte>> a)
        {
            data = a;
        }

    }
}
