using System.IO;
using System.IO.Compression;

public class ZipUtility
{
    public static byte[] CreateZipFromFiles(Dictionary<string, byte[]> files)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var fileEntry in files)
                {
                    var file = archive.CreateEntry(fileEntry.Key);
                    using (var entryStream = file.Open())
                    using (var fileContentStream = new MemoryStream(fileEntry.Value))
                    {
                        fileContentStream.CopyTo(entryStream);
                    }
                }
            }
            return memoryStream.ToArray();
        }
    }
}