namespace RenStore.Domain.Exceptions;

public class DuplicateException(Type type, object key) : Exception;