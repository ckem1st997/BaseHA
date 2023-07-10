using System.Collections;
using System.Collections.Generic;

namespace BaseHA.Models
{
    public enum ActiveStatus
    {
        //
        // Summary:
        //     Kích hoạt
        Activated = 1,
        //
        // Summary:
        //     Ngừng kích hoạt
        Deactivated
    }


    public interface ISettings
    {
    }
    public class AdminAreaSettings
    {
        public int GridPageSize { get; set; }

        public int GridButtonCount { get; set; }

        public IEnumerable GridPageSizeOptions { get; set; }

        public string RichEditorFlavor { get; set; }

        public AdminAreaSettings()
        {
            GridPageSize = 15;
            GridButtonCount = 5;
            GridPageSizeOptions = new string[5] { "15","50", "100", "200", "500" };
            RichEditorFlavor = "RichEditor";
        }
    }
}
