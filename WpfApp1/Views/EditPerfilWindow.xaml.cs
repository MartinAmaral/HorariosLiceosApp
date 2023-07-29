using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for EditPerfilWindow.xaml
    /// </summary>
    public partial class EditPerfilWindow : Window
    {
        private Window _window;
        public EditPerfilWindow(Window previousWindow)
        {
            InitializeComponent();

            _window = previousWindow;

            Title = $"Perfil: {DataManager.CurrentProfile.name}";

            xTitulo.Text = $"Perfil: {DataManager.CurrentProfile.name}";

            xEditarMaterias.Click += XEditarMaterias_Click;
            xVerDias.Click += XVerDias_Click;
            xVerHoras.Click += XVerHoras_Click;
            xGenerar.Click += XGenerar_Click;

            xVolver.Click += XVolver_Click;
            xBorrar.Click += XBorrar_Click;
            xGuardar.Click += XGuardar_Click;
            xMostrar.Click += XMostrar_Click;
            xConditionsButton.Click += XConditionsButton_Click;

            xMostrar.IsEnabled = DataManager.CurrentProfile.semanasGuardadas.Count > 0;


            ResetHorariosClases(null,new EventArgs());
        }

        private void XConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditarCondiciones();
            window.ShowDialog();
        }

        private void XMostrar_Click(object sender, RoutedEventArgs e)
        {
            var window = new MostrarSemanasWindow();
            window.ShowDialog();

        }

        private void XGuardar_Click(object sender, RoutedEventArgs e)
        {
            DataManager.SaveCurrentProfile();
            // show a message that the perfil has been saved
        }

        private void XGenerar_Click(object sender, RoutedEventArgs e)
        {
            var window = new AnalizingWindow();
            window.Closing +=(x,y)=> { xMostrar.IsEnabled = DataManager.CurrentProfile.semanasGuardadas.Count > 0; };            ;
            window.ShowDialog();
        }

        public void ResetHorariosClases(object? sender, EventArgs e)
        {
            foreach (var clase in DataManager.CurrentProfile.clases)
            {
                List<List<byte>> replaceHorarios = new List<List<byte>>();
                for (byte dia = 0; dia < DataManager.CurrentProfile.daysName.Count; dia++)
                {
                    List<byte> replaceDia = new List<byte>();

                    if(clase.horarios.Count > dia) {

                        foreach (var item in clase.horarios[dia])
                        {
                            replaceDia.Add(item);
                        }
                    }               
                    replaceHorarios.Add(replaceDia);
                }
                clase.horarios = replaceHorarios;
            }
        }

        private void XEditarMaterias_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddToPerfilWindow(DataManager.CurrentProfile.materiasNames,AddToPerfilWindow.AddToPerfilOptions.materias);
            window.ShowDialog();
        }

        private void XVerHoras_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddToPerfilWindow(DataManager.CurrentProfile.hoursNames, AddToPerfilWindow.AddToPerfilOptions.dias, false);
            window.Closed += ResetHorariosClases;
            window.ShowDialog();
        }

        private void XBorrar_Click(object sender, RoutedEventArgs e)
        {
            DataManager.RemoveProfile(DataManager.CurrentProfile);
            var window = new SeleccionarPerfilWindow();
            window.Show();
            this.Hide();
        }

        private void XVerDias_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddToPerfilWindow(DataManager.CurrentProfile.daysName,AddToPerfilWindow.AddToPerfilOptions.dias,false);
            window.Closed += ResetHorariosClases;
            window.ShowDialog();
        }

        private void XVolver_Click(object sender, RoutedEventArgs e)
        {
            _window.Show();
            // maybe save info??
            this.Close();
        }
    }
}
