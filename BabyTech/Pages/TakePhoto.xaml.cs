using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Amazon.CognitoIdentity;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace BabyTech.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakePhoto : ContentPage
    {

        private const string AWSAccessKeyID = "";
        private const string AWSSecretAccessKey = "";

        private string facialAnalysisData;
        public string FacialAnalysisData
        {
            get { return facialAnalysisData;  }
            set
            {
                facialAnalysisData = value;
                OnPropertyChanged(nameof(FacialAnalysisData));
            }
        }   

        public TakePhoto()
        {
            InitializeComponent();
        }

        async void PickImage_Clicked(System.Object sender, System.EventArgs e)
        {

            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                resultImage.Source = ImageSource.FromStream(() => stream);
            }

        }

        async void TakeImage_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                resultImage.Source = ImageSource.FromStream(() => stream);
            }

        }

        async void Analyze_Clicked(System.Object sender, System.EventArgs e)
        {

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(AWSAccessKeyID, AWSSecretAccessKey, Amazon.RegionEndpoint.USEast1);

            DetectFacesRequest detectFacesRequest = new DetectFacesRequest()
            {
                Image = new Amazon.Rekognition.Model.Image()
                {
                    S3Object = new S3Object
                    {
                        Bucket = "babytech-images",
                        Name = "baby-eyes-mouth-open.jpg"
                    }
                },
                Attributes = new List<String>() { "ALL" }
            };

            try
            {
                DetectFacesResponse detectFacesResponse = await rekognitionClient.DetectFacesAsync(detectFacesRequest);

                foreach (FaceDetail face in detectFacesResponse.FaceDetails)
                {

                    const float confidence_threshold = 0.8F;

                    // check if mouth is open
                    if ((face.MouthOpen != null) && (face.MouthOpen.Confidence > confidence_threshold))
                    {
                        FacialAnalysisData += (face.MouthOpen.Value) ? "\n✔ Baby's mouth is open." : "\n❌ Baby's mouth is not open.";  
                    }
                    else
                    {
                        FacialAnalysisData += "\n❌ Unable to determine if baby's mouth is open.";
                    }

                    // check if eyes are open
                    if ((face.EyesOpen != null) && (face.EyesOpen.Confidence > confidence_threshold))
                    {
                        FacialAnalysisData += (face.EyesOpen.Value) ? "\n✔ Baby's eyes are open." : "\n❌ Baby's eyes are not open.";
                    }
                    else
                    {
                        FacialAnalysisData += "\n❌ Unable to determine if baby's eyes are open.";
                    }

                }

                DisplayAlert("Analysis Results", FacialAnalysisData, "OK");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}