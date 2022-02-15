using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoParking.servicios
{
    class AzureBlobStorage
    {
        string cadenaConexion = "DefaultEndpointsProtocol=https;AccountName=proyparkint;AccountKey=ur67FtV/k+733CqWL8+Iz7TfAcUT2yy/bXgXNB+ghTBN6Hww4d28OE4hv1wbeJYmPeUCg3HlIiN4hTL7zTuRcA==;EndpointSuffix=core.windows.net";
        string nombreContenedorBlobs = "imgparking";

        public string SubirImagen(string rutaImagen)
        {
            //Obtenemos el cliente del contenedor
            var clienteBlobService = new BlobServiceClient(cadenaConexion);
            var clienteContenedor = clienteBlobService.GetBlobContainerClient(nombreContenedorBlobs);

            //Leemos la imagen y la subimos al contenedor
            Stream streamImagen = File.OpenRead(rutaImagen);
            string nombreImagen = DateTime.Now.ToString("hh:mm:ss") + ".jpg";
            clienteContenedor.UploadBlob(nombreImagen, streamImagen);

            //Una vez subida, obtenemos la URL para referenciarla
            var clienteBlobImagen = clienteContenedor.GetBlobClient(nombreImagen);
            string urlImagen = clienteBlobImagen.Uri.AbsoluteUri;

            return urlImagen;
        }
    }
}
