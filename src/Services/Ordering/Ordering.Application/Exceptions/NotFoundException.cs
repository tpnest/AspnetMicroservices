namespace Ordering.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string entity, object key)
    :base($"entity \"{entity}\" ({key}) was not found.")
    {
        
    }
}