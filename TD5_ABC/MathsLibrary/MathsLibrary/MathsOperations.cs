using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MathsLibrary
{
    public class MathsOperations : IMathsOperations
    {
        public int Add(int num1, int num2)
        {
            return (num1 + num2);
        }
        public int Multiply(int num1, int num2)
        {
            return (num1 * num2);
        }

        public int Divide(int num1, int num2)
        {
            if (num2 == 0)
            {
                CustomFaultDetails customFaultDetails = new CustomFaultDetails("Divide by zero forbidden");
                throw new FaultException<CustomFaultDetails>(customFaultDetails, new FaultReason(customFaultDetails.Message));
            }
            return (num1 / num2);
        }
    }
}
