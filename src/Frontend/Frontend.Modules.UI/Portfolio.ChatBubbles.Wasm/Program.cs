using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Portfolio.ChatBubbles.Wasm.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.RegisterCustomElement<ChatBubbles>("chat-bubbles");
builder.Services.AddScoped(sp => new HttpClient());
builder.Logging.SetMinimumLevel(LogLevel.Debug);
await builder.Build().RunAsync();
