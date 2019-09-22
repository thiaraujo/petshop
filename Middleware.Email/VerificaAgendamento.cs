using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;


namespace Middleware.Email
{
    public class VerificaAgendamento
    {
        public VerificaAgendamento()
        {
            RunProgramRunExample().GetAwaiter().GetResult();
        }

        private static async Task RunProgramRunExample()
        {
            // Cria uma instância do agendamento
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();

            // Inicia o serviço agendamento
            await scheduler.Start();

            // Define o job e onde ele se encontra
            var job = JobBuilder.Create<EnviarEmailJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger que cria o tipo do job e o intervalo de tempo ao qual será chamado
            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();

            // Função para executar o agendamento do job e o tipo do job
            await scheduler.ScheduleJob(job, trigger);

            // Para desligar o job
            //await scheduler.Shutdown();
        }
    }
}
