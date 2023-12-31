﻿namespace BackEnd
{
    public class Analyzer
    {
        private List<Clase> clasesOriginal;
        private byte daysAmount;
        private bool parar = false;
        private SemanaController _semana;

        public Action<byte> OnFinishClass;
        public Action OnFinish;
        public Action OnCancelled;
        public CancellationToken Token { get; set; }

        public Analyzer(List<Clase> clases, SemanaController semana)
        {
            clasesOriginal = clases;
            _semana = semana;
            this.daysAmount = (byte) clases[0].horarios.Count;
        }


        public async Task GenerateSemana(CancellationToken token)
        {
            this.Token = token;
            token.Register(() => parar= true );

             await Task.Run(() => ScheduleFistClass());
        }

        private void ScheduleFistClass()
        {
             ScheduleClassTrackingHelper(clasesOriginal[0], clasesOriginal[0].cantidadHorasSemana, 0, 0);
        }

        private void ScheduleClass(byte index = 0)
        {
             ScheduleClassHelper(clasesOriginal[index], clasesOriginal[index].cantidadHorasSemana, 0, index);
        }

        private void ScheduleClassTrackingHelper(Clase clase, byte horasRestantesOg, byte diaInicial, byte claseID)
        {
            for (byte diaActual = diaInicial; diaActual < daysAmount; diaActual++)
            {
                OnFinishClass?.Invoke(diaActual);
                var horariosDelDia = clase.horarios[diaActual];
                byte horariosChecked = 0;
                int horariosToCheck = horariosDelDia.Count - clase.cantidadMinima;
                while (horariosChecked <= horariosToCheck)
                {
                    for (byte h = clase.cantidadMinima; h <= Math.Min(horasRestantesOg, clase.cantidadMaxima); h++)
                    {
                        if (Token.IsCancellationRequested)
                        {
                            OnCancelled?.Invoke();
                            return;
                        }
                        if (horasRestantesOg - h < clase.cantidadMinima && horasRestantesOg - h > 0) continue;
                        if (horariosChecked > horariosDelDia.Count - h) continue;
                        int horasRestantes = horasRestantesOg;
                        bool hayEspacio = true;
                        for (int i = 0; i < h; i++)
                        {
                            var horarioChequear = horariosDelDia[horariosChecked + i];
                            if (!_semana.HoraLibre(diaActual, horarioChequear))
                            {
                                hayEspacio = false;
                                break;
                            }
                        }
                        if (hayEspacio)
                        {
                            for (int j = 0; j < h; j++)
                            {
                                _semana.AddToHorarios(diaActual, clase.id, horariosDelDia[horariosChecked + j]);
                                horasRestantes--;
                            }
                            if (horasRestantes == 0)
                            {
                                byte id = (byte)(claseID + 1);
                                if (id < clasesOriginal.Count)
                                    ScheduleClass(id);
                                else
                                    _semana.SaveSemana();
                            }
                            else
                                ScheduleClassHelper(clase, (byte)horasRestantes, (byte)(diaActual + 1 + clase.diasEntreClases), claseID);
                        }
                        if (horasRestantes == 0)
                            _semana.RemoveLastClase(clase.id);
                    }
                    horariosChecked++;
                }
            }
            _semana.RemoveAllClase(clase.id);
            OnFinish?.Invoke();
        }

        private void ScheduleClassHelper(Clase clase, byte horasRestantesOg, byte diaInicial, byte claseID)
        {
            for (byte diaActual = diaInicial; diaActual < daysAmount; diaActual++)
            {
                var horariosDelDia = clase.horarios[diaActual];
                byte horariosChecked = 0;
                int horariosToCheck = horariosDelDia.Count - clase.cantidadMinima;
                while (horariosChecked <= horariosToCheck)
                {
                    for (byte h = clase.cantidadMinima; h <= Math.Min(horasRestantesOg, clase.cantidadMaxima); h++)
                    {
                        if (horasRestantesOg - h < clase.cantidadMinima && horasRestantesOg - h > 0) continue;
                        if (horariosChecked > horariosDelDia.Count - h) continue;
                        int horasRestantes = horasRestantesOg;
                        bool hayEspacio = true;
                        for (int i = 0; i < h; i++)
                        {
                            var horarioChequear = horariosDelDia[horariosChecked + i];
                            if (!_semana.HoraLibre(diaActual, horarioChequear))
                            {
                                hayEspacio = false;
                                break;
                            }
                        }
                        if (hayEspacio)
                        {
                            for (int j = 0; j < h; j++)
                            {
                                _semana.AddToHorarios(diaActual,clase.id, horariosDelDia[horariosChecked + j]);
                                horasRestantes--;
                            }
                            if (horasRestantes == 0)
                            {
                                byte id = (byte) (claseID + 1);
                                if (id < clasesOriginal.Count)
                                    ScheduleClass(id);
                                else
                                    _semana.SaveSemana();
                            }
                            else
                                ScheduleClassHelper(clase,(byte)horasRestantes, (byte)(diaActual + 1 + clase.diasEntreClases), claseID);
                        }
                        if (horasRestantes == 0)
                            _semana.RemoveLastClase(clase.id); 
                    }
                    horariosChecked++;
                }
            }
            _semana.RemoveAllClase(clase.id);
        }
    }
}
