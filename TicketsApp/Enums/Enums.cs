using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TicketsApp.Models;

namespace TicketsApp.Enums;

//Clase encapsulando todos los campos enumerables para los modelos de datos
public class Enumerables
{
    public enum Unidades
    {
        Cyrburgos = 1,
        BDF = 2,
        DPC = 3,
        OilBaku = 4,
        EnergyPoint = 5
    }

    public enum Categorias
    {
        Hardware = 1,
        Software = 2,
        [Display(Name = "Redes e Internet")]
        RedesEInternet = 3,
        [Display(Name = "Sistemas ERP")]
        SistemasERP = 4
    }

    public enum Estatus
    {
        Abierto = 1,
        [Display(Name = "En Proceso")]
        EnProceso = 2,
        [Display(Name = "Cerrado Parcial")]
        CerradoParcial = 3,
        Cerrado = 4
    }

    public enum Roles
    {
        Usuario = 1,
        Analista = 2,
        Administrador = 3
    }
}
