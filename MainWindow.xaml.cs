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
            ClearControls();
            GetValue();
           
           
        }

        // DataTable object represents tabular data as an in-memory, tabular cache of rows, columns, and constraints.





        //Make sure API return value that name and where you want to store that name are the same. Like in API, get response INR, then set it with INR name.


        /*
        public static async Task<Root> GetDataGetMethod<T>(string url)
        {
            var ss = new Root();
            try
            {
                using (var client = new HttpClient())
                { //Using e disponse alocam e desalocam os objetos. O dispose é feito de forma explícita, Só o using, o dispose é feito de forma implícita.
                    client.Timeout = TimeSpan.FromMinutes(1);   
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ResponseString = await response.Content.ReadAsStringAsync();
                        var ResponseObject = JsonConvert.DeserializeObject<Root>(ResponseString);
                        return ResponseObject;
                    }
                    return ss;
                }//Desaloca o objeto HttpClient;
            }
            catch
            {
                
                MessageBox.Show("GetDataGetMethod com erro.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return ss;
            }
        }*/
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
        //O #region em C# é uma diretiva de compilação que serve para agrupamento de código em regiões que podem ser expandidas ou recolhidas no editor,
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
        }
        #endregion


      

        
        #region Extra Events

        //ClearControls usado para dar um "clear" em todas as caixas escolhidas;
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }


        //Expressao regular para permitir apenas inteiros

        //A propriedade PreviewTextInput no XAML é um evento que captura a entrada de texto antes que ela seja processada pelo controle.
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); 
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        #region Button Click Event

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
        
            double ConvertedValue;

            //Check amount textbox is Null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
               
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus(); //Focus garante que a caixa esteja em foco.
                return;
            }
            //Else if currency From não foi selecionada ou ainda é  --SELECT--
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0 || cmbFromCurrency.Text == "--SELECT--")
            {
                //Then show message box
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbFromCurrency.Focus();
                return;
            }
            //Else if currency To não foi selecionada ou ainda é  --SELECT--
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0 || cmbToCurrency.Text == "--SELECT--")
            {
                //Then show message
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus on To Combobox
                cmbToCurrency.Focus();
                return;
            }

            //If From and To Combobox selecionou o mesmo valor.
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
               
                ConvertedValue = double.Parse(txtCurrency.Text);
              
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N2");

            }
            else
            {
                //Calculo de conversão                
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N2");
            }
        }

      
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            
            ClearControls();
        }
        #endregion
    }
}
