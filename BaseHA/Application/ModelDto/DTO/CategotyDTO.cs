namespace BaseHA.Application.ModelDto.DTO
{
    public class BaseModel
    {
        public string Id { get; set; }
        public bool OnDelete { get; set; }
    }
    public class CategotyDTO : BaseModel
    {
        public string NameCategory { get; set; } 
        public string IntentCodeEn { get; set; } 
        public string IntentCodeVn { get; set; } 
        public string? Description { get; set; }
        public string? ParentId { get; set; }
        public bool Inactive { get; set; }

        public virtual IEnumerable<CategotyDTO> CategotyDTOs { get; set; }
    }
}
