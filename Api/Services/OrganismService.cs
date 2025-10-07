using PlantAnimalApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantAnimalApi.Services
{
    public class OrganismService
    {
        private readonly List<Organism> _items = new()
        {
            new Organism { Id = 1, Common_Name = "Rose", ScientificName = "Rosa", Category = "Plant", ImageUrl = [], Description = "A common flowering plant." },
            new Organism { Id = 2, Common_Name = "Blue Jay", ScientificName = "Cyanocitta cristata", Category = "Animal", ImageUrl = [], Description = "A bright blue songbird." }
        };

        public Task<List<Organism>> GetAllAsync() => Task.FromResult(_items);

        public Task<Organism?> GetByIdAsync(int id) =>
            Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

        public Task<Organism> AddAsync(Organism item)
        {
            item.Id = _items.Count == 0 ? 1 : _items.Max(x => x.Id) + 1;
            _items.Add(item);
            return Task.FromResult(item);
        }
    }
}
