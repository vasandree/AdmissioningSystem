namespace Common.Configurators.ConfigClasses;

public class EmailConfig
{
    public required string FromAddress { get; set; }
    public required string FromName { get; set; }
    public required string SmtpServer { get; set; }
    public required int SmtpPort { get; set; }
    public required string SmtpUsername { get; set; }
    public required string SmtpPassword { get; set; }
}