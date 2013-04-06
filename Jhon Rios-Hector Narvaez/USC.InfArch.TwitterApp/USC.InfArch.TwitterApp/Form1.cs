using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TweetSharp;

namespace USC.InfArch.TwitterApp
{
    public partial class Form1 : Form
    {
        private static string _consumerKey = "AgrTOY6M3IiVg8a7uw";
        private static string _consumerSecret = "6GRwk4jJMORwTmcRblZFABuzbaDa3FUHnqR0N8Ys";
        private OAuthRequestToken _requestToken { get; set; }
        private TwitterService _twitterService { get; set; }
        private bool _alreadyRegister { get; set; }

        public Form1()
        {
            InitializeComponent();
            _alreadyRegister = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listTimeLineTweets();
            //getPublicTweets();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getAuthorizationUri();
        }

        public void getAuthorizationUri()
        {
            try
            {
                // Pass your credentials to the service
                _twitterService = new TwitterService(_consumerKey, _consumerSecret);

                // Step 1 - Retrieve an OAuth Request Token
                _requestToken = _twitterService.GetRequestToken();

                // Step 2 - Redirect to the OAuth Authorization URL
                var uri = _twitterService.GetAuthorizationUri(_requestToken);
                Process.Start(uri.ToString());
                button1.Enabled = true;
                textBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error tratando de obtener el código de verificación.");
            }

        }

        public void listTimeLineTweets()
        {
            listView1.Items.Clear();

            if (!_alreadyRegister)
            {
                // Step 3 - Exchange the Request Token for an Access Token
                string verifier = textBox1.Text; // <-- This is input into your application by your user
                var access = _twitterService.GetAccessToken(_requestToken, verifier);
                // Step 4 - User authenticates using the Access Token
                _twitterService.AuthenticateWith(access.Token, access.TokenSecret);
                _alreadyRegister = true;
            }

            var options = new ListTweetsOnHomeTimelineOptions();
            options.Count = 200;
            // Step 5 - get Tweets
            var tweets = _twitterService.ListTweetsOnHomeTimeline(options);

            if (tweets == null)
                return;

            foreach (var tweet in tweets)
            {
                if(tweet.Text.ToLower().Contains(textBox2.Text.ToLower()))
                {

                    var values = new[]
                                     {
                                         tweet.User.ScreenName, tweet.Text, tweet.RetweetCount.ToString(),
                                         tweet.IsFavorited.ToString()
                                     };
                    var lvItem = new ListViewItem(values);
                    listView1.Items.Insert(listView1.Items.Count, lvItem);
                }
                //Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
            }
            //IEnumerable<TwitterStatus> mentions = service.ListTweetsMentioningMe();
        }

        public void getPublicTweets()
        {
            SearchOptions searchOptions = new SearchOptions();

            searchOptions.Count = 2000;
            searchOptions.Locale = "-122.75,36.8,-121.75,37.8";
            //var twitterResults = _twitterService.Search(searchOptions);
            GeoSearchOptions geoSearch = new GeoSearchOptions();
            geoSearch.ContainedWithin = "-122.75,36.8,-121.75,37.8";
            //geoSearch.
            var cosas = _twitterService.GeoSearch(geoSearch);

            Console.WriteLine("Hola");
            //TwitterGeoLocation geoLocation = new TwitterGeoLocation();
            //geoLocation.Coordinates = 
            //searchOptions.

            //_twitterService.Search();

        }
    }
}
