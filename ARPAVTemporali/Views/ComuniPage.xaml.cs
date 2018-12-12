using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Linq;

namespace ARPAVTemporali.Views
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public partial class ComuniPage : ContentPage
    {
        private const string Url = "http://jsonplaceholder.typicode.com/posts";
        private HttpClient _client = new HttpClient();
        private ObservableCollection<Post> _posts;

        public ComuniPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {

            var uri = new Uri(string.Format(Url, string.Empty));
			var response = await _client.GetAsync(uri); //assicurarsi di abilitare il permesso a usare internet in android->options->android application->required permissions

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<Post>>(content);
                _posts = new ObservableCollection<Post>(posts);
                listview.ItemsSource = _posts;
            }
           

            base.OnAppearing();
        }

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var post = e.SelectedItem as Post;
            ((ListView)sender).SelectedItem = null; //unselect
		}

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var post = e.Item as Post;
            DisplayAlert("Tapped", post.Body, "Ok");
        }

        public List<Post> GetComuni(string filter = null)
		{
            if (string.IsNullOrWhiteSpace(filter))
            {
                return _posts.ToList();
            }
            else
            {
                return _posts.Where( post => post.Title.ToLower().Contains(filter.ToLower())).ToList();
            }
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            string filter = e.NewTextValue;
            listview.ItemsSource = new ObservableCollection<Post>(GetComuni(filter));
        }

        // Context Actions
        public void OnMore(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
		}

        /*
         * TODO: far funzionare il remove anche dopo aver filtrato la lista
         */
		public void OnDelete(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
            Post post = mi.CommandParameter as Post;
            _posts.Remove(post);
            //_posts.Remove(_posts.Where(i => i.Id == post.Id).Single());
			DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
		}
    }
}
