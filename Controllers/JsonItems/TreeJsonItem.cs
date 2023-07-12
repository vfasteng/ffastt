using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers.JsonItems
{
    public class TreeJsonItem
    {
        public string text { get; set; }

        public string path { get; set; }

        public List<TreeJsonItem> nodes { get; set; }

        public TreeJsonItem()
        {
            this.nodes = new List<TreeJsonItem>();
        }

        public TreeJsonItem(CADFEA.AzureStorage.AzureFolder azureFolder)
        {
            this.text = azureFolder.Name;
            this.path = azureFolder.Path.Replace(String.Format("{0}/", CADFEA.AzureStorage.Storage.StorageFolder), String.Empty);
            this.nodes = GetChildNodes(azureFolder);
            if (this.nodes.Count == 0)
            {
                this.nodes = null;
            }
        }

        private List<TreeJsonItem> GetChildNodes(CADFEA.AzureStorage.AzureFolder azureFolder)
        {
            if (azureFolder == null)
            {
                return new List<TreeJsonItem>();
            }

            var outItems = new List<TreeJsonItem>();
            foreach (var child in azureFolder.Childrens)
            {
                if (child.Path.Contains("/bctool") ||
                    child.Path.Contains("/tool") ||
                    child.Path.Contains("/meshing") ||
                    child.Path.Contains("/calculix") ||
                    child.Path.Contains("/tetgen") ||
                    child.Path.Contains("/simres"))
                {
                    // Skip system folders
                    continue;
                }

                outItems.Add(new TreeJsonItem(child));
            }

            return outItems;
        }
    }
}