using BackEnd;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for AddToPerfilWindow.xaml
    /// </summary>
    public partial class AddToPerfilWindow : Window
    {

        private Profile _schema;
        private List<string> _originalList;
        private List<string> _copyList;
        private Stack<BorderItem> _panels = new();
        private Stack<BorderItem> _usedPanels = new();
        private AddToPerfilOptions _id;

        private bool _canLeaveEmpty;
        private List<Clase> _clasesCopies = new();

        public enum AddToPerfilOptions
        {
            horarios,
            dias,
            materias
        }

        public AddToPerfilWindow(List<string> list, AddToPerfilOptions id, bool canLeaveEmpty = true)
        {
            InitializeComponent();

            _originalList = list;
            _copyList = new List<string>(list);

            if (id == AddToPerfilOptions.materias) _copyList.RemoveAt(0);

            _id = id;
            _canLeaveEmpty = canLeaveEmpty;

            xNombreDelDia.PreviewTextInput += XNombreDelDia_PreviewTextInput;

            XAtrasButton.Click += XAtrasButton_Click;
            xGuardarButton.Click += XGuardarButton_Click;


            switch (id)
            {
                case AddToPerfilOptions.horarios:
                    xTitleTextBlock.Text = "Horarios";
                    xIngreseTextBlock.Text = "Ingrese el nombre del horario";
                    xIngresarNombreButton.Click += XIngresarNombreButton_Click;
                    xNombreDelDia.TextChanged += XNombreDelDia_TextChanged;

                    Title = "Editar Horarios";
                    break;
                case AddToPerfilOptions.dias:
                    xTitleTextBlock.Text = "Dias";
                    xIngreseTextBlock.Text = "Ingrese el nombre de los dias";
                    xIngresarNombreButton.Click += XIngresarNombreButton_Click;
                    xNombreDelDia.TextChanged += XNombreDelDia_TextChanged;

                    Title = "Editar Dias";
                    break;
                case AddToPerfilOptions.materias:
                    xTitleTextBlock.Text = "Materias";
                    xIngreseTextBlock.Text = "Ingrese el nombre de la materia";
                    xNombreDelDia.Visibility = Visibility.Collapsed;
                    xIngresarNombreButton.IsEnabled = true;
                    xIngresarNombreButton.Click += XIngresarMateriaButton_Click;
                    Title = "Editar Materias";


                    foreach (var item in DataManager.CurrentProfile.clases)
                    {
                        _clasesCopies.Add(new Clase(item.id, item.cantidadHorasSemana, item.cantidadMinima, item.cantidadMaxima,
                            item.diasEntreClases, item.horarios));
                    }

                    break;
                default:
                    break;
            }
            UpdateScrollView();
        }

        private void XIngresarMateriaButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new IngresarMateriaWindow(new Clase((byte)(_clasesCopies.Count + 1), 0, 0, 0, 0, new List<List<byte>>()),
                _copyList, _clasesCopies);
            window.Closed += Window_Closed;
            window.ShowDialog();
        }

        private void XGuardarButton_Click(object sender, RoutedEventArgs e)
        {
            _originalList.Clear();
            if (_id == AddToPerfilOptions.materias)
            {
                DataManager.CurrentProfile.clases = _clasesCopies;
                _originalList.Add("");
            }
            foreach (var item in _copyList)
            {
                _originalList.Add(item);
            }
            this.Close();
        }

        private void XAtrasButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void XIngresarNombreButton_Click(object sender, RoutedEventArgs e)
        {
            _copyList.Add(xNombreDelDia.Text);
            xNombreDelDia.Text = "";
            UpdateScrollView();
        }

        private void XNombreDelDia_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "")
            {
                xIngresarNombreButton.IsEnabled = false;
            }
            else
            {
                xIngresarNombreButton.IsEnabled = true;
            }
        }

        private void XNombreDelDia_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            InputValidator.ValidarTexto(textBox, e, () => XIngresarNombreButton_Click(this, new RoutedEventArgs()), 14, false);
        }

        private void UpdateScrollView()
        {

            if (_panels.Count < _copyList.Count)
            {
                int times = _copyList.Count - _panels.Count;
                for (int i = 0; i < times; i++)
                {
                    AddPanel();
                }
            }

            if (_copyList.Count > 0)
            {
                xGuardarButton.IsEnabled = true;
            }
            else
            {
                xGuardarButton.IsEnabled = false;
                if (!_canLeaveEmpty)
                {
                    XAtrasButton.IsEnabled = false;
                }
                else XAtrasButton.IsEnabled = true;
            }
        }

        private void AddPanel()
        {
            if (_usedPanels.Count == 0)
            {
                CreatePanel();
            }
            else UsePanel();

        }

        private void CreatePanel()
        {
            int id = _panels.Count;

            Border border = new Border()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black
            };

            Grid grid = new Grid();

            ColumnDefinition colDef1 = new ColumnDefinition() { Width = new GridLength(10) };
            ColumnDefinition colDef2 = new ColumnDefinition() { Width = GridLength.Auto };
            ColumnDefinition colDef3 = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
            ColumnDefinition colDef4 = new ColumnDefinition() { Width = GridLength.Auto };
            ColumnDefinition colDef5 = new ColumnDefinition() { Width = GridLength.Auto };
            ColumnDefinition colDef6 = new ColumnDefinition() { Width = GridLength.Auto };
            ColumnDefinition colDef7 = new ColumnDefinition() { Width = GridLength.Auto };

            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);
            grid.ColumnDefinitions.Add(colDef4);
            grid.ColumnDefinitions.Add(colDef5);
            grid.ColumnDefinitions.Add(colDef6);
            grid.ColumnDefinitions.Add(colDef7);

            RowDefinition rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);

            TextBlock textBlock = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
            };
            grid.Children.Add(textBlock);
            Grid.SetColumn(textBlock, 1);
            Grid.SetRow(textBlock, 0);

            Button editButton = new Button()
            {
                Margin = new Thickness(2),
                MinWidth = 50
            };
            grid.Children.Add(editButton);
            Grid.SetColumn(editButton, 3);
            Grid.SetRow(editButton, 0);

            Button deleteButton = new Button()
            {
                Margin = new Thickness(2),
                MinWidth = 50
            };
            grid.Children.Add(deleteButton);
            Grid.SetColumn(deleteButton, 4);
            Grid.SetRow(deleteButton, 0);

            Button upButton = new Button()
            {
                Margin = new Thickness(2),
                MinWidth = 30
            };
            Grid.SetColumn(upButton, 5);
            Grid.SetRow(upButton, 0);
            grid.Children.Add(upButton);

            Button downButton = new Button()
            {
                Margin = new Thickness(2),
                MinWidth = 30
            };
            grid.Children.Add(downButton);
            Grid.SetColumn(downButton, 6);
            Grid.SetRow(downButton, 0);

            border.Child = grid;

            textBlock.Text = _copyList[id];

            if (_id == AddToPerfilOptions.materias)
            {
                editButton.Content = "Editar";
                editButton.Tag = id;
                editButton.Click += EditButton_Click;
            }
            else editButton.Visibility = Visibility.Collapsed;

            deleteButton.Content = "Borrar";
            deleteButton.Tag = id;
            deleteButton.Click += DeleteButton_Click;

            upButton.Content = "▲";
            upButton.Tag = id;
            upButton.Click += UpButton_Click;

            downButton.Content = "▼";
            downButton.Tag = id;
            downButton.Click += DownButton_Click;
            
            xStackPanel.Children.Add(border);
            _panels.Push(new BorderItem(border, textBlock, editButton, deleteButton, upButton, downButton));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var window = new IngresarMateriaWindow(
                _clasesCopies[(int)button.Tag], _copyList, _clasesCopies, true);
            window.Closed += Window_Closed;
            window.ShowDialog();
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            Reconstruct();
        }

        private void UsePanel()
        {
            var panel = _usedPanels.Pop();

            panel.Border.Visibility = Visibility.Visible;
            int id = _panels.Count;
            panel.TextBlock.Text = _copyList[id];
            panel.EditButton.Tag = id;
            panel.DeleteButton.Tag = id;
            panel.UpButton.Tag = id;
            panel.DownButton.Tag = id;

            _panels.Push(panel);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int id = (int)button.Tag;

            if (IsValidIndex(id, _copyList.Count) && IsValidIndex(id + 1, _copyList.Count))
            {
                string temp = _copyList[id + 1];
                _copyList[id + 1] = _copyList[id];
                _copyList[id] = temp;
            }
            Reconstruct();
        }

        private void Reconstruct()
        {
            var times = _panels.Count;
            for (int i = 0; i < times; i++)
            {
                var x = _panels.Pop();
                x.Border.Visibility = Visibility.Collapsed;
                _usedPanels.Push(x);
            }
            UpdateScrollView();

        }
        private static bool IsValidIndex(int index, int listLength)
        {
            return index >= 0 && index < listLength;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int id = (int)button.Tag;

            if (IsValidIndex(id, _copyList.Count) && IsValidIndex(id - 1, _copyList.Count))
            {
                string temp = _copyList[id - 1];
                _copyList[id - 1] = _copyList[id];
                _copyList[id] = temp;
            }
            Reconstruct();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            int id = (int)button.Tag;
            _copyList.RemoveAt(id);
            if (_id == AddToPerfilOptions.materias)
            {
                _clasesCopies.RemoveAt(id);
            }
            Reconstruct();
        }
    }
}
