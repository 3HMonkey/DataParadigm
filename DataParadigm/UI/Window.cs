using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParadigm.UI
{
    internal abstract class Window : IWindow
    {
        protected bool _isVisible;

        public bool IsVisible => _isVisible;

        public void Hide()
        {
            _isVisible = false;
        }

        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
        }

        public abstract bool Show(uint dockSpaceId);
    }
}
