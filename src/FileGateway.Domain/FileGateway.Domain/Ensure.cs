using System.Runtime.CompilerServices;

namespace FileGateway.Domain;

public static class Ensure
{
    public static T IsNotNull<T>(T value, [CallerArgumentExpression(nameof(value))] string argumentName = "") where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(argumentName, $"{argumentName} cannot be null.");
        }
        return value;
    }

    public static string IsNotNullOrWhiteSpace(string value, [CallerArgumentExpression(nameof(value))] string argumentName = "")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(argumentName, $"{argumentName} cannot be null.");
        }
        return value;
    }
}
