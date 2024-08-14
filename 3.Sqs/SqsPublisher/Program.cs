using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var SqsClient = new AmazonSQSClient();  

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "David Pucheta",
    Email = "davidpucheta@gmail.com",
    DateOfBirth = new DateTime(1978,10,21),
    GitHubUsername = "davidpucheta"
};

var queueUrlResponse = await SqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        { "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            } 
        }
    }  
};

var response = await SqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine(response.ToString());

