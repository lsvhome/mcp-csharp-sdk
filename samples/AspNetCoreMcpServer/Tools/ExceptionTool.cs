using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AspNetCoreMcpServer.Tools;

[McpServerToolType]
public sealed class ExceptionTool
{
    [McpServerTool, Description("Raises exception for testing purposes")]
    public static string ThrowException(string message)
    {
        throw new Exception($"Exception text: {message}");
    }
}
