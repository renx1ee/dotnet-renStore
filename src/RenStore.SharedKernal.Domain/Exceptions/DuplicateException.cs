namespace RenStore.SharedKernal.Domain.Exceptions;

public class DuplicateException(Type type, object key) : Exception;