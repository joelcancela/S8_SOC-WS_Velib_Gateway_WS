# TD5

## Question 1

## Question 2

Les deux bindings sont SOAP
BasicHttpBinding (plain text)
WsHttpBinding plus sécurisé (pas transmis en plain text)

!=

webHttpBinding => REST

## Question 3

```csharp
# CustomFaultDetails.cs
[DataContract]
public class CustomFaultDetails
{
    public CustomFaultDetails(string message)
    {
        Message = message;
    }

    [DataMember]
    public string Message { get; set; }
}

# IMathsOperations.cs
[OperationContract]
[FaultContract(typeof(CustomFaultDetails))]
int Divide(int num1, int num2);


# MathsOperations.cs
public int Divide(int num1, int num2)
{
    if (num2 == 0)
    {
        CustomFaultDetails customFaultDetails = new CustomFaultDetails("Divide by zero forbidden");
        throw new FaultException<CustomFaultDetails>(customFaultDetails, new FaultReason(customFaultDetails.Message));
    }
    return (num1 / num2);
}
```

Sans FaultContract, l'exception apparaît chez le client sans aucune indication.
