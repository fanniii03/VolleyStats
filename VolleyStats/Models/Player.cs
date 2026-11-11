using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyStats.Models
{
    public partial class Player : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private int jerseynumber;
        [ObservableProperty]
        private string position;
        [ObservableProperty]
        private int heightCm;
        [ObservableProperty]
        private string photoPath; // A kamera által készített kép helyi útvonala
    }
}
