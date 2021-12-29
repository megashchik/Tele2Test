using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO
{
    public record PersonSmall
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("sex")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Sex Sex { get; init; }

        public PersonSmall(string id, string name, Sex sex)
        {
            Id = id;
            Name = name;
            Sex = sex;
        }

        public static implicit operator PersonSmall(Person person)
        {
            return new PersonSmall(person.Id, person.Name, person.Sex);
        }
    }
}
