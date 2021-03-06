# WS GENERIC DB

Esta es la versión en C# de la librería .Net para conexión a base de datos, inicialmente, pensada para conectar a SQL Server, pero puede ser adaptada para cualquier RDBMS (Relational database management system).

Esta librería utiliza un conjunto de método fijos que automatizan las operaciones CRUD con un motor de base de datos. Está basada en el uso de procedimientos almacenados, por lo tanto, es una condición para su utilización que la base de datos con la cual trabajen permita este tipo de objetos.

# Métodos

- public Byte[] CONSULTA_GENERICA_SP_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, string vConnectionString)
    * Parámetros:
        + Byte[] NOMBRESP: Nombre del procedimiento almacenado.
        + Byte[] XMLParams: conjunto de parámetros del procedimiento pasado en formato XML.
        + string vConnectionString: cadena de conexión a la base de datos.
    * Objetivo: 
        + Este método espera que el procedimento retorne uno o varios registros de datos (DataSet) para ser operados en las apliaciones que realizan el llamado.
      
- public Byte[] CONSULTA_GENERICA_SP2_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, string vConnectionString)
    * Parámetros:
        + Byte[] NOMBRESP: Nombre del procedimiento almacenado.
        + Byte[] XMLParams: conjunto de parámetros del procedimiento pasado en formato XML.
        + string vConnectionString: cadena de conexión a la base de datos.
    * Objetivo: 
        + Este método espera que el procedimento retorne un registro de datos (DataSet) base que indica si la operación de modificación en la base de datos (INSERT, UPDATE, DELETE) se ha realizado con éxito, luego la salida del método a esperar es una cadena de texto que incluye el texto "OK -", si es así la operación se realizó en forma exitosa, de lo contrario, se devlverá como salida un texto que indica el error presentado, indicando nombre de procedimento, código de error, línea del error.

- public Byte[] CONSULTA_GENERICA_SP3_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, Byte[] XMLDetalle, string vConnectionString)
    * Parámetros:
        + Byte[] NOMBRESP: Nombre del procedimiento almacenado.
        + Byte[] XMLParams: conjunto de parámetros del procedimiento pasado en formato XML.
        + Byte[] XMLDetalle: parámetro en formato XML, que se usa para representar el paso de un conjunto octogonal de datos (filas, columnas), un ejemplo de esto, serían el detalle de una factura.
        + string vConnectionString: cadena de conexión a la base de datos.
    * Objetivo: 
        + Este método espera que el procedimento retorne un registro de datos (DataSet) base que indica si la operación de modificación en la base de datos (INSERT, UPDATE, DELETE) se ha realizado con éxito, luego la salida del método a esperar es una cadena de texto que incluye el texto "OK -", si es así la operación se realizó en forma exitosa, de lo contrario, se devlverá como salida un texto que indica el error presentado, indicando nombre de procedimento, código de error, línea del error.

- public Byte[] CONSULTA_GENERICA_SP4_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, Byte[] XMLDetalle, Byte[] ObjetoBinario, string vConnectionString)
    * Parámetros:
        + Byte[] NOMBRESP: Nombre del procedimiento almacenado.
        + Byte[] XMLParams: conjunto de parámetros del procedimiento pasado en formato XML.
        + Byte[] XMLDetalle: parámetro en formato XML, que se usa para representar el paso de un conjunto octogonal de datos (filas, columnas), un ejemplo de esto, serían el detalle de una factura.
        + Byte[] ObjetoBinario: parámetros que representa un objeto binario, este puede ser cualquier tipo de archivo, por ejemplo, una imagen, un documento PDF, u otro que se desee almacenar en una base de datos.
        + string vConnectionString: cadena de conexión a la base de datos.
    * Objetivo: 
        + Este método espera que el procedimento retorne un registro de datos (DataSet) base que indica si la operación de modificación en la base de datos (INSERT, UPDATE, DELETE) se ha realizado con éxito, luego la salida del método a esperar es una cadena de texto que incluye el texto "OK -", si es así la operación se realizó en forma exitosa, de lo contrario, se devlverá como salida un texto que indica el error presentado, indicando nombre de procedimento, código de error, línea del error.

- public Boolean CargaParametrosSP(SqlCommand cmd, String XMLParams)
    * Parámetros:
        + SqlCommand cmd: Objeto de tipo Command al que se le asignó un procedimiento almacenado.
        + String XMLParams: conjunto de parámetros del procedimiento pasado en formato XML.
        
- public Byte[] Serialize(object Obj, Boolean AsByte)
    * Parámetros:
        + object Obj: objeto al que se aplica serialización.
        + Boolean AsByte: indica si el parámetro se está pasando como tipo Byte[].
        
- public object Deserialize(Byte[] Obj)
    * Parámetros:
        + object Obj: objeto serializado al que se aplica deserialización para devolverlo a su formato original.
        
# Formato de procedimientos almacenados requerido WS_GENERIC_DB (ejemplos)
    * Para devolución de DataSet
    
      CREATE PROCEDURE [dbo].[nombre_procedimiento_almacenado]
         @param1 as varchar(8),
         @param2 as varchar(5)
      AS
      ----------------------------------------------------------
      --AUTOR		: IVAN SALDIVAR RODRIGUEZ
      --FECHA		: 2017-11-03
      --OBJETIVO	: 
      ----------------------------------------------------------
      BEGIN
         BEGIN TRY
            SET NOCOUNT ON;

            SELECT     
               campo1, 
               campo2
            FROM [dbo].[tabla1]
            WHERE 
               (llave1 = @param1) 
            AND (llave2 = @param2);

            SET NOCOUNT OFF;
         END TRY	
         BEGIN CATCH
            SELECT  
               -1 AS REGISTRO_ACTUALIZADO, 
               ERROR_PROCEDURE() AS ERROR_PROCEDURE_,  
               ERROR_NUMBER() AS ERROR_NUMBER_,  
               ERROR_MESSAGE() AS ERROR_MESSAGE_,  
               ERROR_LINE() AS ERROR_LINE_;
			
            RETURN 0;
         END CATCH
         RETURN 1;
    END
    GO
    
    * Para operaciones de actualización de base de datos (INSERT, UPDATE, DELETE)

      CREATE PROCEDURE [dbo].[nombre_procedimiento_almacenado_CRUD]
         @param1 as varchar(8),
         @param2 as varchar(5)
      AS
      ----------------------------------------------------------
      --AUTOR		: IVAN SALDIVAR RODRIGUEZ
      --FECHA		: 2017-11-03
      --OBJETIVO	: 
      ----------------------------------------------------------
      BEGIN
         BEGIN TRY
            SET NOCOUNT ON;
            DECLARE @ident as int;
	    
            INSER INTO [dbo].[tabla1]( campo1,  campo2 )
            VALUES(@param1, @param2);

            SET @ident= @@IDENTITY;
		
	    --se puede devolver un campo llave o un valor positivo
	    --específico que sirva para determinar un comportamiento dado.
            SELECT  
               @ident AS REGISTRO_ACTUALIZADO, 
               '' AS ERROR_PROCEDURE_,  
               0 AS ERROR_NUMBER_,  
               '' AS ERROR_MESSAGE_,  
               0 AS ERROR_LINE_;

            SET NOCOUNT OFF;
         END TRY	
         BEGIN CATCH
            SELECT  
               -1 AS REGISTRO_ACTUALIZADO, 
               ERROR_PROCEDURE() AS ERROR_PROCEDURE_,  
               ERROR_NUMBER() AS ERROR_NUMBER_,  
               ERROR_MESSAGE() AS ERROR_MESSAGE_,  
               ERROR_LINE() AS ERROR_LINE_;
			
            RETURN 0;
         END CATCH
         RETURN 1;
    END
    GO
