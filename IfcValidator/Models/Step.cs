using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace IfcValidator.Models
{
    public class Step
    {
        public static SolidColorBrush ValidateBrush = new SolidColorBrush(Color.FromArgb(200, 0, 174, 86));
        public static SolidColorBrush NonValidateBrush = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
        public Step(int index, string header, bool completed = false)
        {
            Index = index;
            Header = header;
            IsCompleted = completed;
        }
        public int Index { get; set; }
        public string Header { get; set; }
        public bool IsCompleted { get; set; }
    }
}
