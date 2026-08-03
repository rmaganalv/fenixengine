
namespace FenixEngine.Shared.Services;
    public class ServiceFolder: IFolderService
    {
        public async Task CreateFolder(string? pathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder))
                throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder);
        }

        public async Task DeleteFolder(string? pathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder))
                throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

            if (Directory.Exists(pathFolder))
                Directory.Delete(pathFolder, recursive: true);
        }

        public async Task FindFolder(string? pathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder))
                throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

            if (!Directory.Exists(pathFolder))
                throw new DirectoryNotFoundException($"La carpeta no existe: {pathFolder}");
        }

        public async Task EditFolder(string? pathFolder, string? newPathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder) || string.IsNullOrWhiteSpace(newPathFolder))
                throw new ArgumentException("Las rutas no pueden ser nulas o vacías.");

            if (!Directory.Exists(pathFolder))
                throw new DirectoryNotFoundException($"La carpeta no existe: {pathFolder}");

            Directory.Move(pathFolder, newPathFolder);
        }


    }


