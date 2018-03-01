﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
            fillCitiesListBox();
        }

        private void fillCitiesListBox()
        { 
            citiesListBox.ItemsSource = client.GetCities();
        }

        private void citiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string citySelected = citiesListBox.SelectedItem.ToString();
            stationsListBox.ItemsSource = client.GetStations(citySelected);
        }

        private void stationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stationsListBox.SelectedItem != null)
            {
                int bikes_available = client.GetAvailableVelibs(stationsListBox.SelectedItem.ToString());
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
    }
}
