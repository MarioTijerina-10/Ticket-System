using Microsoft.AspNetCore.Components;

public class ComponenteBase : ComponentBase
{

    //  Funcion para los  valores de filtro, el parametro asignador es la accion de asignacion que se lleva acabo 
    protected async Task AsignarValorFiltro<T>(
       T valor,
       Action<T> asignador)
    {
        asignador(valor);
        await filtrar();
    }

    protected virtual async Task filtrar() { }
}