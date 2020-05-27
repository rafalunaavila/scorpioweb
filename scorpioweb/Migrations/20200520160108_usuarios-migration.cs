using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace scorpioweb.Migrations
{
    public partial class usuariosmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "abandonoestado",
                columns: table => new
                {
                    idAbandonoEstado = table.Column<int>(type: "int(11)", nullable: false),
                    Cuantos = table.Column<int>(type: "int(5)", nullable: true),
                    DocumentacionSalirPais = table.Column<string>(maxLength: 2, nullable: true),
                    FamiliaresFuera = table.Column<string>(maxLength: 2, nullable: true),
                    LugaresViaje = table.Column<string>(maxLength: 200, nullable: true),
                    LugaresVivido = table.Column<string>(maxLength: 200, nullable: true),
                    MotivoViaje = table.Column<string>(maxLength: 200, nullable: true),
                    MotivoVivido = table.Column<string>(maxLength: 200, nullable: true),
                    Pasaporte = table.Column<string>(maxLength: 2, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    TiempoViaje = table.Column<string>(maxLength: 100, nullable: true),
                    TiempoVivido = table.Column<string>(maxLength: 100, nullable: true),
                    ViajaHabitual = table.Column<string>(maxLength: 2, nullable: true),
                    VISA = table.Column<string>(maxLength: 2, nullable: true),
                    VividoFuera = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abandonoestado", x => x.idAbandonoEstado);
                });

            migrationBuilder.CreateTable(
                name: "actividadsocial",
                columns: table => new
                {
                    idActividadSocial = table.Column<int>(type: "int(11)", nullable: false),
                    Horario = table.Column<string>(maxLength: 100, nullable: true),
                    Lugar = table.Column<string>(maxLength: 150, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 300, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    Referencia = table.Column<string>(maxLength: 100, nullable: true),
                    SePuedeEnterar = table.Column<string>(maxLength: 2, nullable: true),
                    Telefono = table.Column<string>(maxLength: 10, nullable: true),
                    TipoActividad = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actividadsocial", x => x.idActividadSocial);
                });

            migrationBuilder.CreateTable(
                name: "asientofamiliar",
                columns: table => new
                {
                    idAsientoFamiliar = table.Column<int>(type: "int(11)", nullable: false),
                    Dependencia = table.Column<string>(maxLength: 2, nullable: true),
                    DependenciaExplica = table.Column<string>(maxLength: 250, nullable: true),
                    Domicilio = table.Column<string>(maxLength: 200, nullable: true),
                    Edad = table.Column<int>(type: "int(11)", nullable: true),
                    EnteradoProceso = table.Column<string>(maxLength: 2, nullable: true),
                    HorarioLocalizacion = table.Column<string>(maxLength: 200, nullable: true),
                    Nombre = table.Column<string>(maxLength: 150, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 250, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    PuedeEnterarse = table.Column<string>(maxLength: 2, nullable: true),
                    Relacion = table.Column<string>(maxLength: 45, nullable: true),
                    Sexo = table.Column<string>(maxLength: 1, nullable: true),
                    Telefono = table.Column<string>(maxLength: 10, nullable: true),
                    TiempoHabitando = table.Column<string>(maxLength: 45, nullable: true),
                    Tipo = table.Column<string>(maxLength: 45, nullable: true),
                    VivenJuntos = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asientofamiliar", x => x.idAsientoFamiliar);
                });

            migrationBuilder.CreateTable(
                name: "consumosustancias",
                columns: table => new
                {
                    idConsumoSustancias = table.Column<int>(type: "int(11)", nullable: false),
                    Cantidad = table.Column<string>(maxLength: 45, nullable: true),
                    Consume = table.Column<string>(maxLength: 45, nullable: true),
                    Frecuencia = table.Column<string>(maxLength: 45, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 400, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    Sustancia = table.Column<string>(maxLength: 45, nullable: true),
                    UltimoConsumo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumosustancias", x => x.idConsumoSustancias);
                });

            migrationBuilder.CreateTable(
                name: "domicilio",
                columns: table => new
                {
                    idDomicilio = table.Column<int>(type: "int(11)", nullable: false),
                    Calle = table.Column<string>(maxLength: 200, nullable: true),
                    CP = table.Column<string>(maxLength: 45, nullable: true),
                    DomcilioSecundario = table.Column<string>(maxLength: 45, nullable: true),
                    Estado = table.Column<string>(maxLength: 45, nullable: true),
                    Horario = table.Column<string>(maxLength: 100, nullable: true),
                    Municipio = table.Column<string>(maxLength: 45, nullable: true),
                    No = table.Column<string>(maxLength: 45, nullable: true),
                    NombreCF = table.Column<string>(maxLength: 45, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 400, nullable: true),
                    Pais = table.Column<string>(maxLength: 45, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    Referencias = table.Column<string>(maxLength: 300, nullable: true),
                    ResidenciaHabitual = table.Column<string>(maxLength: 45, nullable: true),
                    Temporalidad = table.Column<string>(maxLength: 45, nullable: true),
                    TipoDomicilio = table.Column<string>(maxLength: 45, nullable: true),
                    TipoUbicacion = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domicilio", x => x.idDomicilio);
                });

            migrationBuilder.CreateTable(
                name: "domiciliosecundario",
                columns: table => new
                {
                    idDomicilioSecundario = table.Column<int>(type: "int(11)", nullable: false),
                    Calle = table.Column<string>(maxLength: 200, nullable: true),
                    CP = table.Column<string>(maxLength: 45, nullable: true),
                    Estado = table.Column<string>(maxLength: 45, nullable: true),
                    Horario = table.Column<string>(maxLength: 100, nullable: true),
                    idDomicilio = table.Column<int>(type: "int(11)", nullable: false),
                    Motivo = table.Column<string>(maxLength: 100, nullable: true),
                    Municipio = table.Column<string>(maxLength: 45, nullable: true),
                    No = table.Column<string>(maxLength: 45, nullable: true),
                    NombreCF = table.Column<string>(maxLength: 100, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 300, nullable: true),
                    Pais = table.Column<string>(maxLength: 45, nullable: true),
                    Referencias = table.Column<string>(maxLength: 300, nullable: true),
                    ResidenciaHabitual = table.Column<string>(maxLength: 2, nullable: true),
                    Temporalidad = table.Column<string>(maxLength: 45, nullable: true),
                    TipoDomicilio = table.Column<string>(maxLength: 45, nullable: true),
                    TipoUbicacion = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domiciliosecundario", x => x.idDomicilioSecundario);
                });

            migrationBuilder.CreateTable(
                name: "estados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    estado = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "estudios",
                columns: table => new
                {
                    idEstudios = table.Column<int>(type: "int(11)", nullable: false),
                    Direccion = table.Column<string>(maxLength: 150, nullable: true),
                    Estudia = table.Column<string>(maxLength: 45, nullable: true),
                    GradoEstudios = table.Column<string>(maxLength: 45, nullable: true),
                    Horario = table.Column<string>(maxLength: 100, nullable: true),
                    InstitucionE = table.Column<string>(maxLength: 150, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 200, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    Telefono = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estudios", x => x.idEstudios);
                });

            migrationBuilder.CreateTable(
                name: "familiaresforaneos",
                columns: table => new
                {
                    idFamiliaresForaneos = table.Column<int>(type: "int(11)", nullable: false),
                    Edad = table.Column<int>(type: "int(11)", nullable: true),
                    EnteradoProceso = table.Column<string>(maxLength: 2, nullable: true),
                    Estado = table.Column<string>(maxLength: 70, nullable: true),
                    FrecuenciaContacto = table.Column<string>(maxLength: 45, nullable: true),
                    Nombre = table.Column<string>(maxLength: 150, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 200, nullable: true),
                    Pais = table.Column<string>(maxLength: 45, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    PuedeEnterarse = table.Column<string>(maxLength: 2, nullable: true),
                    Relacion = table.Column<string>(maxLength: 45, nullable: true),
                    Sexo = table.Column<string>(maxLength: 1, nullable: true),
                    Telefono = table.Column<string>(maxLength: 10, nullable: true),
                    TiempoConocerlo = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_familiaresforaneos", x => x.idFamiliaresForaneos);
                });

            migrationBuilder.CreateTable(
                name: "firmas",
                columns: table => new
                {
                    idfirmas = table.Column<int>(type: "int(11)", nullable: false),
                    codigo = table.Column<string>(maxLength: 500, nullable: true),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: true),
                    libro = table.Column<string>(maxLength: 45, nullable: true),
                    nombre = table.Column<string>(maxLength: 200, nullable: true),
                    sexo = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firmas", x => x.idfirmas);
                });

            migrationBuilder.CreateTable(
                name: "municipios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    estados_id = table.Column<int>(type: "int(11)", nullable: false),
                    municipio = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_municipios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    Alias = table.Column<string>(maxLength: 45, nullable: true),
                    Celular = table.Column<string>(maxLength: 45, nullable: true),
                    ConsumoSustancias = table.Column<string>(maxLength: 45, nullable: true),
                    CURP = table.Column<string>(maxLength: 45, nullable: true),
                    DatosGeneralescol = table.Column<string>(maxLength: 100, nullable: true),
                    Duracion = table.Column<string>(maxLength: 45, nullable: true),
                    Edad = table.Column<int>(type: "int(11)", nullable: true),
                    EspecifiqueIdioma = table.Column<string>(maxLength: 45, nullable: true),
                    EspecifiqueTraductor = table.Column<string>(maxLength: 100, nullable: true),
                    EstadoCivil = table.Column<string>(maxLength: 45, nullable: true),
                    FNacimiento = table.Column<DateTime>(type: "datetime", nullable: true),
                    Genero = table.Column<string>(maxLength: 45, nullable: true),
                    Hijos = table.Column<string>(maxLength: 45, nullable: true),
                    LeerEscribir = table.Column<string>(maxLength: 45, nullable: true),
                    LNEstado = table.Column<string>(maxLength: 45, nullable: true),
                    LNLocalidad = table.Column<string>(maxLength: 45, nullable: true),
                    LNMunicipio = table.Column<string>(maxLength: 45, nullable: true),
                    LNPais = table.Column<string>(maxLength: 45, nullable: true),
                    Materno = table.Column<string>(maxLength: 45, nullable: true),
                    NHijos = table.Column<int>(type: "int(11)", nullable: true),
                    Nombre = table.Column<string>(maxLength: 100, nullable: true),
                    NPersonasVive = table.Column<int>(type: "int(11)", nullable: true),
                    OtroIdioma = table.Column<string>(maxLength: 45, nullable: true),
                    Paterno = table.Column<string>(maxLength: 45, nullable: true),
                    Propiedades = table.Column<string>(maxLength: 45, nullable: true),
                    TelefonoFijo = table.Column<string>(maxLength: 45, nullable: true),
                    Traductor = table.Column<string>(maxLength: 45, nullable: true),
                    UltimaActualización = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona", x => x.idPersona);
                });

            migrationBuilder.CreateTable(
                name: "proceso",
                columns: table => new
                {
                    idProceso = table.Column<int>(type: "int(11)", nullable: false),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    CausaPenal = table.Column<string>(maxLength: 50, nullable: true),
                    Coimputados = table.Column<string>(maxLength: 45, nullable: true),
                    CoimputadosSupervision = table.Column<string>(maxLength: 45, nullable: true),
                    EspecificaFuncionario = table.Column<string>(maxLength: 100, nullable: true),
                    EspecificaRelacion = table.Column<string>(maxLength: 100, nullable: true),
                    Funcionario = table.Column<string>(maxLength: 45, nullable: true),
                    NoCoimputadosSupervision = table.Column<int>(type: "int(11)", nullable: true),
                    Observaciones = table.Column<string>(maxLength: 100, nullable: true),
                    RelacionLugar = table.Column<string>(maxLength: 45, nullable: true),
                    RelacionSupervisado = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proceso", x => new { x.idProceso, x.Persona_idPersona });
                });

            migrationBuilder.CreateTable(
                name: "saludfisica",
                columns: table => new
                {
                    idSaludFisica = table.Column<int>(type: "int(11)", nullable: false),
                    Discapacidad = table.Column<string>(maxLength: 2, nullable: true),
                    EmbarazoLactancia = table.Column<string>(maxLength: 2, nullable: true),
                    Enfermedad = table.Column<string>(maxLength: 2, nullable: true),
                    EspecifiqueDiscapacidad = table.Column<string>(maxLength: 100, nullable: true),
                    EspecifiqueEnfermedad = table.Column<string>(maxLength: 100, nullable: true),
                    EspecifiqueServicioMedico = table.Column<string>(maxLength: 45, nullable: true),
                    InstitucionServicioMedico = table.Column<string>(maxLength: 45, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 200, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    ServicioMedico = table.Column<string>(maxLength: 45, nullable: true),
                    Tiempo = table.Column<string>(maxLength: 100, nullable: true),
                    Tratamiento = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saludfisica", x => x.idSaludFisica);
                });

            migrationBuilder.CreateTable(
                name: "trabajo",
                columns: table => new
                {
                    idTrabajo = table.Column<int>(type: "int(11)", nullable: false),
                    Direccion = table.Column<string>(maxLength: 150, nullable: true),
                    EmpledorJefe = table.Column<string>(maxLength: 100, nullable: true),
                    EnteradoProceso = table.Column<string>(maxLength: 2, nullable: true),
                    Horario = table.Column<string>(maxLength: 100, nullable: true),
                    Observaciones = table.Column<string>(maxLength: 300, nullable: true),
                    Persona_idPersona = table.Column<int>(type: "int(11)", nullable: false),
                    Puesto = table.Column<string>(maxLength: 45, nullable: true),
                    Salario = table.Column<string>(maxLength: 45, nullable: true),
                    SePuedeEnterar = table.Column<string>(maxLength: 2, nullable: true),
                    Telefono = table.Column<string>(maxLength: 45, nullable: true),
                    TemporalidadSalario = table.Column<string>(maxLength: 45, nullable: true),
                    TiempoTrabajano = table.Column<string>(maxLength: 45, nullable: true),
                    TipoOcupacion = table.Column<string>(maxLength: 150, nullable: true),
                    Trabaja = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trabajo", x => x.idTrabajo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "abandonoestado");

            migrationBuilder.DropTable(
                name: "actividadsocial");

            migrationBuilder.DropTable(
                name: "asientofamiliar");

            migrationBuilder.DropTable(
                name: "consumosustancias");

            migrationBuilder.DropTable(
                name: "domicilio");

            migrationBuilder.DropTable(
                name: "domiciliosecundario");

            migrationBuilder.DropTable(
                name: "estados");

            migrationBuilder.DropTable(
                name: "estudios");

            migrationBuilder.DropTable(
                name: "familiaresforaneos");

            migrationBuilder.DropTable(
                name: "firmas");

            migrationBuilder.DropTable(
                name: "municipios");

            migrationBuilder.DropTable(
                name: "persona");

            migrationBuilder.DropTable(
                name: "proceso");

            migrationBuilder.DropTable(
                name: "saludfisica");

            migrationBuilder.DropTable(
                name: "trabajo");
        }
    }
}
