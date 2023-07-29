using BackEnd;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for MostrarSemanasWindow.xaml
    /// </summary>
    public partial class MostrarSemanasWindow : Window
    {
        private List<List<List<byte>>> _semanas;
        private List<List<byte>> _currentSemana;
        private int _index = 0;
        private Style _cellBorderStyle;

        private List<List<TextBlock>> _horariosTextBlocks;

        public MostrarSemanasWindow(int index =0)
        {
            InitializeComponent();

            _index = index;
            _semanas = DataManager.CurrentProfile.semanasGuardadas;

            _cellBorderStyle = new Style(typeof(Border));
            _cellBorderStyle.Setters.Add(new Setter(BorderBrushProperty, Brushes.Black));
            _cellBorderStyle.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1)));


            xLeftButton.Click += XLeftButton_Click;
            xRightButton.Click += XRightButton_Click;

            xAtrasButton.Click += XAtrasButton_Click;
            xGuardarButton.Click += XGuardarButton_Click;
            ShowSemana();
        }

        private void XGuardarButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.FileName = "SemanaElegida";

            saveFileDialog.Filter = "Excel Files|*.xlsx|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var dec = new SemanaDecoder(DataManager.CurrentProfile);
                if(System.IO.Path.GetExtension(saveFileDialog.FileName) == ".xlsx")
                {
                    dec.SaveSemanaAsExcel(saveFileDialog.FileName,_currentSemana);
                }
                else
                {
                    File.WriteAllText(saveFileDialog.FileName, dec.SerializeSemana(_currentSemana));
                }
            }

        }

        private void XAtrasButton_Click(object sender, RoutedEventArgs e)
        {
            // maybe return or remember with index was left of?
            this.Close();
        }

        private void XRightButton_Click(object sender, RoutedEventArgs e)
        {
            _index++;
            ShowSemana();
        }

        private void XLeftButton_Click(object sender, RoutedEventArgs e)
        {
            _index --;
            ShowSemana();
        }

        private void ShowSemana()
        {
            _currentSemana = _semanas[_index];

            DrawCalendar();

            CheckIfLimit();
        }


        private void DrawCalendar()
        {   

            if(_horariosTextBlocks == null)
            {
                AddHorarios();
                AddDias();
                AddMaterias();
            }    
            else
            {
                RenameMaterias();
            }
        }
        private void AddHorarios()
        {
            var x = new TextBlock()
            {
                Text = "Horarios",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            AddCellRowToSemana(x, 0, 0);


            for (int i = 0; i < DataManager.CurrentProfile.hoursNames.Count; i++)
            {
                x = new TextBlock() { Text = DataManager.CurrentProfile.hoursNames[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                AddCellRowToSemana(x, 0, i + 1);
            }


            xCalendar.RowDefinitions.Add(new RowDefinition());
        }
        private void AddCellRowToSemana(TextBlock text, int column, int row)
        {
            var border = new Border();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Margin = new Thickness(5, 0, 5, 0);
            border.Style = _cellBorderStyle;
            border.Child = text;
            var rowDef = new RowDefinition() { MaxHeight = 40 };

            xCalendar.RowDefinitions.Add(rowDef);
            xCalendar.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
        }

        private void AddDias()
        {
            TextBlock x;
            for (int i = 0; i < DataManager.CurrentProfile.daysName.Count; i++)
            {
                x = new TextBlock() { Text = DataManager.CurrentProfile.daysName[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };
                AddCellColumnToSemana(x, i + 1, 0);
            }
        }

        private void AddCellColumnToSemana(TextBlock text, int column, int row)
        {
            var border = new Border();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Margin = new Thickness(5, 0, 5, 0);
            border.Style = _cellBorderStyle;
            border.Child = text;
            var rowDef = new ColumnDefinition();

            xCalendar.ColumnDefinitions.Add(rowDef);
            xCalendar.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
        }

        private void AddMaterias()
        {
            Border border;
            _horariosTextBlocks = new();
            for (byte dia = 0; dia < DataManager.CurrentProfile.daysName.Count; dia++)
            {
                _horariosTextBlocks.Add(new());
                for (byte hora = 0; hora < DataManager.CurrentProfile.hoursNames.Count; hora++)
                {
                    border = new Border();
                    border.Style = _cellBorderStyle;

                    TextBlock textBlock = new TextBlock()
                    {
                        Text = DataManager.CurrentProfile.materiasNames[_currentSemana[dia][hora]],
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    _horariosTextBlocks[dia].Add(textBlock);

                    border.Child = textBlock;

                    xCalendar.Children.Add(border);

                    Grid.SetRow(border, hora + 1);
                    Grid.SetColumn(border, dia + 1);
                }
            }
        }

        private void RenameMaterias()
        {
            for (int dia = 0; dia < _horariosTextBlocks.Count; dia++)
            {
                for (int hora = 0; hora < _horariosTextBlocks[dia].Count; hora++)
                {
                    _horariosTextBlocks[dia][hora].Text = DataManager.CurrentProfile.materiasNames[_currentSemana[dia][hora]];
                }
            }
        }

        private void CheckIfLimit()
        {
            xLeftButton.IsEnabled = _index >0;

            if(_index < _semanas.Count-1)
            {
                xRightButton.IsEnabled = true;
            }
            else xRightButton.IsEnabled = false;

        }
    }
}
