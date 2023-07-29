namespace BackEnd.Conditions
{
    public class NoHoraLibreCondition : ICondition
    {
        public bool MetCondition(List<List<byte>> semana)
        {
            for (int dia = 0; dia < semana.Count; dia++)
            {
                bool encontroClase = false;
                bool espacioDespuesClase = false;
                for (int hora = 0; hora < semana[dia].Count; hora++)
                {
                    if (semana[dia][hora] == 0 && encontroClase)
                        espacioDespuesClase = true;
                    else
                    {
                        if (espacioDespuesClase)
                            return false;
                        encontroClase = true;
                    }
                }
            }
            return true;
        }
    }
}
