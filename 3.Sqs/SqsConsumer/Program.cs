using Amazon.SQS;
using Amazon.SQS.Model;

var cts = new CancellationTokenSource();

var SqsClient = new AmazonSQSClient();  

var queueUrlResponse = await SqsClient.GetQueueUrlAsync("customers");

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageSystemAttributeNames = new List<string> { "All"},
    MessageAttributeNames = new List<string>{"All"}                                                                                                            
};

while (!cts.IsCancellationRequested)
{
    var response = await SqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);

    foreach(var message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        await SqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(3000);
}