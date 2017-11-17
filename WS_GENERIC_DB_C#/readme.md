# WS GENERIC DB

Esta es la versión en C# de la librería .Net para conexión a base de datos, inicialmente, pensada para conectar a SQL Server, pero puede ser adaptada para cualquier RDBMS (Relational database management system).

Esta librería utiliza un conjunto de método fijos que automatizan las operaciones CRUD con un motor de base de datos. Está basada en el uso de procedimientos almacenados, por lo tanto, es una condición para su utilización que la base de datos con la cual trabajen permita este tipo de objetos.

# Métodos

- public Byte[] CONSULTA_GENERICA_SP_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, string vConnectionString)
        Byte[] NOMBRESP: Nombre del procedimiento almacenado.
        Byte[] XMLParams: conjunto de parámetros del procedimiento pasado en formato XML.
        string vConnectionString: cadena de conexión a la base de datos.
      
- public Byte[] CONSULTA_GENERICA_SP2_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, string vConnectionString)
- public Byte[] CONSULTA_GENERICA_SP3_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, Byte[] XMLDetalle, string vConnectionString)
- public Byte[] CONSULTA_GENERICA_SP4_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, Byte[] XMLDetalle, Byte[] ObjetoBinario, string vConnectionString)
- public Boolean CargaParametrosSP(SqlCommand cmd, String XMLParams)
- public Byte[] Serialize(object Obj, Boolean AsByte)
- public object Deserialize(Byte[] Obj)
