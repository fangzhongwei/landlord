using SimpleSQL;

public class ConfigRow
{
    [PrimaryKey]
    public int Id { get; set; }
    public int ResourceVersion { get; set; }
    public string Lan { get; set; }
}
