using HtmlAgilityPack;
using System.DirectoryServices;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Search_Tool
{
    public partial class MainWindow : Window
    {
        // Define a dictionary to map dropdown items to their respective websites
        private Dictionary<string, string> WebsiteUrls = new Dictionary<string, string>
        {
            { "Google", "https://www.google.com/search?q=" },
            { "Bing", "https://www.bing.com/search?q=" },
            { "Yahoo", "https://search.yahoo.com/search?p=" },
            // Add more mappings as needed
        };
        private object ex;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        // Event handler for Dropdown selection change
        private void Dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ComboBox
            var selectedWebsite = ((ComboBox)sender).SelectedItem as string;
            HtmlNodeCollection resultNodes=null;

            // Perform actions based on the selected item
            if (selectedWebsite != null)
            {
                // Retrieve the corresponding website URL
                if (WebsiteUrls.TryGetValue(selectedWebsite, out string websiteUrl))
                {
                    // Fetch data from the website and update the search result
                    string searchQuery = SearchTextBox.Text;
                    string searchResult = FetchDataFromWebsite(websiteUrl, resultNodes, searchQuery);
                    ((MainViewModel)DataContext).SearchResult = searchResult;
                }
                else
                {
                    ((MainViewModel)DataContext).SearchResult = "Website not supported.";
                }
            }
        }
        // Method to fetch data from the website
        private string FetchDataFromWebsite(string searchQuery, HtmlNodeCollection resultNodes, string searchQuery1)
        {
            try
            {
                // Get the selected website from the dropdown
                string selectedWebsite = (string)Dropdown.SelectedItem;

                // Retrieve the corresponding website URL
                if (WebsiteUrls.TryGetValue(selectedWebsite, out string websiteUrl))
                {
                    // Create a web request to the website
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(websiteUrl + Uri.EscapeDataString(searchQuery));
                    request.Method = "GET";

                    // Get the response from the website
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string responseData = reader.ReadToEnd();

                        // Parse the HTML content using HtmlAgilityPack
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(responseData);

                        // Extract search result data from HTML
                        // Example: Extracting search result titles
                        List<string> searchResults = new List<string>();
                        // Modify the XPath expression and data extraction logic based on the structure of the search results page of the specific website
                        if (doc.DocumentNode.SelectNodes("//div[@class='result']") != null)
                        {
                            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='result']").Nodes())
                            {
                                searchResults.Add(node.InnerText);
                            }
                        }

                        // Join search results into a single string
                        string searchResult = string.Join(Environment.NewLine, searchResults);

                        return searchResult;
                    }
                }
                else
                {
                    return "Website not supported.";
                }
            }
            catch (Exception ex)
            {
                // Handle any errors and return appropriate message
                return "Error: " + ex.Message;
            }
        }


        // Event handler for SearchTextBox text change
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the text from the search text box
            string searchText = ((TextBox)sender).Text;

            // Example: Update search result
            ((MainViewModel)DataContext).SearchResult = $"Search query: {searchText}";
        }

        // Event handler for SearchButton click
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the search query from the text box
            string searchQuery = SearchTextBox.Text;

            // Get the selected website
            string selectedWebsite = (string)Dropdown.SelectedItem;

            // Example: Perform search based on selected website and search query
            string searchResult = $"Searching {selectedWebsite} for '{searchQuery}'...";

            // Update search result
            ((MainViewModel)DataContext).SearchResult = searchResult;
        }
    }
}
