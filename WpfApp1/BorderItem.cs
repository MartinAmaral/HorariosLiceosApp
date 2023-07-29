using System.Windows.Controls;

namespace WpfApp1
{
    public class BorderItem
    {
        public Border Border { get; set; }
        public TextBlock TextBlock { get; set; }
        public Button EditButton { get; set; }
        public Button DeleteButton { get; set; }
        public Button UpButton { get; set; }
        public Button DownButton { get; set; }

        public BorderItem(Border stackBorder, TextBlock textBlock, Button editButton, Button deleteButton, Button upButton, Button downButton)
        {
            Border = stackBorder;
            TextBlock = textBlock;
            EditButton = editButton;
            DeleteButton = deleteButton;
            UpButton = upButton;
            DownButton = downButton;
        }
    }
}
