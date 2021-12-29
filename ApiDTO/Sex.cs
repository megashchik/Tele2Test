using System.ComponentModel;

namespace DTO
{
    public enum Sex
    {
        Any,
        [Description("male")]
        Male,
        [Description("female")]
        Female
    }
}
