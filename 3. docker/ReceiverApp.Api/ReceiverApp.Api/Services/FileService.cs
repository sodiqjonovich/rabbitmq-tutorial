using ReceiverApp.Api.Interfaces;

namespace ReceiverApp.Api.Services;
public class FileService : IFileService
{
    private readonly string _path;
    public FileService(IWebHostEnvironment webEnv)
    {
        _path = Path.Combine(webEnv.WebRootPath, "Messages.txt");
    }
    public void Write(string text)
    {
        try
        {
            File.WriteAllText(_path, text);
        }
        catch
        {

        }
    }
}
