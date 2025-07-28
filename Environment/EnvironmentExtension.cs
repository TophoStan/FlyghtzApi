namespace Environment;

public static class EnvironmentExtension
{
    public static string GetPostgresConnectionString() => System.Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
}