using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TicketsApp.Models;

namespace TicketsApp.Enums;

//Clase encapsulando todos los campos enumerables para los modelos de datos
public class Enumerables
{
    public enum Estatus
    {
        Abierto = 1,
        [Display(Name = "En Proceso")]
        EnProceso = 2,
        [Display(Name = "Cerrado Parcial")]
        CerradoParcial = 3,
        Cerrado = 4
    }
}
