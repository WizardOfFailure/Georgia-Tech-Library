using System.Diagnostics.Metrics;
using Prometheus;

namespace PrometheusMonitoring
{
    public class PerformanceMetricsService
    {
        private readonly Counter _requestCounter;



        public PerformanceMetricsService()

        {

            // Custom metric for counting specific requests

            _requestCounter = Metrics.CreateCounter("custom_request_count", "Counts specific requests");

        }



        public void RecordRequest()

        {

            _requestCounter.Inc();

        }
    }
}
