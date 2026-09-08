using System.Text;

namespace FenixEngine.AppCore.Nodes.Common;

public static class FileReadHandler
{
    public async static Task<string> ReadAndConsolidateFilesAsync(string sourceDirectory, Func<string, bool>? fileFilter = null)
    {
        if (!Directory.Exists(sourceDirectory))
            throw new DirectoryNotFoundException($"La ruta '{sourceDirectory}' no existe.");

        var stringBuilder = new StringBuilder();
        
        // Obtener todos los archivos (puedes filtrar por extensión si lo necesitas)
        var files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.TopDirectoryOnly)
                            .OrderBy(f => f); // Ordenar para mantener consistencia

        foreach (var filePath in files)
        {
            // Filtro opcional (ej. solo .cs o .txt)
            if (fileFilter != null && !fileFilter(filePath))
                continue;

            // Leer contenido
            string content = await File.ReadAllTextAsync(filePath);
            
            // Opcional: Agregar un separador o comentario con el nombre del archivo
            stringBuilder.AppendLine($"// --- Inicio de archivo: {Path.GetFileName(filePath)} ---");
            stringBuilder.AppendLine(content);
            stringBuilder.AppendLine($"// --- Fin de archivo: {Path.GetFileName(filePath)} ---");
            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    /*
    // Ejemplo de uso
    string codigoConsolidado = await ReadAndConsolidateFilesAsync(@"C:\MiProyecto\Src", path => path.EndsWith(".cs"));   

    */

}