# Currency Converter WPF

## Descrição
O **Currency Converter WPF** é um aplicativo desenvolvido em **C# e WPF (Windows Presentation Foundation)** que permite converter moedas utilizando taxas de câmbio em tempo real. O projeto faz requisições HTTP para obter os valores das taxas de um serviço de API externa e exibe os resultados de maneira intuitiva.

## Tecnologias Utilizadas
- **C#** (Linguagem de Programação)
- **WPF (Windows Presentation Foundation)**
- **HttpClient** para requisição HTTP
- **JSON.NET (Newtonsoft.Json)** para manipulação de JSON
- **DataTable** para gerenciamento de dados das taxas de câmbio

## Funcionalidades
- Conversão de moedas em tempo real
- Seleção de moeda de origem e destino através de combobox
- Atualização automática das taxas de câmbio ao iniciar o programa
- Possibilidade de redefinir os valores para a configuração padrão

## Instalação e Execução
1. Clone este repositório:
   ```sh
   git clone https://github.com/seu-usuario/currency-converter-wpf.git
   ```
2. Abra o projeto no **Visual Studio**.
3. Certifique-se de que o pacote **Newtonsoft.Json** está instalado via **NuGet Package Manager**.
4. Compile e execute o projeto.

## API Utilizada
Este projeto utiliza a API de taxas de câmbio do Open Exchange Rates:
```
https://openexchangerates.org/api/latest.json?app_id=SEU_APP_ID
```
**Nota:** É necessário registrar-se no site para obter uma chave de API (APP ID).

## Exemplo de Uso
1. Escolha uma moeda de origem (exemplo: USD).
2. Escolha uma moeda de destino (exemplo: BRL).
3. Insira o valor a ser convertido.
4. O resultado aparecerá automaticamente.

![image](https://github.com/user-attachments/assets/c6ee8602-de3f-47d0-a9bd-20d58a45f5bf)



## Licença
Este projeto é distribuído sob a **MIT License**. Veja o arquivo `LICENSE` para mais detalhes.
