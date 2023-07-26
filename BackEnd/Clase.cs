using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public class Clase
    {
        public byte id;
        public byte cantidadHorasSemana;
        public byte cantidadMinima;
        public byte cantidadMaxima;
        public byte diasEntreClases;
        public List<List<byte>> horarios = new List<List<byte>>();

        public Clase(byte id, byte HorasSemanam, byte minima,byte maxima, byte diasEntreClase, List<List<byte>> horarios)
        {
            this.id = id;
            cantidadHorasSemana = HorasSemanam;
            cantidadMinima = minima;
            cantidadMaxima = maxima;
            this.diasEntreClases = diasEntreClase;
            this.horarios = horarios;
        }
    }
}
