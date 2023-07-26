namespace BackEnd
{
    public class Schema
    {
        public string Name { get; set; }


        public List<string> daysName = new();
        public List<string> hoursNames = new();
        public List<string> materiasNames = new();

        public List<Clase> clases = new();

        public List<List<byte>> hoursPerDays = new();
        public Schema(string name, List<string> daysName,List<string> hoursNames,List<string> materiasNames  )
        {
            Name = name;
            if (materiasNames.Count >=1)
            {
                 var x = materiasNames.FirstOrDefault();
                if (x != "")
                {
                    materiasNames.Insert(0, "");
                }
            }
            else materiasNames.Add("");
            for (byte i = 0; i < daysName.Count; i++)
            {
                hoursPerDays.Add(new List<byte>(hoursNames.Count));
                this.daysName.Add(daysName[i]);
            }
            foreach (var item in hoursNames)
            {
                this.hoursNames.Add(item);
            }
            foreach (var item in materiasNames)
            {
                this.materiasNames.Add(item);
            }

        }


    }
}