using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFireDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        
        private readonly ILogger<HangfireController> _logger;

        public HangfireController(ILogger<HangfireController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("fireandforget")]
        public String FireAndForget()
        {
            //Fire - and - Forget Job -> se ejecuta una vez
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("job fire and forget ejecutado"));

            return $"Job ID: {jobId}. Se encol� un job tipo fire and forget satisfactoriamente";
        }

        [HttpGet]
        [Route("delayedjob")]
        public String DelayedJob()
        {
            //Delayed Job -> se ejecuta una vez cuando pas� el tiempo.
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("job delayed job ejecutado"), TimeSpan.FromSeconds(10));

            return $"Job ID: {jobId}. Se encol� un job tipo delayed job satisfactoriamente";
        }

        [HttpGet]
        [Route("continuejob")]
        public String ContinuationsJob()
        {
            //Fire and Forget Job
            var parentjobId = BackgroundJob.Enqueue(() => Console.WriteLine("job fire and forget ejecutado"));

            //Continuations Job -> Se ejecuta cuando termine de ejecutarse el job padre.
            BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("continuejob ejecutado"));

            return "Se encol� un continuejob satisfactoriamente";
        }

        [HttpGet]
        [Route("recurringjob")]
        public String RecurringJobs()
        {
            //Recurring Job -> Se ejecuta cada vez qye se cumple la expresi�n cron
            RecurringJob.AddOrUpdate(() => Console.WriteLine("recurring job ejecutado"), Cron.Minutely);

            return "Se encol� un recurringjob satisfactoriamente";
        }


        private void ExecuteJob()
        {
            Console.WriteLine("job delayed job ejecutado");
        }
    }
}