using Newtonsoft.Json;
using PokeApiNet;
using PokemonExplorerWPF.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace PokemonExplorerWPF.Views
{
    public partial class MainWindow : Window
    {
        private readonly PokeApiClient client = new PokeApiClient();
        private int currentPokemonId;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            int id = rand.Next(1, 1011);
            currentPokemonId = id;

            var pokemon = await client.GetResourceAsync<Pokemon>(id);
            var species = await client.GetResourceAsync(pokemon.Species);
            var encounterUri = pokemon.LocationAreaEncounters;

            var encounterList = await GetEncountersAsync(encounterUri);
            var genderList = await client.GetNamedResourcePageAsync<Gender>();
            var gameVersions = species.GameIndices;

            NameText.Text = $"Name: {pokemon.Name}";
            HeightText.Text = $"Height: {pokemon.Height}";
            WeightText.Text = $"Weight: {pokemon.Weight}";
            ColorText.Text = $"Color: {species.Color.Name}";

            var locations = new List<string>();
            foreach (var enc in encounterList)
            {
                locations.Add(enc.LocationArea.Name);
            }
            LocationText.Text = $"Locations: {string.Join(", ", locations)}";

            var genders = new List<string>();
            foreach (var gender in genderList.Results)
            {
                var genderData = await client.GetResourceAsync(gender);
                foreach (var g in genderData.PokemonSpeciesDetails)
                {
                    if (g.PokemonSpecies.Name == species.Name)
                        genders.Add(gender.Name);
                }
            }
            GenderText.Text = $"Genders: {string.Join(", ", genders)}";

            var versions = new List<string>();
            foreach (var version in gameVersions)
            {
                versions.Add(version.Version.Name);
            }
            VersionText.Text = $"Versions: {string.Join(", ", versions)}";

            MoreInfoButton.Visibility = Visibility.Visible;
        }

        private async Task<List<LocationAreaEncounter>> GetEncountersAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<LocationAreaEncounter>>(json);
            }
        }

        private void MoreInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var detailWindow = new DetailWindow(currentPokemonId);
            detailWindow.Show();
        }
    }
}
