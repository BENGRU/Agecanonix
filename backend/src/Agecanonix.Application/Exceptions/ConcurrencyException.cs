namespace Agecanonix.Application.Exceptions;

/// <summary>
/// Exception levée quand une violation de concurrence optimiste est détectée
/// (la ressource a été modifiée par un autre utilisateur)
/// </summary>
public class ConcurrencyException : ApplicationException
{
    public ConcurrencyException(string message) 
        : base(message)
    {
    }

    public ConcurrencyException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
