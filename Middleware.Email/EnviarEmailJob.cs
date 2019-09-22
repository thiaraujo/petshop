using System;
using System.Threading.Tasks;
using Data.Entities.Models;
using Quartz;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

namespace Middleware.Email
{
    public class EnviarEmailJob : IJob
    {
        protected PetshopContext Db;

        // Contexto da inteface do Job e Quartz
        public Task Execute(IJobExecutionContext context)
        {
            Db = new PetshopContext();
            ExisteAgendamentoDia().GetAwaiter().GetResult();
            return null;
        }

        private async Task ExisteAgendamentoDia()
        {
            // Pega os agendados para hoje
            var agendamentos = await Db.Agendamento.Include(x => x.Cliente).Where(x => x.DiaMarcado == DateTime.Now.Date).ToListAsync();
            if (agendamentos.Any())
            {
                // Pega a hora atual
                var hora = DateTime.Now.Hour;
                agendamentos = agendamentos.Where(x => x.HoraMarcado.Hours >= hora && x.HoraMarcado.Hours <= hora).ToList();
                //Se tiver agendamentos, faz um foreach para enviar os emails
                if (agendamentos.Any())
                {
                    foreach (var item in agendamentos)
                    {
                        // Envia o email
                        var task = EnviarEmail(item.Cliente.Email, "Lembrete de agendamento", CorpoDoEmail(item.DiaMarcado.ToShortDateString(), item.HoraMarcado.ToString("g"), item.Cliente.Nome));
                        task.Wait(5000);
                    }
                }
            }
        }

        // Função que envia os e-mails
        private async Task EnviarEmail(string toEmailAddress, string emailSubject, string emailMessage)
        {
            var message = new MailMessage { From = new MailAddress("email de quem envia") };
            message.To.Add(toEmailAddress);

            message.Subject = emailSubject;
            message.Body = emailMessage;

            var smtp = new SmtpClient("smtp.google.com", 587);
            var netcred = new NetworkCredential("email da conta de quem envia", "senha da conta");
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = false;
            smtp.Credentials = netcred;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception)
            {
                return;
            }
        }

        // Cria o corpo do e-mail
        private string CorpoDoEmail(string data, string hora, string cliente)
        {
            return $"Olá sr(a). {cliente}, estamos lembrando você do agendamento para seu pet no dia {data} às {hora}.";
        }
    }
}
