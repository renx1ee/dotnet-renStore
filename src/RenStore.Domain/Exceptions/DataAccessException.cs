namespace RenStore.Domain.Exceptions;

public class DataAccessException(string message, Exception innerException) : Exception(message, innerException);