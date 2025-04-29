using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;


namespace ApiGame.Controllers;
[ApiController]
[Route("[controller]")]
public class leaderboardController : ControllerBase
{
    private readonly string connectionString;

    public leaderboardController(string connectionString)
    {
        this.connectionString = connectionString;
    }

    [HttpGet("GetLeaders")]
    public IEnumerable<Leadearboard> GetLeaders()
    {
        var personas = new List<Leadearboard>();
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand(
            @"select u.nombre, u.apellido_pat, sum(h.exp) as `exp`  from usuario u 
            join usuario_historial uh on uh.id_usuario = u.id_usuario
            join historial h on h.id_historial = uh.id_historial
            group by u.id_usuario, u.nombre, u.apellido_pat
            order by `exp` desc
            limit 5;", conexion);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var per = new Leadearboard(
                reader.GetString("nombre"),
                reader.GetString("apellido_pat"),
                reader.GetInt32("exp")
            );
            personas.Add(per);
        }
        return personas;

    }
}