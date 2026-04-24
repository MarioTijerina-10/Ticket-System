using System;
using TicketsApp.Helper_Classes;

namespace TicketsApp.Interfaces;

//Interfaz para los diferentes servicios de info en la app
public interface IServicio<T>
{
    Task<List<MensajeDeRespuesta<T>>> GetAll();
    Task<T> GetOne(int id);
    Task Delete();
    Task Update();
    Task Creates();
}
