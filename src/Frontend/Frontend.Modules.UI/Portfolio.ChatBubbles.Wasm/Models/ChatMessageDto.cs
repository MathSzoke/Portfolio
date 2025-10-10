using System.Text.Json.Serialization;

namespace Portfolio.ChatBubbles.Wasm.Models;

public sealed class ChatMessageDto
{
    [JsonPropertyName("id")] public string Id { get; set; } = default!;
    [JsonPropertyName("sessionId")] public string SessionId { get; set; } = default!;
    [JsonPropertyName("sender")] public string Sender { get; set; } = default!;
    [JsonPropertyName("content")] public string Content { get; set; } = default!;
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("readAt")] public DateTime? ReadAt { get; set; }
}
