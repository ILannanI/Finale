using PokeApiNet;
using System.Windows;
using System.Windows.Controls;

namespace PokemonExplorerWPF.Views
{
    public partial class DetailWindow : Window
    {
        private readonly PokeApiClient client = new PokeApiClient();
        private readonly int pokemonId;

        public DetailWindow(int id)
        {
            InitializeComponent();
            pokemonId = id;
            LoadEvolutionChain();
        }

        private async void LoadEvolutionChain()
        {
            var pokemon = await client.GetResourceAsync<Pokemon>(pokemonId);
            var species = await client.GetResourceAsync(pokemon.Species);
            var evolutionChain = await client.GetResourceAsync(species.EvolutionChain);
            var chain = evolutionChain.Chain;
            TraverseChain(chain);
        }

        private void TraverseChain(ChainLink node)
        {
            string name = node.Species.Name;
            string trigger = "";
            int? level = null;

            foreach (var detail in node.EvolutionDetails)
            {
                trigger = detail.Trigger?.Name;
                level = detail.MinLevel;
            }

            EvolutionStack.Children.Add(new TextBlock
            {
                Text = $"Name: {name}, Trigger: {trigger}, Min Level: {level}",
                FontSize = 16,
                Margin = new Thickness(0, 10, 0, 0)
            });

            foreach (var next in node.EvolvesTo)
            {
                TraverseChain(next);
            }
        }
    }
}
