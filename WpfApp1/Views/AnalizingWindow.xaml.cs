using BackEnd;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for AnalizingWindow.xaml
    /// </summary>
    public partial class AnalizingWindow : Window
    {
        private CancellationTokenSource token = new();
        private bool hasStopped = false;
        private Analyzer _analyzer;
        private SemanaController _controller;

        public AnalizingWindow()
        {
            InitializeComponent();

            xLoadingBar.Minimum = 0;
            xLoadingBar.Value = 0;
            xLoadingBar.Maximum = DataManager.CurrentProfile.daysName.Count - 1;

            _controller = new SemanaController(DataManager.CurrentProfile.daysName.Count-1,
                DataManager.CurrentProfile.hoursNames.Count-1,DataManager.CurrentProfile.conditions );
            _analyzer = new Analyzer(DataManager.CurrentProfile.clases,_controller);

            xCancelarOkButton.Click += XCancelarOkButton_Click;
            _analyzer.OnCancelled += () =>
            {
                xMessage.Text = "Cancelo";

            };
            Start(_analyzer);
        }

        private void XCancelarOkButton_Click(object sender, RoutedEventArgs e)
        {
            if(hasStopped)
            {
                Close();
            }
            else
            {
                xMessage.Text = "Cancelando...";
                token.Cancel();
            }
        }

        public async void Start(Analyzer analyzer)
        {
            analyzer.OnFinishClass += OnValueChange;
            analyzer.OnFinish += Finished;
            await analyzer.GenerateSemana(token.Token);
        }


        public void OnValueChange(byte value)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                xLoadingBar.Value = value;
            });
        }

        public void Finished()
        {
            DataManager.SetSemanasGeneradas(_controller.semanasCorrectas);

            Application.Current.Dispatcher.Invoke(() =>
            {
                xMessage.Text = "Finalizado";
                hasStopped = true;
                
                xCancelarOkButton.Content = "OK";
            });
        }
    }
}
