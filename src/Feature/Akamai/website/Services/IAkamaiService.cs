using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helixbase.Feature.Akamai.Services
{
    public interface IAkamaiService
    {
        void AkamaiInvalidate(Item sourceItem);
        void InvalidateByUrl(string url);
    }
}