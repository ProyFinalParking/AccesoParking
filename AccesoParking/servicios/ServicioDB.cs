using AccesoParking.modelo;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoParking.servicios
{
    class ServicioDB
    {
        // Propiedad de Configuración de aplicaciones con el nombre de la BBDD
        static readonly string nombreBD = Properties.Settings.Default.NombreBD;

        protected ServicioDB()
        { }

        static ServicioDB()
        {
            // TODO: Llamar el metodo 'CreateTablesIfNotExists()' en vez del siguiente:
            CreateDemoDB();
        }

        /**************************************************************************************************************************** 
         * TODO: Ver la forma de controlar excepciones de SQLite, para devolver TRUE en caso de exito y FALSE en caso de error
         * 
         * TODO: Eliminar los metodos inutilizados¿?
         * **************************************************************************************************************************/

        
        /*******************************************************
            METODOS RELACIONADOS CON EL VEHICULO
         *******************************************************/

        // Comprueba si existe la Matricula en la tabla Vehiculos y devuelve la id
        // En caso de que no exista, devuelve 0
        public static int GetVehicleId(string matricula)
        {
            int id = 0;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT id_vehiculo FROM vehiculos WHERE matricula = @matricula";

                // Se Configura el tipo de valores
                command.Parameters.Add("@matricula", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@matricula"].Value = matricula;

                // Se ejecuta el SELECT
                if (command.ExecuteScalar() != null && Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    id = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return id;
        }

        /*******************************************************
            METODOS RELACIONADOS CON EL ESTACIONAMIENTO
         *******************************************************/

        // Inserta un estacionamiento
        public static void InsertVehicleInParking(Estacionamiento e)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = "INSERT INTO estacionamientos VALUES (null, @id_vehiculo, @matricula, @entrada, @salida, @importe, @tipo)";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_vehiculo", SqliteType.Integer);
                command.Parameters.Add("@matricula", SqliteType.Text);
                command.Parameters.Add("@entrada", SqliteType.Text);
                command.Parameters.Add("@salida", SqliteType.Text);
                command.Parameters.Add("@importe", SqliteType.Real);
                command.Parameters.Add("@tipo", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@id_vehiculo"].Value = e.IdVehiculo;
                command.Parameters["@matricula"].Value = e.Matricula;
                command.Parameters["@entrada"].Value = e.Entrada;
                command.Parameters["@salida"].Value = e.Salida;
                command.Parameters["@importe"].Value = e.Importe;
                command.Parameters["@tipo"].Value = e.Tipo;

                // Se ejecuta el INSERT
                command.ExecuteNonQuery();
            }
        }
        
        // Comprueba si el vehiculo esta estacionado actualmente (no tiene fecha de salida)
        public static bool IsActiveParkedVehicle(string matricula)
        {
            bool estacionamientoActivo = false;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT COUNT(*) FROM estacionamientos 
                                        WHERE matricula = @matricula
                                        AND salida = ''";

                // Se Configura el tipo de valores
                command.Parameters.Add("@matricula", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@matricula"].Value = matricula;

                // Se ejecuta el SELECT
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    estacionamientoActivo = true;
                }
            }

            return estacionamientoActivo;
        }

        // Devuelve el numero de Coches aparcados actualmente
        public static int GetNumberParkedCars()
        {
            int cochesAparcados;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM estacionamientos WHERE salida = '' AND tipo='Coche'";

                // Se ejecuta el SELECT
                cochesAparcados = Convert.ToInt32(command.ExecuteScalar());
            }

            return cochesAparcados;
        }

        // Devuelve el numero de Motos aparcadas actualmente
        public static int GetNumberParkedMotorcycles()
        {
            int motosAparcados;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM estacionamientos WHERE salida = '' AND tipo='Moto'";

                // Se ejecuta el SELECT
                motosAparcados = Convert.ToInt32(command.ExecuteScalar());
            }

            return motosAparcados;
        }


        /*******************************************************
            METODO PARA CREAR LA BBDD EN CASO DE QUE NO EXISTA
         *******************************************************/
        private static void CreateTablesIfNotExists()
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                // Se conecta a la BBDD
                connection.Open();

                // Empieza la Trasaccion
                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = connection.CreateCommand();

                    // Tabla Clientes
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS clientes
                                        (id_cliente INTEGER PRIMARY KEY,
                                        nombre     TEXT,
                                        documento  TEXT    NOT NULL,
                                        foto       TEXT,
                                        edad       INTEGER,
                                        genero     TEXT,
                                        telefono   TEXT)";
                    command.ExecuteNonQuery();

                    // Tabla Estacionamientos
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS estacionamientos
                                        (id_estacionamiento INTEGER PRIMARY KEY,
                                        id_vehiculo        INTEGER REFERENCES vehiculos (id_vehiculo),
                                        matricula          TEXT    NOT NULL,
                                        entrada            TEXT    NOT NULL,
                                        salida             TEXT,
                                        importe            REAL,
                                        tipo               TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Tabla Marcas
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS marcas
                                        (id_marca INTEGER PRIMARY KEY,
                                        marca    TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Tabla Vehiculos
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS vehiculos
                                        (id_vehiculo INTEGER PRIMARY KEY,
                                        id_cliente  INTEGER NOT NULL
                                                            REFERENCES clientes (id_cliente),
                                        matricula   TEXT    NOT NULL,
                                        id_marca    INTEGER REFERENCES marcas (id_marca),
                                        modelo      TEXT,
                                        tipo        TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    //Commit
                    transaction.Commit();
                }
            }
        }


        public static void CreateDemoDB()
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                // Se conecta a la BBDD
                connection.Open();

                // Empieza la Trasaccion
                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText = @"DROP TABLE IF EXISTS estacionamientos";
                    command.ExecuteNonQuery();
                    command.CommandText = @"DROP TABLE IF EXISTS vehiculos";
                    command.ExecuteNonQuery();
                    command.CommandText = @"DROP TABLE IF EXISTS clientes";
                    command.ExecuteNonQuery();
                    command.CommandText = @"DROP TABLE IF EXISTS marcas";
                    command.ExecuteNonQuery();

                    // Tabla Clientes
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS clientes
                                    (id_cliente INTEGER PRIMARY KEY,
                                    nombre     TEXT,
                                    documento  TEXT    NOT NULL,
                                    foto       TEXT,
                                    edad       INTEGER,
                                    genero     TEXT,
                                    telefono   TEXT)";
                    command.ExecuteNonQuery();

                    // Tabla Estacionamientos
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS estacionamientos
                                    (id_estacionamiento INTEGER PRIMARY KEY,
                                    id_vehiculo        INTEGER REFERENCES vehiculos (id_vehiculo),
                                    matricula          TEXT    NOT NULL,
                                    entrada            TEXT    NOT NULL,
                                    salida             TEXT,
                                    importe            REAL,
                                    tipo               TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Tabla Marcas
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS marcas
                                    (id_marca INTEGER PRIMARY KEY,
                                    marca    TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Tabla Vehiculos
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS vehiculos
                                    (id_vehiculo INTEGER PRIMARY KEY,
                                    id_cliente  INTEGER NOT NULL
                                                        REFERENCES clientes (id_cliente),
                                    matricula   TEXT    NOT NULL,
                                    id_marca    INTEGER REFERENCES marcas (id_marca),
                                    modelo      TEXT,
                                    tipo        TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Datos Clientes
                    command.CommandText = @"INSERT INTO 'clientes' ('id_cliente','nombre','documento','foto','edad','genero','telefono') 
                        VALUES (0,'No Registrado','0','',0,'',''),
                                (1,'Acevedo Manríquez María Mireya','12358496A','URL Imagen 1',25,'Femenino','956124578'),
                                (2, 'Aguilar Lorantes Irma', '32659815B', 'URL 2', 33, 'Femenino', '965234589'),
                                (3, 'Alcoverde Martínez Roberto Antonio', '95123678C', 'URL 3', 49, 'Masculino', '654128935'),
                                (4, 'Alvarado Mendoza Oscar', '56489520D', 'URL4', 65, 'Masculino', '784629514'),
                                (5, 'Serina Byrd', '65841523H', 'URL 5', 62, 'Femenino', '645127895'),
                                (6, 'Ursa Mcdowell', '62845951K', 'URL 6', 33, 'Femenino', '964513498'),
                                (7, 'Channing Melton', '98156343O', 'URL 7', 54, 'Femenino', '651284231'),
                                (8, 'Chantale Barrera', '83492154Y', 'URL 8', 24, 'Femenino', '679512354'),
                                (9, 'Jonah Quinn', '65124578T', 'URL 9', 45, 'Masculino', '635951555'),
                                (10, 'John Daniels', '95152648H', 'URL 10', 25, 'Masculino', '632145791'),
                                (11, 'Alexander Lancaster', '01324807G', 'URL 11', 34, 'Masculino', '654123951'),
                                (12, 'Hollee Pratt', '96234584R', 'URL 12', 41, 'Femenino', '653753159'),
                                (13, 'Fernando Alonso', '95123648J', 'URL', 45, 'Masculino', '654789512'),
                                (14, 'Juan Antonio Gonzalez Rodriguez', '26154859G', 'Foto', 44, 'Masculino', '651234895')";
                    command.ExecuteNonQuery();

                    // Datos Marcas
                    command.CommandText = @"INSERT INTO 'marcas' ('id_marca','marca') VALUES (0,'No Registrado'), (1,'BMW'),
                        (2, 'Citroen'), (3, 'Renault'), (4, 'Mercedez'), (5, 'Peugeot'), (6, 'Audi'), (7, 'Range Rover'),
                        (8, 'Opel'), (9, 'Hyundai'), (10, 'Ford'), (11, 'Fiat'), (12, 'Jeep'), (13, 'Lexus'), (14, 'MINI'),
                        (15, 'SEAT'), (16, 'Subaru'), (17, 'Mitsubishi'), (18, 'Nissan'), (19, 'Skoda'), (20, 'Porsche'),
                        (21, 'Smart'), (22, 'Tesla'), (23, 'Toyota'), (24, 'Volvo'), (25, 'Volkswagen'), (26, 'SsangYong'),
                        (27, 'MG'), (28, 'Mazda'), (29, 'Land Rover'), (30, 'Jaguar'),(31, 'Dacia'), (32, 'Ferrari'), (33, 'Bentley'),
                        (34, 'Aston Martin'), (35, 'Bugatti'), (36, 'Yamaha'), (37, 'Suzuki'), (38, 'Honda'), (39, 'Aprilia'), (40, 'KTM')";
                    command.ExecuteNonQuery();

                    // Datos Vehiculos
                    command.CommandText = @"INSERT INTO 'vehiculos' ('id_vehiculo','id_cliente','matricula','id_marca','modelo','tipo')
                            VALUES (0,0,'No Registrado',0,'','Generico'),
                                (1,0,'Eliminado',0,'','Generico'),
                                (2, 2, '45778KYB', 2, 'C3', 'Coche'),
                                (3, 11, '4595HHY', 36, 'R1', 'Moto'),
                                (4, 14, '9543YAC', 19, 'Fabia', 'Coche'),
                                (5, 8, '3215KPE', 37, 'Hayabusa', 'Moto'),
                                (6, 6, '9435ODS', 22, 'Model S', 'Coche'),
                                (7,1,'2648KHY',1,'Serie 3','Coche'),
                                (8,1,'5564KIK',1,'Serie 5','Coche'),
                                (9, 8, '8899KIO', 1, 'Serie 1', 'Coche'),
                                (10,0,'',0,'','Moto')";
                    command.ExecuteNonQuery();

                    // Datos Estacionamientos
                    command.CommandText = @"INSERT INTO 'estacionamientos' ('id_estacionamiento','id_vehiculo','matricula','entrada','salida','importe','tipo') 
                            VALUES (1,0,'0295GTS','02/02/2022 1:00:33','',0.0,'Coche'),
                                (2, 0, '9315KNM', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (3, 0, '7854HAL', '02/02/2022 1:00:33', '', 0.0, 'Moto'),
                                (4, 1, '*******', '02/02/2022 1:00:33', '07/02/2022 17:20:09', 252.63, 'Coche'),
                                (5, 0, '3694UDM', '02/02/2022 1:00:33', '', 0.0, 'Moto'),
                                (6, 0, '7812MNB', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (7, 4, '9543YAC', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (8, 0, '9764ASD', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (9, 0, '3278KLJ', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (10, 5, '3215KPE', '02/02/2022 1:00:33', '', 0.0, 'Moto'),
                                (11, 0, '8528FTJ', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (12, 0, '6542DTJ', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (13, 7, '2648KHY', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (14, 0, '9644JJH', '02/02/2022 1:00:33', '', 0.0, 'Coche')";
                    command.ExecuteNonQuery();

                    //Commit
                    transaction.Commit();
                }

            }
        }
    }
}
