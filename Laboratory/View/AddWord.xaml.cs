using Laboratory.ViewModel;
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
using System.Windows.Shapes;

namespace Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для AddWord.xaml
    /// </summary>
    public partial class AddWord : Window
    {
        public AddWordViewModel Model { get; set; }    

        public AddWord()
        {
            InitializeComponent();
            Model = new AddWordViewModel();
            this.DataContext = Model;

            if (Model.Close == null)
                Model.Close = new Action(this.Close);
        }
    }
}
