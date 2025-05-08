using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBasicApp02.Model
{
    public class Book : INotifyPropertyChanged
    {
        public int Idx { get; set; }
        public string DName { get; set; }
        public string Names { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime ReleaseTime { get; set; }
        public int Price { get; set; }
        // 위의 여덟 개의 값이 기존상태에서 변경이 되면 발생하는 이벤트
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
