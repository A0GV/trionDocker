using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ApiGame.Controllers;

[ApiController]
[Route("[controller]")]
public class EnemigosController : ControllerBase
{
    private readonly string connectionString;

    public EnemigosController(string connectionString)
    {
        this.connectionString = connectionString;
    }

    [HttpGet("GetPreguntasEnemigo")]
    public IEnumerable<Enemigos> GetPreguntasEnemigo()
    {
        var preguntas = new List<Enemigos>();

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        string query = @"SELECT id_preguntaenemigo, textoPregunta, opcion1, opcion2, opcion3, opcion4, respuestaCorrecta 
                         FROM preguntasenemigo";

        using var cmd = new MySqlCommand(query, conexion);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var pregunta = new Enemigos
            {
                id_preguntaenemigo = reader.GetInt32("id_preguntaenemigo"),
                textoPregunta = reader.GetString("textoPregunta"),
                opciones = new string[]
                {
                    reader.GetString("opcion1"),
                    reader.GetString("opcion2"),
                    reader.GetString("opcion3"),
                    reader.GetString("opcion4")
                },
                respuestaCorrecta = reader.GetInt32("respuestaCorrecta")
            };

            preguntas.Add(pregunta);
        }

        return preguntas;
    }
}

