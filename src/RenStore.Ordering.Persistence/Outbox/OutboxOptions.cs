namespace RenStore.Order.Persistence.Outbox;

/// <summary>
/// Configuration for <see cref="OutboxWorker"/>.
/// Bind from appsettings: "Outbox" section.
/// </summary>
public sealed class OutboxOptions
{
    public const string SectionName = "Outbox";

    /// <summary>
    /// How often the worker polls for unprocessed messages, in seconds.
    /// Default: 5.
    /// </summary>
    public int PollingIntervalSeconds { get; init; } = 5;

    /// <summary>
    /// Maximum number of messages processed per polling cycle.
    /// Default: 50.
    /// </summary>
    public int BatchSize { get; init; } = 50;

    /// <summary>
    /// Maximum number of publish attempts before a message is skipped (dead-lettered).
    /// Default: 5.
    /// </summary>
    public int MaxRetryCount { get; init; } = 5;
}