using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupCatalog {
    public interface IProgress {
        float Percentage { get; set; }
    }
}
