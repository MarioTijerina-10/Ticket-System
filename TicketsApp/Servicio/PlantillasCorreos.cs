public class PlantillasCorreo
{
    public async Task<string> TicketCerradoParcial(int ticketId, string asunto)
    {
        return $"""
        <h2>Ticket N° {ticketId} - {asunto}</h2>
        
        <p>Ticket ha Sido Cerrado Por Tecnico - Favor de Validar Solucion.</p>

        <p>Saludos Cordiales</p>
        """;
    }

    public async Task<string> TicketCerrado(int ticketId, string asunto)
    {
        return $"""
        <h2>Ticket N° {ticketId} - {asunto}</h2>
        
        <p>Ticket ha Sido Valido y Cerrado Correctamente.</p>

        <p>Saludos Cordiales</p>
        """;
    }

    public async Task<string> TicketReasignado(int ticketId, string asunto)
    {
        return $"""
        <h2>Ticket N° {ticketId} - {asunto}</h2>
        
        <p>Ticket ha Sido Reasignado por El Usuario, Favor de Validar y Revisar</p>

        <p>Saludos Cordiales</p>
        """;
    }

    public async Task<string> TicketAsignado_Proveedor(int ticketId, string asunto)
    {
        return $"""
        <h2>Ticket N° {ticketId} - {asunto}</h2>
        
        <p>Ticket ha Sido Reasignado por El Usuario, Favor de Validar y Revisar</p>

        <p>Saludos Cordiales</p>
        """;
    }
}