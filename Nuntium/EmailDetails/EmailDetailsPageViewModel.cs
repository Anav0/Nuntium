
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;

namespace Nuntium
{
    public class EmailDetailsPageViewModel : BaseViewModel
    {

        public string Html { get; set; }

        public ObservableCollection<AttachFileViewModel> AttachementsList { get; set; }

        public EmailDetailsPageViewModel()
        {
            Html = GetHtmlFromLink("http://c0185784a2b233b0db9b-d0e5e4adc266f8aacd2ff78abb166d77.r51.cf2.rackcdn.com/v1_templates/template_02.html");

            AttachementsList = new ObservableCollection<AttachFileViewModel>
                {
                    new AttachFileViewModel
                    {
                        FileName = "cat1.jpg",
                        FileSize = 3058506,
                    },

                    new AttachFileViewModel
                    {
                        FileName = "cat2.jpg",
                        FileSize = 5068576,
                    },
                };
        }

        public string GetHtmlFromLink(string link)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                var output = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return output;
            }

            return response.StatusDescription;
        }
    }

}
