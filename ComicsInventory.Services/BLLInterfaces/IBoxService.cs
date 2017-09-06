using System.Drawing;

namespace ComicsInventory.Services.BLLInterfaces
{
    public interface IBoxService
    {
        Bitmap CreateQrCode(string urlToEncode, int boxId);
    }
}