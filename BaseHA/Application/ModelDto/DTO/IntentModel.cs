using BaseHA.Domain.Entity;

namespace BaseHA.Application.ModelDto.DTO
{
    public class IntentModel : BaseModel
    {

        public string? IntentCodeEn { get; set; }

        public string? IntentEn { get; set; }

        public string? IntentVn { get; set; }

        public bool Inactive { get; set; }
        public CategotyDTO CategotyDTO { get; set; }

        public IntentModel()
        {
        }
    }
}
