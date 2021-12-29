using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO
{
    public record Person
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; init; }
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("sex")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Sex Sex { get; init; }
        [JsonPropertyName("age")]
        public int Age { get; init; }

        public Person(string id, string name, Sex sex, int age)
        {
            Id = id;
            Name = name;
            Sex = sex;
            Age = age;
        }
    }
}
