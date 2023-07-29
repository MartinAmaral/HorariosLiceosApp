using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public  class SemanaDecoder
    {

        public List<string> diasNames = new();
        public List<string> materiasNames = new();
        public List<string> horariosNames = new();

        public int spacing = 12;
        public int id = 0;
        public const string path = "Semanas/Test";

        public SemanaDecoder(Profile profile) 
        {
            this.diasNames = profile.daysName;
            materiasNames = profile.materiasNames;
            horariosNames = profile.hoursNames;
        }

        public string SerializeSemana(List<List<byte>> semana)
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
                List<string> x = new() { horariosNames[horario].PadCenter(spacing) };

                for (int dia = 0; dia < diasNames.Count; dia++)
                {
                    x.Add(materiasNames[semana[dia][horario]].PadCenter(spacing));
                }
                result += GetDatos(x);
                result += GetLinea();
            }
            return result;
        }

        private string GetDatos(List<string> datos)
        {
            string result = "|";
            for (int i = 0; i < datos.Count; i++)
            {
                result += datos[i] + "|";
            }
            result += '\n';
            return result;
        }

        private string GetLinea()
        {
            return new string('-', spacing * (diasNames.Count + 1) + diasNames.Count + 2) + '\n';
        }


        public void SaveSemanaAsExcel(string path, List<List<byte>> semana)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                
                SetCell(worksheet.Cell("A1"), "Horarios");

                for (int i = 0;i < horariosNames.Count;i++)
                    SetCell(worksheet.Cell($"A{2 + i}"), horariosNames[i]);
                
                for (int j = 0; j < diasNames.Count; j++)
                    SetCell(worksheet.Cell($"{(char)('B' + j)}1"), diasNames[j]);
                
                for (int c = 0; c < semana.Count; c++)
                {
                    for (int r = 0; r < semana[c].Count; r++)
                        SetCell(worksheet.Cell($"{(char)('B' + c)}{(2 + r)}"), materiasNames[semana[c][r]]);
                }

                for (int c = 0;c < diasNames.Count+1; c++)
                {
                    worksheet.Column($"{(char)('A'+ c)}").AdjustToContents();
                }

                workbook.SaveAs(path);
            }
        }

        private void SetCell(IXLCell cell,string value)
        {
            cell.Value = value;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
    }
}
