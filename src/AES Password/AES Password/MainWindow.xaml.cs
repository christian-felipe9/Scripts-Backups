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

namespace AES_Password
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCriptografar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Chave.Text))
                    Resultado.Text = Security.Encrypt(Senha.Text, Chave.Text);
                else
                    MessageBox.Show("Chave não pode estar vazia!", "Chave vazia.");
            }
            catch
            {
                MessageBox.Show("Chave não é valida!", "Chave vazia.");
            }
        }

        private void btnDescriptografar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Chave.Text))
                    Resultado.Text = Security.Decrypt(Senha.Text, Chave.Text);
                else
                    MessageBox.Show("Chave não pode estar vazia!", "Chave vazia.");
            }
            catch
            {
                MessageBox.Show("Chave não é valida!", "Chave vazia.");
            }
        }
    }
}
