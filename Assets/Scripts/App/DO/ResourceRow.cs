using SimpleSQL;

public class ResourceRow {
    [PrimaryKey][AutoIncrement]
    public int Id { get; set; }
    public string Code { get; set; }
    public string Lan { get; set; }
    public string Desc { get; set; }
}
