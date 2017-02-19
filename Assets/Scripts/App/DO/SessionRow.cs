using  SimpleSQL;

public class SessionRow {
    [PrimaryKey]
    public int Id {get; set; }
    public string Token { get; set; }
    public string Mobile { get; set; }
    public int Status { get; set; }
    public string NickName { get; set; }
}
