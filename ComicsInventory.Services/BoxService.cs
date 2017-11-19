using System;
using System.Collections.Generic;
using System.Drawing;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Repositories.Interfaces;
using ComicsInventory.Services.BLLInterfaces;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace ComicsInventory.Services
{
  public  class BoxService : IBoxService
  {
      private readonly IBoxRepo _boxRepo;

      public BoxService(IBoxRepo box)
      {
          _boxRepo = box;
      }

        public Bitmap CreateQrCode(string urlToEncode, int boxId)
        {
            string url; //to be set upon checking of sent parameters
            if (!string.IsNullOrWhiteSpace(urlToEncode))
            {
                //check if urlToencode is empty if not url equals the sent
                url = urlToEncode;
            }
            else
            {
                /*if it does send null, then apply the box id to the following url as the encode
                This provides redundancy if urlToencode is empty somehow*/
                url = "http://www.tappaucomics.co.uk/Box/Details/?boxId=" + Convert.ToString(boxId);
            }
            var zXingWriter = new BarcodeWriter //Create a new object of Barcode
            {
                Format = BarcodeFormat.QR_CODE, //defines the format to output
                Options =
                    new QrCodeEncodingOptions
                    {
                        Height = 250, //define size of the qr code
                        Width = 250,
                        Margin = 0,
                        ErrorCorrection = ErrorCorrectionLevel.Q //defines the error correction level
                    }
            };
            var x = zXingWriter.Write(url);
            return x;
        }

      public IEnumerable<BoxContentDto> GetBoxIssueDetails(int boxId)
      {
          return _boxRepo.GetBoxContents(boxId);
      }
    }
}
