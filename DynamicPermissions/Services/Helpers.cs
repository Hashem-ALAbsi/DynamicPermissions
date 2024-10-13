namespace DynamicPermissions.Services
{
    internal static class Helpers
    {
        public static double GetAge(DateTime? age)
        {
            var v = age.Value.CompareTo(DateTime.Now);
            return v;
        }
    }
}