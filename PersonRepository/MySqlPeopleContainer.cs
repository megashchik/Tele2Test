using DTO;
using Microsoft.EntityFrameworkCore;

namespace PersonRepository
{
    public class MySqlPeopleContainer : IPeopleContainer
    {
        public async Task AddPeople(IEnumerable<Person> people)
        {
            using MySqlPeopleContext context = new MySqlPeopleContext();
            //var toUpdateId = context.People.Select(p => p.Id).Intersect(people.Select(x => x.Id).ToArray());
            var toUpdateId = context.People.Where(p => people.Select(n => n.Id).Contains(p.Id));
            //var toAddId = people.Select(p => p.Id).Except(toUpdateId);
            var toAddId = people.Where(p => !toUpdateId.Select(n => n.Id).Contains(p.Id));

            context.People.UpdateRange(toUpdateId);
            context.People.AddRange(toAddId);

            //context.People.UpdateRange(people.Where(p => toUpdateId.Contains(p.Id)));
            //context.People.AddRange(people.Where(p => toAddId.Contains(p.Id)));
            /*foreach (var p in people)
            {
                if (context.People.Any(n => n.Id == p.Id))
                    context.People.Update(p);
                else
                    context.People.Add(p);
            }*/
            await context.SaveChangesAsync();
        }

        public async Task<Person> GetPersonById(string id)
        {
            using MySqlPeopleContext context = new MySqlPeopleContext();
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
            using MySqlPeopleContext context = new MySqlPeopleContext();
            IQueryable<Person> query = context.People.AsQueryable();
            if (sex != Sex.Any)
                query = query.Where(p => p.Sex == sex);
            query = query.Where(n => n.Age >= minAge && n.Age <= maxAge);
            return await query.Skip(skip).Take(count).ToListAsync();
        }
    }
}
