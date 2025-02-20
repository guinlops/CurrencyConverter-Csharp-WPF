using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;




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

        Root val = new Root();
        public class Rate
        {


            public double INR { get; set; }
            public double JPY { get; set; }
            public double USD { get; set; }
            public double NZD { get; set; }
            public double EUR { get; set; }
            public double CAD { get; set; }
            public double ISK { get; set; }
            public double PHP { get; set; }
            public double DKK { get; set; }
            public double CZK { get; set; }

            public double BRL { get; set; }

        }


        public class Root
        {
            //Get all record in rates and set in rate class as currency name wise
            public Rate rates { get; set; }

        }

        public MainWindow()
        {
            InitializeComponent();
            //BindCurrency();
            GetValue();
            //resetInfo();
            
        }

        public static async Task<Root> GetDataGetMethod<T>(string url)
        {
            var ss = new Root { rates = new Rate() }; // Evita NullReferenceException
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseString); // Log do JSON recebido

                        var responseObject = JsonConvert.DeserializeObject<Root>(responseString);
                        return responseObject ?? ss; // Evita retornar null
                    }

                    MessageBox.Show($"Erro na requisição: {response.StatusCode}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return ss;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro na conexão: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Erro ao processar JSON: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ss; // Retorna um objeto vazio em caso de erro
        }

        private async void GetValue()
        {

            val = await GetDataGetMethod<Root>("https://openexchangerates.org/api/latest.json?app_id=c03f291dab7e4086be81fe9585e62a7b");
            //val = await GetDataGetMethod<Root>("<>");

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

            TextBox_Converted.Text = "0,00";
            CurrencyFromIndex.Content = "";
            CurrencyToIndex.Content = "";
        }
        #endregion


        private void cmbFromCurrency_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            if (cmbFromCurrency.SelectedItem is DataRowView selectedRow)
            {
                
               
                CurrencyFromIndex.Content = selectedRow["Text"].ToString();
                CalcConvertedValue();
            }
        }

        private void cmbToCurrency_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbToCurrency.SelectedItem is DataRowView selectedRow)
            {
                CurrencyToIndex.Content = selectedRow["Text"].ToString();
                CalcConvertedValue();
            }

        }

        private void TextBoxFromChanged(object sender, TextChangedEventArgs e)
        {
            CalcConvertedValue();

        }

        private void CalcConvertedValue()
        {
            if (TextBox_Converted != null && TextBoxFrom != null)
            {
                if (!string.IsNullOrEmpty(TextBoxFrom.Text))
                {
                    if (cmbFromCurrency.Text == cmbToCurrency.Text)
                    {

                        double ConvertedValue = double.Parse(TextBoxFrom.Text);

                        TextBox_Converted.Text = ConvertedValue.ToString("N2");

                    }
                    else
                    {
                        //Calculo de conversão                
                        double ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(TextBoxFrom.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());


                        TextBox_Converted.Text = ConvertedValue.ToString("N2");
                    }
                }
            }
        }

        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            resetInfo();
        }

        private void resetInfo() {
            cmbToCurrency.SelectedIndex = 0;
            cmbFromCurrency.SelectedIndex = 0;
            TextBox_Converted.Text = "0,00";
            TextBoxFrom.Text = "0,00";
            CurrencyFromIndex.Content = "";
            CurrencyToIndex.Content = "";
        }

       
    }
}
