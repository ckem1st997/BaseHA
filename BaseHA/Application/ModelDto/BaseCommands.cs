namespace BaseHA.Application.ModelDto
{
    public class BaseCommands
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool Ondelete { get; set; }
    }
    
    public class BaseModel
    {
        public string Id { get; set; }
        public bool Ondelete { get; set; }
    }
}
