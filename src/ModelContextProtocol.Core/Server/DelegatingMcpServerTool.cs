using ModelContextProtocol.Protocol;
using System;
using System.Text.Json;

namespace ModelContextProtocol.Server;

/// <summary>Provides an <see cref="McpServerTool"/> that delegates all operations to an inner <see cref="McpServerTool"/>.</summary>
/// <remarks>
/// This is recommended as a base type when building tools that can be chained around an underlying <see cref="McpServerTool"/>.
/// The default implementation simply passes each call to the inner tool instance.
/// </remarks>
public abstract class DelegatingMcpServerTool : McpServerTool
{
    private readonly McpServerTool _innerTool;

    /// <summary>Initializes a new instance of the <see cref="DelegatingMcpServerTool"/> class around the specified <paramref name="innerTool"/>.</summary>
    /// <param name="innerTool">The inner tool wrapped by this delegating tool.</param>
    /// <exception cref="ArgumentNullException"><paramref name="innerTool"/> is <see langword="null"/>.</exception>
    protected DelegatingMcpServerTool(McpServerTool innerTool)
    {
        Throw.IfNull(innerTool);
        _innerTool = innerTool;
    }

    /// <inheritdoc />
    public override Tool ProtocolTool => _innerTool.ProtocolTool;

    /// <inheritdoc />
    public override IReadOnlyList<object> Metadata => _innerTool.Metadata;

    /// <inheritdoc />
    public override async ValueTask<CallToolResult> InvokeAsync(
        RequestContext<CallToolRequestParams> request, 
        CancellationToken cancellationToken = default)
        {
            try
            {
                var ret =  await _innerTool.InvokeAsync(request, cancellationToken).ConfigureAwait(false);
                return ret;
            }
            catch(Exception ex)
            {
                //throw new McpException("Error while calling tool", ex);
                var ret = new CallToolResult
                    {
                        IsError = false,
                        Content = [new TextContentBlock
                        {
                            Text = 
                                $"An error occurred invoking '{request.Params?.Name}': {ex.Message}"
                        }],

                //         //  Content = new List<ContentBlock>
                //         //  {
                //         //      new ContentBlock{
                //         //          Annotations = new Annotations{ },
                //         //          Meta = 
                             
                //         //       }
                //         //  }
                //         //StructuredContent = ex.to

                     };
                return ret;
            }
        }

    /// <inheritdoc />
    public override string ToString() => _innerTool.ToString();
}



// #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
// public static class ExceptionExtensions
// #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
// {
//     public static JsonElement ToJsonElement(this Exception exception)
//     {
//         // Створюємо анонімну структуру з базовими даними помилки
//         var errorDetails = new ErrorDetails
//         {
//             TypeName = exception.GetType()?.FullName ?? string.Empty,
//             Message = exception.Message,
//             StackTrace = exception.StackTrace ?? string.Empty,
//             InnerException = exception.InnerException?.Message ?? string.Empty // або рекурсивно
//         };

//         var options = new JsonSerializerOptions();
//         options.Converters.Add(new ExceptionConverter());

//         //Exception ex = new InvalidOperationException("Щось пішло не так", new ArgumentException("Wrong argument"));

//         // Серіалізуємо за допомогою нашого конвертера
//         string json = JsonSerializer.Serialize(exception, options);


//         // Серіалізуємо у рядок і парсимо як JsonElement
//         string jsonString = JsonSerializer.Serialize<ErrorDetails>(errorDetails, 
//             new System.Text.Json.Serialization.Metadata.JsonTypeInfo<ErrorDetails>());
//         using JsonDocument document = JsonDocument.Parse(jsonString);
        
//         // Повертаємо Clone(), оскільки оригінальний document буде утилізовано
//         return document.RootElement.Clone();
//     }
// }

// [Serializable]
// public class ErrorDetails
// {
//     public string TypeName { get;set;}
//     public string Message { get;set;}

//     public string StackTrace { get;set;}

//     public string InnerException { get;set;}
// }

// public class ExceptionConverter : JsonConverter<Exception>
// {
//     public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         throw new NotImplementedException("Десеріалізація виключень не підтримується.");
//     }

//     public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
//     {
//         writer.WriteStartObject();
//         writer.WriteString("Type", value.GetType().FullName);
//         writer.WriteString("Message", value.Message);
//         writer.WriteString("StackTrace", value.StackTrace);
        
//         if (value.InnerException != null)
//         {
//             writer.WritePropertyName("InnerException");
//             Write(writer, value.InnerException, options); // Рекурсивний запис
//         }
//         writer.WriteEndObject();
//     }
// }
// #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
