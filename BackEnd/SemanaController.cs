using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace BackEnd
{
    public static class SemanaController
    {
        public static List<string> diasNames = new ();
        public static List<string> materiasNames = new();
        public static List<string> horariosNames = new ();
        
        public static HashSet<string> semanaID = new();

        public static List<List<byte>> semanaGenerandose = new ();

        public static int created;

        public static bool metConditions;
        //public static List<ICondition> conditions = new();
        public static byte spacing = 11;

        public static bool buenaHora;
        public static bool hayEspacio;
        public const string path = "Semanas/Test";

        public static List<byte> combination = new List<byte>();

        public static int maxSemanasDTOs = 1000;
        public static List<SemanaDTO> semanaDTOs = new(maxSemanasDTOs);

        public static void Initialize(List<string> materias, List<string> horarios, List<string> dias )
        {
            materiasNames = materias;
            diasNames = dias;
            horariosNames = horarios;
            semanaGenerandose.Clear();
            foreach ( var dia in dias )
            {
                var x = new List<byte>();

                foreach ( var horario in horarios )
                {
                    x.Add(0);
                }
                semanaGenerandose.Add(x);
            }
            //conditions.Add(new NoEspaciosYBuenaHora());
        }

        public static void AddToHorarios(byte diaIndex, byte claseId, byte horarioIndex)
        {
            semanaGenerandose[diaIndex][horarioIndex] = claseId;
        }

        public static bool HoraLibre(byte diasIndex, byte horarioIndex)
        {
            if (semanaGenerandose[diasIndex][horarioIndex] == 0)
                return true;
            else return false;
        }

        public static void RemoveLastClase(byte claseId)
        {
            bool found = false;
            for (int dia = diasNames.Count - 1; dia >= 0; dia--)
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

        public static void RemoveAllClase(byte claseId)
        {
            for (int dia = diasNames.Count-1; dia >= 0; dia--)
            {
                for (int horario = 0; horario < semanaGenerandose[dia].Count; horario++)
                {
                    if (semanaGenerandose[dia][horario] == claseId)
                        semanaGenerandose[dia][horario] = 0;
                }
            }
        }

        public static void SaveSemana()
        {
            semanaDTOs.Add(new SemanaDTO(semanaGenerandose));
            /*
            var text = SerializeSemana();
            var id = GenerateFileId(text);

            AnaliazarSemana();

            if (metConditions)
            {
                if (semanaID.Add(id))
                {
                    SaveSemana(text, id);
                }
            }*/
        }
        public static string SerializeSemana()
        {
            string result = GetLinea();
            List<string> header = new() { "Horarios".PadCenter(spacing) };
            
            for (int dia = 0; dia < diasNames.Count; dia++)
            {
                header.Add(diasNames[dia].PadCenter(spacing));
            }

            result += GetDatos(header);
            result += GetLinea();

            for (int horario = 0; horario < horariosNames.Count; horario++)
            {
                List<string> x = new() { horariosNames[horario].PadCenter(spacing)};

                for (int dia = 0; dia < diasNames.Count; dia++)
                {
                    x.Add(materiasNames[semanaGenerandose[dia][horario]].PadCenter(spacing));
                }
                result += GetDatos(x);
                result += GetLinea();
            }
            return result;
        }

        private static string GetDatos(List<string> datos)
        {
            string result = "|";
            for (int i = 0; i < datos.Count; i++)
            {
                result += datos[i] + "|";
            }
            result += '\n';
            return result;
        }

        private static string GetLinea()
        {
            return new string('-', spacing * (diasNames.Count+1) + diasNames.Count +2 ) + '\n';
        }

        private static void AnaliazarSemana()
        {
            metConditions = true;
            /*
            foreach (var condition in conditions)
            {
                if (!condition.MetCondition())
                {
                    metConditions = false;
                    return;
                }
            }*/
        }
        public static string GenerateFileId(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                string stringId = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return stringId;
            }
        }
        private static void SaveSemana(string semanaSerializada, string id)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            semanaDTOs.Add(new SemanaDTO(semanaGenerandose));
            string pathFile = $"{path}/Semana_{id}.txt";
            if (!File.Exists(pathFile))
            {
                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(semanaSerializada);
                }
                created++;
            }
        }
    }
}
