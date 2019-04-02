namespace Shared
{
    public class Config : ReflectionConfig
    {
        [ConnectionStringReflectionConfig]
        public string ConnectionString { get; protected set; }
        public int SealingSlabChunkSize { get; protected set; }
        public string CsvExportDirectory { get; protected set; }
        public int MaxPixelDimension { get; protected set; }
        public int CancellationCheckInterval { get; protected set; }
        public int SaveIterationsCount { get; protected set; }
    }
}
