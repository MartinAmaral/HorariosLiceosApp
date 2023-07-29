using BackEnd;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    public static class DataManager
    {
        public static List<Profile> profiles = new List<Profile>();

        public static Profile CurrentProfile { get; private set; }

        public const string basePath = "Data";


        public static void SetCurrentProfile(Profile profile)
        {
            CurrentProfile = profile;
        }

        public static void SetSemanasGeneradas(HashSet<List<List<byte>>> data)
        {
            CurrentProfile.semanasGuardadas = new();

            foreach (var combinaciones in data)
            {
                var combinacion = new List<List<byte>>();

                foreach (var dias in combinaciones)
                {
                    var dia = new List<byte>();

                    foreach (var horas in dias)
                    {
                        dia.Add(horas);
                    }
                    combinacion.Add(dias);
                }
                CurrentProfile.semanasGuardadas.Add(combinacion);
            }
            
        }

        public static void RemoveProfile(Profile profileToRemove)
        {
            profiles.Remove(profileToRemove);

            string pathFile = $"{basePath}/Schema{CurrentProfile.name}.txt";
            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
            }
        }

        public static void LoadAllProfiles()
        {
            var file = Directory.GetFiles(basePath,"*txt");
            foreach (var f in file)
            { 
                string text = File.ReadAllText(f);
                profiles.Add(JsonConvert.DeserializeObject<Profile>(text));
            }

        }

        public static void SaveCurrentProfile()
        {
            var text = JsonConvert.SerializeObject(CurrentProfile, Formatting.Indented);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            string pathFile = $"{basePath}/Schema{CurrentProfile.name}.txt";
            if (!File.Exists(pathFile))
            {
                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(text);
                }
            }
            else
            {
                File.Delete(pathFile);

                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(text);
                }
            }
        }

        public static void SaveProfile(Profile profile)
        {
            var text = JsonConvert.SerializeObject(profile, Formatting.Indented);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            string pathFile = $"{basePath}/Schema{profile.name}.txt";
            if (!File.Exists(pathFile))
            {
                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(text);
                }
            }
            else
            {
                File.Delete(pathFile);

                using var writer = new StreamWriter(pathFile);
                {
                    writer.Write(text);
                }
            }
        }
    }
}
