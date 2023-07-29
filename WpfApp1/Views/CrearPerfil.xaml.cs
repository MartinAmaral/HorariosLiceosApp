using BackEnd;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Views
{
    public partial class CrearPerfil : Window
    {
        private Profile _schema;
        private List<string> _daysNames = new();
        private List<string> _hoursNames = new();

        public CrearPerfil()
        {
            InitializeComponent();

            xnombrePerfilTextBox.PreviewTextInput += XnombrePerfilTextBox_TextChanged;
            xnombrePerfilTextBox.TextChanged += XnombrePerfilTextBox_TextChanged1;

            xCantidadDiasButton.Click += XCantidadDiasButton_Click;
            xCantidadHorasButton.Click += XCantidadHorasButton_Click;
            XAtrasButton.Click += XAtrasButton_Click;
            xGuardarButton.Click += XGuardarButton_Click;

            UpdateButtonsLabel(this, new EventArgs());
        }

        private void XnombrePerfilTextBox_TextChanged1(object sender, TextChangedEventArgs e)
        {
            UpdateButtonsLabel(this, new EventArgs());

        }

        private void XnombrePerfilTextBox_TextChanged(object? sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            InputValidator.ValidarTexto(textBox, e, 20);
        }

        private void XCantidadDiasButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddToPerfilWindow(_daysNames, AddToPerfilWindow.AddToPerfilOptions.dias);
            window.Closing += UpdateButtonsLabel;
            window.ShowDialog();
        }

        private void UpdateButtonsLabel(object? sender, EventArgs e)
        {
            if (_daysNames.Count == 0)
            {
                xCantidadDiasButton.Content = "Crear";
            }
            else xCantidadDiasButton.Content = "Editar";

            if (_hoursNames.Count == 0)
            {
                xCantidadHorasButton.Content = "Crear";
            }
            else xCantidadHorasButton.Content = "Editar";

            if (_daysNames.Count == 0 || _hoursNames.Count == 0 || xnombrePerfilTextBox.Text == "")
            {
                xGuardarButton.IsEnabled = false;
            }
            else
            {
                xGuardarButton.IsEnabled = true;
            }
        }

        private void XCantidadHorasButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddToPerfilWindow(_hoursNames, AddToPerfilWindow.AddToPerfilOptions.horarios);
            window.Closing += UpdateButtonsLabel;
            window.ShowDialog();
        }

        private void XAtrasButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SeleccionarPerfilWindow();
            window.Show();
            this.Close();
        }

        private void XGuardarButton_Click(object sender, RoutedEventArgs e)
        {
            var perfil = new Profile(xnombrePerfilTextBox.Text, _daysNames, _hoursNames, new List<string>() { "" },new(),new());
            DataManager.SaveProfile(perfil);
            var window = new SeleccionarPerfilWindow();
            window.Show();
            this.Close();
        }
    }
}