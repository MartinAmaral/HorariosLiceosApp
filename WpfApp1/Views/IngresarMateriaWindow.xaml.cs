using BackEnd;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Xps.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class IngresarMateriaWindow : Window
    {
        private bool _mousePressed = false;
        private Profile _semana;
        private Style _cellBorderStyle;
        private List<Border> _borderList = new ();
        private bool yaCreada=false;
        private Clase _clase;
        private List<string> _clasesNames;

        private List<Clase> _clasesToCopyTo;

        public IngresarMateriaWindow(Clase materia,List<string> claseNames , List<Clase> clasesToCopyTo,bool editar=false)
        {
            InitializeComponent();

            yaCreada = editar;
            _clase = materia;
            _clasesNames = claseNames;
            _clasesToCopyTo = clasesToCopyTo;

            nombreMateria.TextChanged += CheckIfCanCreate;
            cantidadSemana.TextChanged += CheckIfCanCreate;
            cantidadMinima.TextChanged += CheckIfCanCreate;
            cantidadMaxima.TextChanged += CheckIfCanCreate;

            xAtrasButton.Click += AtrasButtonClick;
            xCrearOrGuardarButton.Click += XCrearOrGuardarButton_Click;

            if(yaCreada)
            {
                this.Title = "Editar Materia";
                xCrearOrGuardarButton.Content = "Guardar";
                xTitle.Text = "Editar Materia";
            }
            else
            {
                Title = "Ingresar Materia";
            }

            _semana = DataManager.CurrentProfile;

            diasEntreClaseComboBox.Items.Clear();
            for (byte i = 0; i < _semana.daysName.Count; i++)
            {
                diasEntreClaseComboBox.Items.Add(i.ToString());
            };
            cantidadSemana.Text = _clase.cantidadHorasSemana.ToString();
            cantidadMinima.Text = _clase.cantidadMinima.ToString();
            cantidadMaxima.Text = _clase.cantidadMaxima.ToString();
            if(yaCreada)
            {
                diasEntreClaseComboBox.SelectedIndex = _clase.diasEntreClases;
                nombreMateria.Text = _clasesNames[_clase.id-1];

            }
            else diasEntreClaseComboBox.SelectedIndex = 0;


            DrawCalendar();
            
        }

        private void XCrearOrGuardarButton_Click(object sender, RoutedEventArgs e)
        {
            if(yaCreada)
            {
                GuardarMateria();
            }
            else
            {
                CrearMateria();
            }
        }

        private void DrawCalendar()
        {
            _cellBorderStyle = new Style(typeof(Border));
            _cellBorderStyle.Setters.Add(new Setter(BorderBrushProperty, Brushes.Black));
            _cellBorderStyle.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1)));

            MouseUp += ResetBlocks;
            AddHorarios();
            AddDias();
            AddBlocks();
        }

        private void ResetBlocks(object sender, MouseButtonEventArgs e)
        {
            _mousePressed = false;
            for (int i = 0; i < _borderList.Count; i++)
            {
                var x= _borderList[i].Tag as BorderInfo;
                x.HasChanged = false;
            }
        }

        private void AddHorarios()
        {        
            var x = new TextBlock()
            {
                Text = "Horarios"
            };
            AddCellRowToSemana(x,0,0);
            
            
            for (int i = 0; i < _semana.hoursNames.Count; i++)
            {
                x = new TextBlock() { Text = _semana.hoursNames[i]};
                
                AddCellRowToSemana(x,0,i+1);
            }


            calendar.RowDefinitions.Add(new RowDefinition());
        }
        private void AddCellRowToSemana(TextBlock text,int column,int row)
        {
            var border = new Border();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Margin = new Thickness(5,0,5,0);
            border.Style = _cellBorderStyle;
            border.Child = text;
            var rowDef = new RowDefinition() { MaxHeight = 40 };
            
            calendar.RowDefinitions.Add(rowDef);
            calendar.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border,column);
        }

        private void AddDias()
        {
            TextBlock x;
            for (int i = 0; i < _semana.daysName.Count; i++)
            {
                x = new TextBlock() { Text = _semana.daysName[i] };

                AddCellColumnToSemana(x, i+1,  0);
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

            calendar.ColumnDefinitions.Add(rowDef);
            calendar.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
        }

        private void AddBlocks()
        {
            Border border;

            for (byte column = 0; column < _semana.daysName.Count; column++)
            {
                for (byte row = 0; row < _semana.hoursNames.Count; row++)
                {
                    border = new Border();
                    border.Tag = new BorderInfo(column,row,false);
                    border.Style = _cellBorderStyle;
                    border.Background = Brushes.White;
                    if(yaCreada)
                    {
                       foreach (byte block in _clase.horarios[column])
                        {
                            if(block == row)
                            {
                                border.Background = Brushes.Yellow;
                                break;
                            }
                        }
                    }
                    border.AllowDrop = true;
                    border.MouseDown += CalendarMouseDown;
                    border.MouseEnter += Border_MouseEnter;
                    calendar.Children.Add(border);

                    Grid.SetRow(border, row+1);
                    Grid.SetColumn(border,column+1);
                    _borderList.Add(border);
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = (Border)sender;
            var borderInfo = border.Tag as BorderInfo;
            if(_mousePressed && !borderInfo.HasChanged)
            {
                if (border.Background == Brushes.White)
                {
                    border.Background = Brushes.Yellow;
                }
                else
                {
                    border.Background = Brushes.White;
                }
                borderInfo.HasChanged = true;
            }
        }

        private void CalendarMouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            BorderInfo info = border.Tag as BorderInfo;
            _mousePressed = true;
            if ( border.Background == Brushes.White )
            {
                border.Background = Brushes.Yellow;
            }
            else
            {
                border.Background = Brushes.White;
            }
            info.HasChanged = true;
        }

        private void GuardarMateria()
        {
            var horarios = new List<List<byte>>();
            foreach (var b in DataManager.CurrentProfile.daysName)
            {
                horarios.Add(new List<byte>());
            }
            foreach (var border in _borderList)
            {
                var info = border.Tag as BorderInfo;
                if (border.Background == Brushes.Yellow)
                    horarios[info.Column].Add(info.Row);
            }
            foreach (var dia in horarios)
            {
                dia.Order();
            }

            _clasesNames[_clase.id-1] = nombreMateria.Text;
            _clase.cantidadMinima = byte.Parse(cantidadMinima.Text);
            _clase.cantidadMaxima = byte.Parse(cantidadMaxima.Text);
            _clase.diasEntreClases = byte.Parse((string)diasEntreClaseComboBox.SelectedValue);
            _clase.cantidadHorasSemana = byte.Parse(cantidadSemana.Text);
            _clase.horarios = horarios;

            this.Close();
        }

        private void CrearMateria()
        {
            var horarios = new List<List<byte>>();
            foreach (var b in DataManager.CurrentProfile.daysName)
            {
                horarios.Add(new List<byte>());
            }
            foreach (var border in _borderList)
            {
                var info = border.Tag as BorderInfo;
                if ( border.Background == Brushes.Yellow)
                    horarios[info.Column].Add(info.Row);
            }
            foreach(var dia in horarios)
            {
                dia.Order();
            }


            _clase = new Clase((byte)(_semana.materiasNames.Count),byte.Parse(cantidadSemana.Text),
                byte.Parse(cantidadMinima.Text),byte.Parse(cantidadMaxima.Text),
                byte.Parse((string)diasEntreClaseComboBox.SelectedValue),horarios);
            _clasesNames.Add(nombreMateria.Text);
            _clasesToCopyTo.Add(_clase);
            this.Close();
        }

        private void ValidateMateriaName(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            InputValidator.ValidarTexto(textBox, e, 10);
        }

        private void ValidateSemana(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            InputValidator.ValidarNumero(textBox, e, 26, 0);
        }

        private void CheckIfCanCreate(object sender, EventArgs e)
        {
            if(nombreMateria.Text == "" || cantidadMaxima.Text == "" || cantidadMinima.Text == "" || cantidadSemana.Text =="")
            {
                xCrearOrGuardarButton.IsEnabled = false;
            }
            else xCrearOrGuardarButton.IsEnabled = true;

        }

        private void AtrasButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
