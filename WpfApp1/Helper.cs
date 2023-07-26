using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    public static class Helper
    {
        public static void ValidarNumero(TextBox textBox, TextCompositionEventArgs e , int max=26, int min=0)
        {
            if (!int.TryParse(e.Text, out var value))
            {
                e.Handled = true;
            }
            else
            {
                if (int.TryParse(textBox.Text + e.Text, out var text))
                {
                    if (text > max || text <= min)
                        e.Handled = true;
                }
            }
        }

        public static void ValidarTexto(TextBox textBox, TextCompositionEventArgs e,int max =14 )
        {
            if (e.Text == "\r")
            {
                Keyboard.ClearFocus();
                e.Handled = true;
                return;
            }
            if (textBox != null)
            {
                var incomingText = textBox.Text + e.Text;
                if (incomingText.Length > max)
                {
                    e.Handled = true;
                }
            }
        }

        public static void ValidarTexto(TextBox textBox, TextCompositionEventArgs e, Action action, int max = 14, bool clearFocus = true)
        {
            if (e.Text == "\r")
            {
                if(clearFocus) Keyboard.ClearFocus();
                action.Invoke();
                e.Handled = true;
                return;
            }
            if (textBox != null)
            {
                var incomingText = textBox.Text + e.Text;
                if (incomingText.Length > max)
                {
                    e.Handled = true;
                }
            }
        }

    }
}
