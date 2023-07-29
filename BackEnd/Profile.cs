namespace BackEnd
{
    public class Profile
    {
        public string name;

        public List<string> daysName = new();
        public List<string> hoursNames = new();
        public List<string> materiasNames = new();

        public List<Clase> clases = new();

        public List<List<byte>> hoursPerDays = new();

        public List<List<List<byte>>> semanasGuardadas;
        
        public FilterConditions conditions = new FilterConditions();

        public Profile(string name, List<string> daysName,List<string> hoursNames,List<string> materiasNames,
            List<List<List<byte>>> semanasGuardadas, FilterConditions conditions )
        {
            this.name = name;
            if (materiasNames.Count >0)
            {
                 var primeraMateria = materiasNames.FirstOrDefault();
                if (primeraMateria != "")
                {
                    materiasNames.Insert(0, "");
                }
            }
            else materiasNames.Add("");

            this.semanasGuardadas = semanasGuardadas;

            for (byte i = 0; i < daysName.Count; i++)
            {
                hoursPerDays.Add(new List<byte>(hoursNames.Count));
                this.daysName.Add(daysName[i]);
            }
            foreach (var hours in hoursNames)
            {
                this.hoursNames.Add(hours);
            }
            foreach (var materia in materiasNames)
            {
                this.materiasNames.Add(materia);
            }

            this.conditions = conditions;
        }
    }
}