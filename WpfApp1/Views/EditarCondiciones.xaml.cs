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
    public partial class EditarCondiciones : Window
    {
        public EditarCondiciones()
        {
            InitializeComponent();

            if(DataManager.CurrentProfile.conditions.todosLosDiasPrimeraHora)
                xPrimeraHoraCheckBox.IsChecked = true;

            if(DataManager.CurrentProfile.conditions.noEspacioEntreClases)
                xHorasLibresCheckBox.IsChecked = true;


            xAtrasButton.Click += XAtrasButton_Click;
            xGuardarButton.Click += XGuardarButton_Click;
        }

        private void XGuardarButton_Click(object sender, RoutedEventArgs e)
        {
            DataManager.CurrentProfile.conditions.todosLosDiasPrimeraHora = xPrimeraHoraCheckBox.IsChecked.Value;
            DataManager.CurrentProfile.conditions.noEspacioEntreClases = xHorasLibresCheckBox.IsChecked.Value;
            this.Close();
        }

        private void XAtrasButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
