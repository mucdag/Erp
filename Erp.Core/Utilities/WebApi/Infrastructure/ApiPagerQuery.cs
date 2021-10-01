namespace Core.Utilities.WebApi.Infrastructure
{
    public class ApiPagerQuery
    {
        public int Skip { get; set; }
        public int Take { get; set; } = 10; //Default
    }
}
