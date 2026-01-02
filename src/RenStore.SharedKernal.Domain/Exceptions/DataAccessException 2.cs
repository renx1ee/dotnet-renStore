namespace RenStore.SharedKernal.Domain.Exceptions;

public class DataAccessException(string message, Exception innerException) : Exception(message, innerException);