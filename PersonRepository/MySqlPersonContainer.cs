using DTO;
using Microsoft.EntityFrameworkCore;

namespace PersonRepository
{
    public class MySqlPersonContainer : IPersonContainer
    {
        public async Task AddPeople(IEnumerable<Person> people)
        {
            using MySqlPersonContext context = new MySqlPersonContext();
            
            foreach (var p in people)
            {
                if (context.People.Any(n => n.Id == p.Id))
                    context.People.Update(p);
                else
                    context.People.Add(p);
            }
            await context.SaveChangesAsync();
        }

        public async Task<Person> GetPersonById(string id)
        {
            using MySqlPersonContext context = new MySqlPersonContext();
            var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if(person is object)
            {
                return person;
            }
            else
            {
                throw new InvalidDataException("Person with this id was not found");
            }
        }

        public async Task<List<Person>> GetPeople(int count = 50, int skip = 0, Sex sex = Sex.Any, int minAge = int.MinValue, int maxAge = int.MaxValue)
        {
            using MySqlPersonContext context = new MySqlPersonContext();
            IQueryable<Person> query = context.People.AsQueryable();
            if (sex != Sex.Any)
                query = query.Where(p => p.Sex == sex);
            query = query.Where(n => n.Age >= minAge && n.Age <= maxAge);
            return await query.Skip(skip).Take(count).ToListAsync();
        }
    }
}
