using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.Domain
{
    public interface IProcessDirty : INotifyPropertyChanged
    {
        Boolean IsNew { get; }
        Boolean IsDirty { get; }
        Boolean IsDeleted { get; }
    }
}
