namespace BL
{
    public class Citas
    {
        public static ML.Result ConsultarCitas()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CitasAgendaContext context = new DL.CitasAgendaContext())
                {
                    var queryResult = from c in context.Cita
                                      select new
                                      {
                                          c.IdCita,
                                          c.NombreCliente,
                                          c.Fecha,
                                          c.Hora,
                                          c.DuracionMinutos,
                                          c.Estatus
                                      };

                    result.Objects = new List<object>();

                    foreach (var obj in queryResult)
                    {
                        ML.Cita cita = new ML.Cita
                        {
                            IdCita = obj.IdCita,
                            NombreCliente = obj.NombreCliente,
                            Fecha = obj.Fecha,   // ahora DateTime
                            Hora = obj.Hora,     // ahora TimeSpan
                            DuracionMinutos = obj.DuracionMinutos,
                            Estatus = obj.Estatus
                        };
                        result.Objects.Add(cita);
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result ConsultarByFecha(DateTime fecha)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CitasAgendaContext context = new DL.CitasAgendaContext())
                {
                    var queryResult = from c in context.Cita
                                      where c.Fecha.Date == fecha.Date
                                      select c;

                    result.Objects = new List<object>();

                    foreach (var obj in queryResult)
                    {
                        ML.Cita cita = new ML.Cita
                        {
                            IdCita = obj.IdCita,
                            NombreCliente = obj.NombreCliente,
                            Fecha = obj.Fecha,
                            Hora = obj.Hora,
                            DuracionMinutos = obj.DuracionMinutos,
                            Estatus = obj.Estatus
                        };
                        result.Objects.Add(cita);
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result AddCita(ML.Cita cita)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CitasAgendaContext context = new DL.CitasAgendaContext())
                {
                    if (cita.Fecha.Date < DateTime.Now.Date)
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pueden agendar citas en fechas pasadas.";
                        return result;
                    }

                    DateTime inicioNueva = cita.Fecha.Date + cita.Hora;
                    DateTime finNueva = inicioNueva.AddMinutes(cita.DuracionMinutos);

                    var citasExistentes = context.Cita
                                .Where(c => c.Estatus == "Agendada" && c.Fecha.Date == cita.Fecha.Date)
                                .ToList();

                    bool traslape = citasExistentes.Any(c =>
                    {
                        DateTime inicioExistente = c.Fecha.Date + c.Hora;
                        DateTime finExistente = inicioExistente.AddMinutes(c.DuracionMinutos);

                        return inicioNueva < finExistente && finNueva > inicioExistente;
                    });

                    if (traslape)
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El horario seleccionado se traslapa con otra cita.";
                        return result;
                    }

                    DL.Citum citaDL = new DL.Citum
                    {
                        NombreCliente = cita.NombreCliente,
                        Fecha = cita.Fecha,
                        Hora = cita.Hora,
                        DuracionMinutos = cita.DuracionMinutos,
                        Estatus = "Agendada"
                    };

                    context.Cita.Add(citaDL);
                    result.Correct = context.SaveChanges() >= 1;
                    if (!result.Correct)
                        result.ErrorMessage = "No se pudo registrar la cita.";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }


        public static ML.Result CancelarCita(int idCita)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.CitasAgendaContext context = new DL.CitasAgendaContext())
                {
                    var queryResult = context.Cita.SingleOrDefault(c => c.IdCita == idCita);

                    if (queryResult != null)
                    {
                        queryResult.Estatus = "Cancelada";

                        int validacion = context.SaveChanges();
                        result.Correct = validacion >= 1;
                        if (!result.Correct)
                        {
                            result.ErrorMessage = "No se pudo cancelar la cita.";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontró la cita.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

    }
}
