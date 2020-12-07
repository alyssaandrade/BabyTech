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

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient("AKIAQHIJUMP7ZHNBDTND", "wcXPTRYMfCYt5Nu7qPLtwwgSBMcF9auEya3dULDX", Amazon.RegionEndpoint.USEast1);

            DetectFacesRequest detectFacesRequest = new DetectFacesRequest()
            {
                Image = new Amazon.Rekognition.Model.Image()
                {
                    S3Object = new S3Object
                    {
                        Bucket = "babytech-images",
                        Name = "newborn-behaviour-nutshell.jpg"
                    }
                },
            };

            try
            {
                DetectFacesResponse detectFacesResponse = await rekognitionClient.DetectFacesAsync(detectFacesRequest);
                bool hasAll = detectFacesRequest.Attributes.Contains("ALL");
                foreach (FaceDetail face in detectFacesResponse.FaceDetails)
                {
                    FacialAnalysisData += String.Format("Confidence: {0}\nLandmarks: {1}\nPose: pitch={2} roll={3} yaw={4}\nQuality: {5}",
                        face.Confidence, face.Landmarks.Count, face.Pose.Pitch,
                        face.Pose.Roll, face.Pose.Yaw, face.Quality);

                    Console.WriteLine(FacialAnalysisData);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}