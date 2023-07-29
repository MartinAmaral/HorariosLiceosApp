using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Conditions
{
    public class PrimeraHoraCondition : ICondition
    {
        public bool MetCondition(List<List<byte>> semana)
        {
            for (int dia = 0; dia < semana.Count; dia++)
            {
                if (semana[dia][0] == 0)
                    return false;
            }
            return true;
        }
    }
}
