using System;

namespace Helixbase.Feature.Sitecron.Extend
{
    public interface ISavedHandler
    {
        void OnItemSaved(object sender, EventArgs args);
    }
}
