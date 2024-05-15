# AWS Lambda Function

## Description
This repository contains an AWS Lambda function designed to subscribe to an SNS (Simple Notification Service) topic. The function deserializes event booking JSON data, retrieves event capacity from a database, and verifies if there is enough seating capacity for the booking.

## Approach
The goal was to keep the Lambda function lightweight and minimize dependencies to ensure it remains quick and efficient. The function makes a single database call to check the capacity. To maintain a clean structure, the repository logic is separated from the main function using an interface, facilitating easier unit testing.

## Technologies
- ASP.NET Core 8
- C#
- AWS Lambda
- AWS SNS (Simple Notification Service)
- MongoDB

## Features
- Use specific names to enable communication with other system parts.
  - Lambda Function Name in AWS: `CheckEventCapacity`
  - SNS Topic to be subscribed to: `Event-Publish`

## Installation

### Prerequisites
- MongoDB Atlas database set up as a cluster
- AWS IAM account

### Instructions

#### 1. Database Setup
1. Create a database with the following structure: `Id`, `capacity`, `eventName`.
2. Populate the database with some test data.

#### 2. Code Setup
1. Navigate to the project directory.
2. Install dependencies using:
   ```bash
   dotnet restore
   ```
3. Deploy the Lambda function to AWS:
   ```bash
   dotnet lambda deploy-function CheckEventCapacity
   ```

#### 3. AWS Setup
1. Create an SNS topic:
   ```bash
   aws sns create-topic --name Event-Publish
   ```
2. Subscribe the `CheckEventCapacity` Lambda function to the SNS topic (add CLI commands as needed).

3. Test the Lambda function by invoking it. Use the JSON example below:
   ```bash
   dotnet lambda invoke-function CheckEventCapacity --payload "This message is for the dotnet lambda function"
   ```
4. This command invokes the Lambda function manually.

## Example JSON for Lambda Function
Ensure you use your test data:

```json
{
  "Records": [
    {
      "EventSource": "aws:sns",
      "EventVersion": "1.0",
      "EventSubscriptionArn": "arn:{partition}:sns:EXAMPLE",
      "Sns": {
        "Type": "Notification",
        "MessageId": "95df01b4-ee98-5cb9-9903-4c221d41eb5e",
        "TopicArn": "arn:{partition}:sns:EXAMPLE",
        "Subject": "TestInvoke",
        "Message": "{ \"eventId\": \"66423a19ea0391811686ab25\", \"eventName\": \"event name\",  \"seats\": 200,  \"emailAddress\": \"test@test.com\" }",
        "Timestamp": "1970-01-01T00:00:00Z",
        "SignatureVersion": "1",
        "Signature": "EXAMPLE",
        "SigningCertUrl": "EXAMPLE",
        "UnsubscribeUrl": "EXAMPLE",
        "MessageAttributes": {
          "Test": {
            "Type": "String",
            "Value": "TestString"
          },
          "TestBinary": {
            "Type": "Binary",
            "Value": "TestBinary"
          }
        }
      }
    }
  ]
}
```

Feel free to adjust the specifics of the installation steps or example JSON as needed to match your project requirements.
