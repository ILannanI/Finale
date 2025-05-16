using System.Collections.Generic;

namespace PokemonExplorerWPF.Models
{
    public class PokemonDataModel
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Color { get; set; }
        public List<string> Locations { get; set; }
        public List<string> Genders { get; set; }
        public List<string> Versions { get; set; }
    }
}
