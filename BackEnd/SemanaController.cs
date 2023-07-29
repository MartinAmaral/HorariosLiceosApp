using BackEnd.Conditions;

namespace BackEnd
{
    public class SemanaController
    {

        public List<List<byte>> semanaGenerandose = new();
        public HashSet<List<List<byte>>> semanasCorrectas = new();
        public List<ICondition> conditions = new();

        public int maxSemanasDTOs = 1000;

        private byte dias;
        private byte horarios;

        public Action OnSuperoMaximo;

        public SemanaController (int dias, int horarios, FilterConditions condiciones)
        {
            this.dias = (byte)dias;
            this.horarios = (byte)horarios;

            for (var dia = 0; dia <= dias; dia++)       
            {
                var horariosVacios = new List<byte>();

                for (var hor = 0; hor<= horarios; hor++)
                {
                    horariosVacios.Add(0);
                }
                semanaGenerandose.Add(horariosVacios);
            }

            if (condiciones.noEspacioEntreClases)
                conditions.Add(new NoHoraLibreCondition());

            if (condiciones.todosLosDiasPrimeraHora)
                conditions.Add(new PrimeraHoraCondition());
        }

        public void AddToHorarios(byte diaIndex, byte claseId, byte horarioIndex)
        {
            semanaGenerandose[diaIndex][horarioIndex] = claseId;
        }

        public bool HoraLibre(byte diasIndex, byte horarioIndex)
        {
            return semanaGenerandose[diasIndex][horarioIndex] == 0;
        }

        public void RemoveLastClase(byte claseId)
        {
            bool found = false;
            for (int dia = dias; dia >= 0; dia--)
            {
                for (int horario = 0; horario < semanaGenerandose[dia].Count; horario++)
                {
                    if (semanaGenerandose[dia][horario] == claseId)
                    {
                        semanaGenerandose[dia][horario] = 0;
                        found = true;
                    }
                }
                if (found) return;
            }
        }

        public void RemoveAllClase(byte claseId)
        {
            for (int dia = dias; dia >= 0; dia--)
            {
                for (int horario = 0; horario < semanaGenerandose[dia].Count; horario++)
                {
                    if (semanaGenerandose[dia][horario] == claseId)
                        semanaGenerandose[dia][horario] = 0;
                }
            }
        }

        public void SaveSemana()
        {
            if (SemanaCumpleConCondiciones(semanaGenerandose))
            {
                if(semanasCorrectas.Count < maxSemanasDTOs)
                {
                    var x = new List<List<byte>>();
                    foreach (var item in semanaGenerandose)
                    {
                        var y = new List<byte>();
                        foreach (var item2 in item)
                        {
                            y.Add(item2);
                        }
                        x.Add(y);
                    }
                    semanasCorrectas.Add(x);
                }
            }
        }

        private bool SemanaCumpleConCondiciones(List<List<byte>> semana)
        {
            foreach (var condition in conditions)
            {
                if (!condition.MetCondition(semana))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
