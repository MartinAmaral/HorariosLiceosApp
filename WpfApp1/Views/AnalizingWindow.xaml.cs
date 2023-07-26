using BackEnd;
using System.Windows;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for AnalizingWindow.xaml
    /// </summary>
    public partial class AnalizingWindow : Window
    {
        public AnalizingWindow()
        {
            InitializeComponent();

            xLoadingBar.Minimum = 0;
            xLoadingBar.Value = 0;
            xLoadingBar.Maximum = DataManager.currentSchema.daysName.Count - 1;

            SemanaController.Initialize(DataManager.currentSchema.materiasNames,
                DataManager.currentSchema.hoursNames, DataManager.currentSchema.daysName);
            var analyzer = new Analyzer(DataManager.currentSchema.clases, (byte)DataManager.currentSchema.daysName.Count);


            Start(analyzer);
        }

        public async void Start(Analyzer analyzer)
        {
            analyzer.OnFinishClass += OnValueChange;
            analyzer.OnFinish += Finished;
            await analyzer.GenerateSemana();
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                xMessage.Text = "Se termino de Analizar";
            });
        }
    }
}
