namespace E_Microservices.Source.Settings
{
    public interface ISourcingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
