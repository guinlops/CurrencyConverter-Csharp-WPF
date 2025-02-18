using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;




namespace CurrencyConverterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    ///Uma classe parcial em C# (partial class) permite dividir a definição de uma classe em vários arquivos. 
    ///=
    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();

        }

        #region Bind Currency From and To Combobox 
        private void BindCurrency()
        {
            //Cria um objeto do tipo datatable;
            DataTable dt = new DataTable();

            //Add display column no datatable
            dt.Columns.Add("Text");

            //Add value column no datatable
            dt.Columns.Add("Rate");

            //Add rows no Datatable com text e value. 
            dt.Rows.Add("--SELECT--", 0);


            //Adiciona o Identificador em texto da Coluna e o valor (rate) respectivo a database;
            /*
            dt.Rows.Add("INR", val.rates.INR);
            dt.Rows.Add("INR", val.rates.INR);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("NZD", val.rates.NZD);
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("CAD", val.rates.CAD);
            dt.Rows.Add("ISK", val.rates.ISK);
            dt.Rows.Add("PHP", val.rates.PHP);
            dt.Rows.Add("DKK", val.rates.DKK);
            dt.Rows.Add("CZK", val.rates.CZK);
            dt.Rows.Add("BRL", val.rates.BRL);
            */

            //Os Dados usados em cmbFromCurrency são do DefaultView do dt;
            cmbFromCurrency.ItemsSource = dt.DefaultView;

            //DisplayMemberPath: Especifica a propriedade do objeto que será exibida na lista.
            //Nesse caso, foi definido que o que será mostrado na comboBox será os valores respectivos a coluna Text do dataTable;
            cmbFromCurrency.DisplayMemberPath = "Text";

            //SelectedValuePath  Define qual valor será de fato usado internamente.
            //Especifica a propriedade que será usada como valor selecionado(SelectedValue).
            //O valor retornado por cmbFromCurrency vai ser o valor respectivo a coluna Rate do datatable;
            cmbFromCurrency.SelectedValuePath = "Rate";

            //SelectedIndex property is used for when bind Combobox it's default selected item is first
            cmbFromCurrency.SelectedIndex = 0;

            //All Property Set For To Currency Combobox As From Currency Combobox
            cmbToCurrency.ItemsSource = dt.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Rate";
            cmbToCurrency.SelectedIndex = 0;
        }
        #endregion


        private void cmbFromCurrency_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
