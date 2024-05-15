# AWS Lambda Simple SNS Function Project

This project consists of:
* SendProcessingEmail.cs - class file containing a class with a single function handler method

The function handler (SendProcessingEmailHandler) responds to events on an Amazon SNS service, and sends an email based on the message information contained within the notification.

A trigger has been manually created via AWS Lambda dashboard that is attached to the SendProcessingEmail function, to ensure execution.

## AWS IAM Role Permissions
As this function integrates with SNS and SES, the relevent permissions/actions will be required:
* AWSLambdaBasicExecutionRole - AWS Managed - Execution of Lambda functions
* AWSElasticBeanstalkRoleSNS  - AWS Managed - Able to write to logs
* ses:SendEmail               - Custom      - Able to send emails

## TODO
Look into AWS Lambda CDK to deploy/build cloud artefacts (Function, Trigger, IAM Policies)

## Here are some steps to follow from Visual Studio:

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To configure event sources for your deployed function use the Event Sources tab in the opened Function View window.

To update the runtime configuration of your deployed function use the Configuration tab in the opened Function View window.

To view execution logs of invocations of your function use the Logs tab in the opened Function View window.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "SendProcessingEmail.Lambda/test/SendProcessingEmail.Lambda.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "SendProcessingEmail.Lambda/src/SendProcessingEmail.Lambda"
    dotnet lambda deploy-function
```
## aws-lambda-tools-defaults.json (will require creating in your project folder
```
{
  "Information": [
    "This file provides default values for the deployment wizard inside Visual Studio and the AWS Lambda commands added to the .NET Core CLI.",
    "To learn more about the Lambda commands with the .NET Core CLI execute the following command at the command line in the project root directory.",
    "dotnet lambda help",
    "All the command line options for the Lambda command can be specified in this file."
  ],
  "profile": "YOUR-AWS-PROFILE-ID",
  "region": "eu-west-2",
  "configuration": "Release",
  "function-runtime": "dotnet8",
  "function-memory-size": 512,
  "function-timeout": 30,
  "function-handler": "SendProcessingEmailLambda::SendProcessingEmailLambda.SendProcessingEmail::SendProcessingEmailHandler"
}
```

## Record structure for testing via AWS Lambda dashboard
```
{
  "Records": [
    {
      "EventSource": "aws:sns",
      "EventVersion": "1.0",
      "EventSubscriptionArn": "arn:aws:sns:eu-west-2:000000000000:TestTopic",
      "Sns": {
        "Type": "Notification",
        "MessageId": "00000000-0000-0000-0000-000000000000",
        "TopicArn": "arn:aws:sns:eu-west-2:000000000000:TestTopic",
        "Subject": "Test Event Booking",
        "Message": "{ \"eventId\": \"00000000-0000-0000-0000-000000000000\", \"eventName\": \"Wombles World Tour\", \"emailAddress\": \"someone@somewhere.com\", \"seatsBooked\": 4 }",
        "Timestamp": "1970-01-01T00:00:00.000Z",
        "SignatureVersion": "1",
        "Signature": "EXAMPLE",
        "SigningCertUrl": "EXAMPLE",
        "UnsubscribeUrl": "EXAMPLE",
        "MessageAttributes": {
          "eventId": {
            "Type": "String",
            "Value": "00000000-0000-0000-0000-000000000000"
          },
          "eventName": {
            "Type": "String",
            "Value": "Wombles World Tour"
          },
		  "emailAddress": {
		    "Type": "String",
			"Value": "someone@somewhere.com"
		  },
		  "seatsBooked": {
		    "Type": "Number",
			"Value": "4"
		  }
        }
      }
    }
  ]
}
```
