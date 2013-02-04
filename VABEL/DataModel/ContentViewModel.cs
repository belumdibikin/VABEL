using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace VABEL.DataModel
{
    public class ContentViewModel
    {
        public ContentViewModel()
        {
        }

        public ObservableCollection<Content> searchFunction(string searchQuery)
        {
            string dataTemp = getSearch(searchQuery);

            ObservableCollection<Content> listContent = new ObservableCollection<Content>();
            var dataParser = JsonArray.Parse(dataTemp);
            foreach (var item in dataParser)
            {
                var obj = item.GetObject();
                Content itemData = new Content();

                foreach (var key in obj.Keys)
                {
                    IJsonValue val;
                    if (!obj.TryGetValue(key, out val)) continue;

                    if (val.GetString() != null)
                    {

                        switch (key)
                        {
                            case "title":
                                itemData.Title = val.GetString();
                                break;
                            case "cover":
                                itemData.Cover = "http://indonesia.npaperbox.com/resource/" + val.GetString() + ".jpg";
                                break;
                            case "description":
                                itemData.Description = val.GetString();
                                break;
                            case "file":
                                itemData.File = "http://indonesia.npaperbox.com/resource/" + val.GetString() + ".mp4";
                                break;
                        }
                    }
                }

                listContent.Add(itemData);

            }

            return listContent;

        }

        public string getSearch(string searchQuery)
        {
            string query = "http://localhost/Vabel/getData.php?query=" + searchQuery;

            HttpWebRequest myReq = HttpWebRequest.CreateHttp(query);
            HttpWebResponse response = (HttpWebResponse)myReq.GetResponseAsync().Result;

            Stream streamData = response.GetResponseStream();

            StreamReader dataReader = new StreamReader(streamData);

            return dataReader.ReadToEnd();

        }

    }

    public class Content
    {
        public string Cover { set; get; }
        public string File { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
    }
}
