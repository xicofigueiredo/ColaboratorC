using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IHolidayValidationConsumer
    {
        public void StartHolidayConsuming(string queueName);
    }
}