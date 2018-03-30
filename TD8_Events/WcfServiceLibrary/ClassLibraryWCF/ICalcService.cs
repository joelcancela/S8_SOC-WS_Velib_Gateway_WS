using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EventsLib
{
    [ServiceContract(CallbackContract = typeof(ICalcServiceEvents))]
    public interface ICalcService
    {
        [OperationContract]
        void Calculate(int nOp, double dblNum1, double dblNum2);

        [OperationContract]
        void SubscribeCalculatedEvent();

        [OperationContract]
        void SubscribeCalculationFinishedEvent();

        [OperationContract]
        void Incr();

        [OperationContract]
        void Decr();

        [OperationContract]
        void SubscribeValueChanged();
    }
}