using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoParking.servicios
{
    class ComputerVisionService
    {
        String COMPUTERVISION_SUSCRIPTION_KEY = "6c5ab42596404d5ea2cd379b22ddd940";
        String COMPUTERVISION_ENDPOINT = "https://nekliozcomputervision.cognitiveservices.azure.com/";

        ComputerVisionClient computerVisionClient;

        public ComputerVisionService()
        {
            computerVisionClient = ComputerVisionAuthenticate(COMPUTERVISION_ENDPOINT, COMPUTERVISION_SUSCRIPTION_KEY);
        }

        public string GetMatricula(string urlFile)
        {
            // Carga el archivo
            var textHeaders = computerVisionClient.ReadAsync(urlFile).Result;
            string operationLocation = textHeaders.OperationLocation;

            // Solo necesitamos el ID de la URL, quitamos lo demás
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Dejamos que encuentre todos los datos necesarios
            ReadOperationResult results;
            do
            {
                results = computerVisionClient.GetReadResultAsync(Guid.Parse(operationId)).Result;
            }
            while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

            // Lista de valores devueltos por el servicio, el primero es la matricula
            List<string> lista = new List<string>();

            // Pasamos los datos encontrados a string
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    lista.Add(line.Text);
                }
            }

            string matricula = lista[0];

            return matricula;
        }

        private static ComputerVisionClient ComputerVisionAuthenticate(string endpoint, string key)
        {
            return new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }
    }
}
