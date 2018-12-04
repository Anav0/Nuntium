
namespace Nuntium
{
    public class AttachmentsSectionDesign : AttachmentsSectionViewModel
    {
        public static AttachmentsSectionDesign Instance { get; set; } = new AttachmentsSectionDesign();


        public AttachmentsSectionDesign()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<AttachFileViewModel>
            {
                new AttachFileViewModel
                {
                    FileName = "index.css",
                    FileSize = 2500,
                },
                new AttachFileViewModel
                {
                    FileName = "report.md",
                    FileSize = 2500,
                },
                new AttachFileViewModel
                {
                    FileName = "funny.txt",
                    FileSize = 5000,
                },
                new AttachFileViewModel
                {
                    FileName = "photo_1234.jpg",
                    FileSize = 15000,
                },
            };
        }
    }
}
