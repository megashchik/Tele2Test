namespace WebServiceTele2.DTO
{
    internal class Age
    {
        public int MinAge { get; set; } = 0;
        public int MaxAge { get; set; } = int.MaxValue;

        public static ValueTask<Age?> BindAsync(HttpContext context)
        {
            const string minAgeString = "minAge";
            const string maxAgeString = "maxAge";
            int minAge, maxAge;
            Age result = new Age();
            if (int.TryParse(context.Request.Query[minAgeString], out minAge))
                result.MinAge = minAge;
            if (int.TryParse(context.Request.Query[maxAgeString], out maxAge))
                result.MaxAge = maxAge;

            return ValueTask.FromResult<Age?>(result);
        }
    }
}
