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
using WPFClientForVelibGateway.VelibGatewayWS;

namespace WPFClientForVelibGateway
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VelibServiceClient client = new VelibServiceClient();
        private string[] cities;
        private string[] stations;
        private string citiesSearchPlaceholder = "Rechercher une ville...";
        private string stationsSearchPlaceholder = "Rechercher une station...";
        public MainWindow()
        {
            InitializeComponent();
            fillCitiesListBox();
        }

        private async void fillCitiesListBox()
        {
            cities = await client.GetCitiesAsync();
            foreach (string city in cities)
            {
                citiesListBox.Items.Add(city);
            }
        }

        private async void citiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (citiesListBox.SelectedItem != null)
            {
                string citySelected = citiesListBox.SelectedItem.ToString();
                stations = await client.GetStationsAsync(citySelected);
                stationsListBox.Items.Clear();
                foreach (string station in stations)
                {
                    stationsListBox.Items.Add(station);
                }
            }
        }

        private async void stationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stationsListBox.SelectedItem != null)
            {
                int bikes_available = await client.GetAvailableVelibsAsync(stationsListBox.SelectedItem.ToString());
                available_bikes_label.Visibility = Visibility.Visible;
                available_bikes_number.Visibility = Visibility.Visible;
                available_bikes_number.Content = bikes_available.ToString();
            }
            else
            {
                available_bikes_label.Visibility = Visibility.Hidden;
                available_bikes_number.Visibility = Visibility.Hidden;
                available_bikes_number.Content = "";
            }
        }


        private void CitySearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CitySearchTextBox.Text = "";
        }

        private void CitySearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CitySearchTextBox.Text))
                CitySearchTextBox.Text = citiesSearchPlaceholder;
        }

        private void StationSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            StationSearchTextBox.Text = "";
        }

        private void StationSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(StationSearchTextBox.Text))
                StationSearchTextBox.Text = stationsSearchPlaceholder;
        }

        private void CitySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CitySearchTextBox.Text != citiesSearchPlaceholder && cities != null)
            {
                citiesListBox.Items.Clear();
                foreach (string str in cities)
                {
                    if (str.StartsWith(CitySearchTextBox.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        citiesListBox.Items.Add(str);
                    }
                }
            }
        }

        private void StationSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (StationSearchTextBox.Text != stationsSearchPlaceholder && stations != null)
            {
                stationsListBox.Items.Clear();
                foreach (string str in stations)
                {
                    if (str.IndexOf(StationSearchTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        stationsListBox.Items.Add(str);
                    }
                }
            }
        }
    }
}
