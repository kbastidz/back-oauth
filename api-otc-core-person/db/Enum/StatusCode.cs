namespace db.Enum
{
    public enum StatusCode
    {
        Success = 200,        // Operación exitosa
        BadRequest = 400,     // Solicitud malformada
        Unauthorized = 401,   // No autenticado
        Forbidden = 403,      // No autorizado
        NotFound = 404,       // Recurso no encontrado
        InternalServerError = 500, // Error interno del servidor

        A = 'A'
    }


}
