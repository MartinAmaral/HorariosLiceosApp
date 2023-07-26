using BackEnd;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public static class DataManager
    {
        public static List<Schema> schemas = new List<Schema>();

        public static Schema currentSchema;

        public const string basePath = "Data";
        public const string clasesPath = "Clases";
        




        public static List<SemanaDTO> semanasGuardadas = new List<SemanaDTO>();


        public static void CargarSemanasGuardadas()
        {




        }


        public static void LoadAllSchemas()
        {
            var file = Directory.GetFiles(basePath,"*txt");
            foreach (var f in file)
            { 
                string text = File.ReadAllText(f);
                schemas.Add(JsonConvert.DeserializeObject<Schema>(text));
            }

        }

        public static void SaveSchema()
        {
            var text = JsonConvert.SerializeObject(currentSchema, Formatting.Indented);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            string pathFile = $"{basePath}/Schema{currentSchema.Name}.txt";
            if (!File.Exists(pathFile))
            {
                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(text);
                }
            }

        }


        private static void SaveClase(Clase claseToSave)
        {
            var text = JsonConvert.SerializeObject(claseToSave, Formatting.Indented);

            if (!Directory.Exists("Clases"))
            {
                Directory.CreateDirectory("Clases");
            }
            string pathFile = $"Clases/Clase.txt";
            if (!File.Exists(pathFile))
            {
                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(text);
                }
            }
        }
    }
}
