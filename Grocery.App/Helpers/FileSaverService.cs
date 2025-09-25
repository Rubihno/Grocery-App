using CommunityToolkit.Maui.Storage;
using Grocery.Core.Interfaces.Services;
using System.Text;


#if MACCATALYST
using Foundation;
using UIKit;
#endif

namespace Grocery.Core.Services
{
    public class FileSaverService : IFileSaverService
    {
        public async Task SaveFileAsync(string fileName, string content, CancellationToken cancellationToken)
        {
#if MACCATALYST
            var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            await File.WriteAllTextAsync(tempFilePath, content, cancellationToken);
        
            var tempFileUrl = NSUrl.FromFilename(tempFilePath);
        
            var picker = new UIDocumentPickerViewController(new[] { tempFileUrl }, UIDocumentPickerMode.ExportToService)
            {
                ModalPresentationStyle = UIModalPresentationStyle.FullScreen
            };
        
            var window = UIApplication.SharedApplication.KeyWindow;
            var rootVC = window?.RootViewController;
            if (rootVC == null)
                throw new Exception("No root view controller available");
        
        await rootVC.PresentViewControllerAsync(picker, true);
#else
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var result = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);

            if (!result.IsSuccessful)
                throw result.Exception ?? new IOException("Unknown error saving file.");
#endif
        }
    }
}
