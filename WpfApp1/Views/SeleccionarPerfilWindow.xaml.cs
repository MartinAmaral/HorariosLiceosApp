using BackEnd;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Views;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SelecionarPerfil.xaml
    /// </summary>
    public partial class SeleccionarPerfilWindow : Window
    {
        public SeleccionarPerfilWindow()
        {
            InitializeComponent();

            if (DataManager.schemas.Count == 0)
            {

                DataManager.LoadAllSchemas();

                if (DataManager.schemas.Count == 0)
                {
                    var schema = new Schema("Test", new List<string>() { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes" },
                        new List<string>() { "7:30-8:10", "8:10-8:50", "9:00-9:40", "9:40-10:20", "10:30-11:10", "11:10-10:50" }, new List<string>());

                    schema.clases.Add(new Clase(1, 1, 1, 1, 0, new List<List<byte>>
                { new List<byte>() {1 },
                  new List<byte>(),
                  new List<byte>()}));
                    schema.materiasNames.Add("test1");

                    schema.clases.Add(new Clase(2, 1, 1, 1, 0, new List<List<byte>>
                { new List<byte>(),
                  new List<byte>(){1,2 },
                  new List<byte>() }));
                    schema.materiasNames.Add("test2");

                    DataManager.schemas.Add(schema);
                }
            }



            perfilesBox.SelectedIndex = 0;
            perfilesBox.SelectionChanged += PerfilesBox_SelectionChanged;

            foreach (var file in DataManager.schemas)
            {
                perfilesBox.Items.Add(file.Name);
            }

            selecionarButton.Click += SelecionarButton_Click;

            crearButton.Click += CrearButton_Click;
        }

        private void CrearButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new CrearPerfil();
            window.Show();
            this.Hide();
        }

        private void SelecionarButton_Click(object sender, RoutedEventArgs e)
        {
            DataManager.currentSchema = DataManager.schemas[perfilesBox.SelectedIndex - 1];

            DataManager.SaveSchema();

            var window = new EditPerfilWindow(this);
            window.Show();
            this.Hide();
        }

        private void PerfilesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (perfilesBox.SelectedIndex == 0)
            {
                selecionarButton.IsEnabled = false;
            }
            else selecionarButton.IsEnabled = true;
        }
    }
}
