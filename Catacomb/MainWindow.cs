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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;


namespace Catacomb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Start
    {

        [STAThread]
        static void Main(string[] args)

        {
            Application test = new Application();
            MainWindow window = new MainWindow();
            test.MainWindow = window;
            test.Run();
            

        }
    }
    public partial class MainWindow : Window
    {
        Canvas mainCanvas;

        
        public MainWindow()
        {
            mainCanvas = new Canvas();
            mainCanvas.Background = Brushes.Black;
            
            this.Content = mainCanvas;
            this.Title = "HELLO WORLD";
            this.Show();
            this.MouseDown += test;

        }
        void test(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Message here");
        }
    }
}
