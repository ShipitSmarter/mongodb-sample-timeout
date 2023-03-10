using MongoDB.Bson;

namespace Sample;

public class Entity
{
    public ObjectId Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; set; }
    public int SchemaVersion { get; set; } = 1;
    public required Upload Data { get; set; }
}

public class Upload
{
    public string ConnectionId { get; set; } = null!;
    public Guid BlobReference { get; set; }
    public string Filename { get; set; } = null!;
    public string Status { get; set; }

    public string? ErrorDescription { get; set; }
}