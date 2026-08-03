namespace FenixEngine.Shared.Services;

public class ServiceFile: IFileService
{
    public async Task CreateFile(string? pathFile)
    {
        if (string.IsNullOrWhiteSpace(pathFile))
            throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");

        if (!File.Exists(pathFile))
            using (File.Create(pathFile)) { }
    }

    public async Task FindFile(string? pathFile)
    {
        if (string.IsNullOrWhiteSpace(pathFile))
            throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");

        if (!File.Exists(pathFile))
            throw new FileNotFoundException($"El archivo no existe: {pathFile}");
    }

    public async Task EditFile(string? pathFile, string? newContent)
    {
        if (string.IsNullOrWhiteSpace(pathFile))
            throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");

        if (!File.Exists(pathFile))
            throw new FileNotFoundException($"El archivo no existe: {pathFile}");

        File.WriteAllText(pathFile, newContent ?? string.Empty);
    }

    public async Task DeleteFile(string? pathFolder)
    {
        if (string.IsNullOrWhiteSpace(pathFolder))
            throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

        if (Directory.Exists(pathFolder))
            Directory.Delete(pathFolder, recursive: true);
    }
}