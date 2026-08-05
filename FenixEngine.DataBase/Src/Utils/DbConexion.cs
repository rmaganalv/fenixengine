namespace FenixEngine.DataBase.Src.Utilis
{
    public static class DbConexion
    {
        public static string DbPath(string dbName)
        {
            // FileSystem.AppDataDirectory devuelve la ruta correcta con permisos de escritura
            // tanto en Mac Catalyst (~/Library/Containers/.../Data/Library/Application Support) 
            // como en Windows (%LOCALAPPDATA%).
            return Path.Combine(FileSystem.AppDataDirectory, dbName);
        }
    }
}