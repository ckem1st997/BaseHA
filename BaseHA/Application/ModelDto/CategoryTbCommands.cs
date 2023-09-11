using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseHA.Application.ModelDto
{
    public partial class CategoryTbCommands : BaseCommands
    {

        public CategoryTbCommands()
        { 
            Intents=new HashSet<Intent>();
            //AvailableCategoryTb = new List<SelectListItem>();
        }
       /* public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool Ondelete { get; set; }*/
        public string Category { get; set; }
        public string IntentCodeEn { get; set; }
        public string IntentCodeVn { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Intent> Intents { get; set; }

        //public IList<SelectListItem> AvailableCategoryTb { get; internal set; }
    }
}
