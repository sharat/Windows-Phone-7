using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml;
using System.ServiceModel.Syndication;

namespace SilverLightWP7Demo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SyndicationItem[] Items;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            webBrowser.Visibility = Visibility.Collapsed;
            LoadFeedItems();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            // Load the feed items
            LoadFeedItems();
        }

        private void LoadFeedItems()
        {
            // Asynchronously load the feed content.
            WebClient client = new WebClient();
            Uri address = new Uri("http://feeds.feedburner.com/sharingmythoughts");
            client.OpenReadCompleted += client_openReadComplete;
            // Add the callback on completion
            client.OpenReadAsync(address);
            webBrowser.Visibility = Visibility.Collapsed;
            buttonBack.Visibility = Visibility.Collapsed;

        }
        private void client_openReadComplete(object sender, OpenReadCompletedEventArgs args)
        {
            try
            {
                listBoxPosts.Items.Clear();
                // try to load the result in the XML reader. 
                // Exception may occur if the request is failed
                XmlReader reader = XmlReader.Create(args.Result);

                // Syndication Feed is not supported by default.
                // It's safe to add this assembly.
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                Items = feed.Items.ToArray();
                foreach (SyndicationItem e in Items)
                    listBoxPosts.Items.Add(e.Title.Text);
            }
            catch
            {
                MessageBox.Show("Error downloading feed");
            }

        }

        private void OnTapItem(object sender, MouseButtonEventArgs e)
        {
            if (listBoxPosts.SelectedIndex >= 0)
            {
                buttonRefresh.Visibility = Visibility.Collapsed;
                buttonBack.Visibility = Visibility.Visible;
                webBrowser.Visibility = Visibility.Visible;
                Uri address = new Uri(Items[listBoxPosts.SelectedIndex].Id);
                webBrowser.Navigate(address);
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            buttonBack.Visibility = Visibility.Collapsed;
            webBrowser.Visibility = Visibility.Collapsed;
            buttonRefresh.Visibility = Visibility.Visible;
            listBoxPosts.Visibility = Visibility.Visible;
        }
        protected override void OnOrientationChanged(OrientationChangedEventArgs args)
        {
            // do your code.
            base.OnOrientationChanged(args);
        }
    }
}