using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoParking.servicios
{
    class CustomVisionService
    {
        String PREDICTION_KEY = "4ded452c9cfd4cd7a72e35ffc5b97bab";
        String CUSTOMVISION_ENDPOINT = "https://nekliozcustomvision-prediction.cognitiveservices.azure.com/";
        String PROJECT_ID = "1ea8ca0d-8360-4200-9eef-76dfeeb02fbd";
        String PUBLISHED_NAME = "Iteration1";

        CustomVisionPredictionClient customVisionClient;

        public CustomVisionService()
        {
            customVisionClient = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials(PREDICTION_KEY))
            {
                Endpoint = CUSTOMVISION_ENDPOINT
            };
        }

        public string ComprobarVehiculo(string imageURL)
        {
            // No nos deja pasarle una URL directamente así que creamos la imagen
            ImageUrl image = new ImageUrl(imageURL);

            // Dejamos que clasifique la imagen entre "Coche" y "Moto"
            ImagePrediction resultado = customVisionClient.ClassifyImageUrl(new Guid(PROJECT_ID), PUBLISHED_NAME, image);

            // Devolvemos la predicción en primer lugar, que es la mayor.
            return resultado.Predictions[0].TagName;
        }
    }
}
