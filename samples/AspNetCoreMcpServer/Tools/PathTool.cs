using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AspNetCoreMcpServer.Tools;

[McpServerToolType]
public sealed class PathTool
{
    /*
     * Determines whether the specified file or directory exists.
       Params:
       path — The path to check
       Returns:
       true if the caller has the required permissions and path contains the name of an existing file or directory; otherwise, false. This method also returns false if path is null, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified path, no exception is thrown and the method returns false regardless of the existence of path.
     */
    [McpServerTool, Description("Determines whether the specified file or directory exists.")]
    public static bool PathExists(string path)
    {
        return System.IO.Path.Exists(path);
    }
}
