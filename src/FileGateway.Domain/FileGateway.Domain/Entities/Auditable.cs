namespace FileGateway.Domain.Entities
{
    public record Auditable
    {
        public DateTime CreatedOn { get; } = DateTime.UtcNow;
        public Guid Owner { get; set; }
    }
}
